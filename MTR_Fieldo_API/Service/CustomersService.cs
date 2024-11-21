using Application.Common;
using Application.Models;
using Microsoft.EntityFrameworkCore;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service.IService;
using MTR_Fieldo_API.ViewModels;

namespace MTR_Fieldo_API.Service
{
    public class CustomersService : ICustomersService
    {
        private readonly MtrContext _context;
        private readonly ResponseDto _responseDto;
        private readonly IConfiguration _configuration;
        private static string bucketName;
        private readonly ICommonService _commonService;
        private readonly ITaskService _taskService;
        public CustomersService(MtrContext db, IConfiguration configuration, ICommonService CommonService, ITaskService taskService)
        {
            _context = db;
            _responseDto = new ResponseDto();
            _configuration = configuration;
            bucketName = (Convert.ToBoolean(_configuration.GetSection("Environment:Staging").Value) ? _configuration.GetSection("AWS:DEVBucketName").Value.ToString() : _configuration.GetSection("AWS:BucketName").Value.ToString());
            _commonService = CommonService;
            _taskService = taskService;

        }
        public async Task<ResponseDto> GetAllCustomers(int domainId)
        {
            try
            {
                var result = await _context.Fieldo_UserDetails
                                           .Where(x => x.IsActive && x.DomainId==domainId)
                                           .OrderByDescending(x=>x.CreatedAt)
                                           .ToListAsync();

                if (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        item.ProfileUrl = item.ProfileUrl != null ?
                            _commonService.GeneratePreSignedURL(bucketName, item.ProfileUrl, 7200) : "";
                    }
                    _responseDto.Result = result;
                    _responseDto.IsSuccess = true;
                    _responseDto.Message = "Customers retrieved successfully.";
                }
                else
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "No active customers found.";
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Error retrieving customers: {ex.Message}\n {ex.InnerException?.Message}";
            }

            return _responseDto;
        }

        public async Task<ResponseDto> GetCustomerById(int customerId, int domainId)
        {
            try
            {
                var result = await _context.Fieldo_UserDetails
                                           .FirstOrDefaultAsync(x => x.Id == customerId && x.IsActive && x.DomainId == domainId);
                if (result != null)
                {
                    CustomerDto res = new CustomerDto()
                    {
                        Id = result.Id,
                        FirstName = result.FirstName,
                        MiddleName = result.MiddleName,
                        LastName = result.LastName,
                        ProfileUrl = result.ProfileUrl,
                        Email = result.Email,
                        PhoneNumber = result.PhoneNumber,
                        CountryCode=result.CountryCode,
                        Password = result.Password,
                        key = result.ProfileUrl,
                        IsActive=result.IsActive,
                        DomainId =result.DomainId,
                        IsOnline=result.IsOnline,
                    };

                    res.ProfileUrl = !string.IsNullOrEmpty(result.ProfileUrl) ? _commonService.GeneratePreSignedURL(bucketName, result.ProfileUrl, 7200) : "";
                    _responseDto.Result = res;
                    _responseDto.IsSuccess = true;
                    _responseDto.Message = "Customer retrieved successfully.";
                }
                else
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Customer not found.";
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Error retrieving customer: {ex.Message}\n {ex.InnerException?.Message}";
            }

            return _responseDto;
        }
        public async Task<ResponseDto> DeleteCustomerById(int customerId, int domainId)
        {
            try
            {
                var customer = await _context.Fieldo_UserDetails
                                             .FirstOrDefaultAsync(x => x.Id == customerId
                                                                       && x.IsActive
                                                                       && x.DomainId == domainId);

                if (customer != null)
                {
                    customer.IsActive = false;

                    _context.Fieldo_UserDetails.Update(customer);
                    await _context.SaveChangesAsync();

                    _responseDto.IsSuccess = true;
                    _responseDto.Message = "Customer deleted successfully.";
                    _responseDto.Result = customer;
                }
                else
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Customer not found or already deleted.";
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Error deleting customer: {ex.Message}\n {ex.InnerException?.Message}";
            }

            return _responseDto;
        }


        public async Task<ResponseDto> AddOrUpdateServiceCustomers(CustomerDto customer, int domainId)
        {
            try
            {
                if (customer != null)
                {
                    if (customer.Id > 0)
                    {

                        var existingCustomer = await _context.Fieldo_UserDetails
                            .FirstOrDefaultAsync(c => c.Id == customer.Id && c.DomainId == domainId);

                        if (existingCustomer != null)
                        {
                            existingCustomer.FirstName = customer.FirstName;
                            existingCustomer.MiddleName = customer.MiddleName;
                            existingCustomer.LastName = customer.LastName;
                            existingCustomer.Email = customer.Email;
                            existingCustomer.PhoneNumber = customer.PhoneNumber;
                            existingCustomer.CountryCode = customer.CountryCode;
                            existingCustomer.ProfileUrl = customer.ProfileUrl;
                            existingCustomer.Password = customer.Password;
                            existingCustomer.IsOnline = customer.IsOnline;

                            // Save changes
                            _context.Fieldo_UserDetails.Update(existingCustomer);
                            await _context.SaveChangesAsync();

                            _responseDto.Result = existingCustomer;
                            _responseDto.Message = "Customer updated successfully";
                            _responseDto.IsSuccess = true;
                        }
                        else
                        {
                            _responseDto.Result = "";
                            _responseDto.Message = "Customer not found for update";
                            _responseDto.IsSuccess = false;
                        }
                    }
                    else
                    {
                        var roleId = (int)Role.User;
                        Fieldo_UserDetails newCustomer = new Fieldo_UserDetails
                        {
                            FirstName = customer.FirstName,
                            MiddleName = customer.MiddleName,
                            LastName = customer.LastName,
                            Email = customer.Email,
                            PhoneNumber = customer.PhoneNumber,
                            CountryCode = customer.CountryCode,
                            ProfileUrl = customer.ProfileUrl,
                            Password = customer.Password,
                            IsActive = true,
                            IsOnline = true,
                            CreatedAt = DateTime.Now,
                            DomainId = domainId,
                            RoleId = roleId,    
                            
                        };

                        await _context.Fieldo_UserDetails.AddAsync(newCustomer);
                        await _context.SaveChangesAsync();

                        _responseDto.Result = newCustomer;
                        _responseDto.Message = "Customer created successfully";
                        _responseDto.IsSuccess = true;
                    }
                }
                else
                {
                    _responseDto.Result = "";
                    _responseDto.Message = "Error while saving customer";
                    _responseDto.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Error: {ex.Message}\n {ex.InnerException?.Message}";
            }

            return _responseDto;
        }

    }
}
