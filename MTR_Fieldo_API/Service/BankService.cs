using Application.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service.IService;
using MTR_Fieldo_API.ViewModels;

namespace MTR_Fieldo_API.Service
{
    public class BankService : IBankService
    {
        private readonly MtrContext _context;
        private readonly IMapper _mapper;

        private readonly ResponseDto _response;

        public BankService(MtrContext mtrContext, IMapper mapper)
        {
            _context = mtrContext;
            _mapper = mapper;
            _response = new();
        }

        public async Task<ResponseDto> GetAllBanks()
        {
            try
            {
                _response.Result = await _context.Fieldo_Banks.Where(b => b.IsActive).AsNoTracking().ToListAsync();
                _response.Message = "Banks retrieved successfully";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }
        public async Task<ResponseDto> GetAllWorkerBanks()
        {
            try
            {
                var res = await _context.Fieldo_WorkerBankDetails
                    .Include(wb => wb.Bank)
                    .Select(wb => new
                    {
                        wb.Id,
                        wb.UserId,
                        wb.FirstName,
                        wb.MiddleName,
                        wb.LastName,
                        wb.LastName2,
                        wb.AccountNumber,
                        wb.RoutingNumber,
                        wb.AccountType,
                        wb.IsActive,
                        wb.CreatedAt,
                        wb.UpdatedAt,
                        wb.OtherBankName,
                        wb.OtherAccountType,
                        wb.DomainId,
                        wb.BankId,
                        BankName = wb.Bank != null ? wb.Bank.BankName : wb.OtherBankName
                    })
                    .ToListAsync();
                if (res != null)
                {
                    _response.Result = res;
                    _response.Message = "Banks retrieved successfully";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }


        public async Task<ResponseDto> AddUserBankDetails(BankDetailsRequestDto bankDetailsRequestDto)
        {
            try
            {
                var bankDetails = await _context.Fieldo_WorkerBankDetails.Where(b => b.UserId == bankDetailsRequestDto.WorkerId).ToListAsync();
                foreach (var item in bankDetails)
                {
                    item.IsActive = false;
                    _context.Fieldo_WorkerBankDetails.Update(item);
                }

                Fieldo_WorkerBankDetails workerBankDetails = _mapper.Map<Fieldo_WorkerBankDetails>(bankDetailsRequestDto);
                workerBankDetails.UserId = bankDetailsRequestDto.WorkerId;
                workerBankDetails.AccountType = bankDetailsRequestDto.AccountType.ToString();
                workerBankDetails.OtherAccountType = bankDetailsRequestDto.OtherAccountType;
                workerBankDetails.BankId = bankDetailsRequestDto.BankId;

                workerBankDetails.IsActive = true;

                workerBankDetails.CreatedAt = DateTime.Now;
                workerBankDetails.UpdatedAt = DateTime.Now;
                await _context.Fieldo_WorkerBankDetails.AddAsync(workerBankDetails);
                await _context.SaveChangesAsync();
                _response.Message = "Bank details added successfully";

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }

        public async Task<ResponseDto> ChangeActiveBankStatus(int workerBankDetailId, int userId)
        {
            try
            {
                var bankDetails = await _context.Fieldo_WorkerBankDetails.Where(b => b.UserId == userId).ToListAsync();
                if (bankDetails.Count == 0)
                {
                    throw new Exception("No bank details found");
                }
                foreach (var item in bankDetails)
                {
                    if (item.Id == workerBankDetailId)
                    {
                        item.IsActive = true;
                        item.UpdatedAt = DateTime.Now;
                    }
                    else
                    {
                        if (item.IsActive == true)
                        {
                            item.UpdatedAt = DateTime.Now;
                        }
                        item.IsActive = false;
                    }
                    _context.Fieldo_WorkerBankDetails.Update(item);
                }
                await _context.SaveChangesAsync();
                _response.Message = "Bank details status updated";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }

        public async Task<ResponseDto> GetBankDetailsByUserId(int userId)
        {
            try
            {
                var bankDetails = await _context.Fieldo_WorkerBankDetails
                    .Include(b => b.Bank)
                    .Include(u => u.UserDetailsWorker)
                    .Where(x => x.UserId == userId).ToListAsync();

                List<UserBankDetailsViewModel> bankDetailsList = new();
                bankDetails.ForEach(bankDetail =>
                {
                    var bankDetailsViewModel = _mapper.Map<UserBankDetailsViewModel>(bankDetail);
                    bankDetailsViewModel.BankName = bankDetail.Bank?.BankName ?? "";
                    bankDetailsViewModel.UserName = bankDetail.UserDetailsWorker.FirstName + " " + bankDetail.UserDetailsWorker.LastName;

                    bankDetailsList.Add(bankDetailsViewModel);
                });
                _response.Message = "success";
                _response.IsSuccess = true;
                _response.Result = bankDetailsList;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }

        public async Task<ResponseDto> AddBank(BankRequestDto bankRequest)
        {
            try
            {
                Fieldo_Banks _bank = new()
                {
                    BankName = bankRequest.BankName,
                    BankCode = bankRequest.BankCode,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsActive = true,
                };

                await _context.Fieldo_Banks.AddAsync(_bank);
                await _context.SaveChangesAsync();
                _response.Message = "Bank added successfully";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }
        //public async Task<ResponseDto> AddUpdateWorkerBank(AddUpdateBankDto bankDetails)
        //{
        //    try
        //    {
        //        Fieldo_Banks _bank = new()
        //        {
        //            BankName = bankRequest.BankName,
        //            BankCode = bankRequest.BankCode,
        //            CreatedAt = DateTime.Now,
        //            UpdatedAt = DateTime.Now,
        //            IsActive = true,
        //        };

        //        await _context.Fieldo_Banks.AddAsync(_bank);
        //        await _context.SaveChangesAsync();
        //        _response.Message = "Bank added successfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
        //    }
        //    return _response;
        //}

        public async Task<ResponseDto> UpdateBank(BankRequestDto bankRequest)
        {
            try
            {
                Fieldo_Banks _bank = await _context.Fieldo_Banks
                                      .FirstAsync(b => b.Id == bankRequest.Id && b.IsActive);

                _bank.BankName = bankRequest.BankName;
                _bank.BankCode = bankRequest.BankCode;
                _bank.UpdatedAt = DateTime.Now;


                _context.Fieldo_Banks.Update(_bank);
                await _context.SaveChangesAsync();
                _response.Message = "Bank updated successfully";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }

        public async Task<ResponseDto> GetBankById(int bankId)
        {
            try
            {
                Fieldo_Banks _bank = await _context.Fieldo_Banks
                                            .AsNoTracking()
                                            .FirstAsync(b => b.Id == bankId && b.IsActive);
                _response.Result = _bank;
                _response.Message = "Bank retrieved successfully";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }

        public async Task<ResponseDto> DeleteBank(int bankId)
        {
            try
            {
                Fieldo_Banks _bank = await _context.Fieldo_Banks
                                      .FirstAsync(b => b.Id == bankId && b.IsActive);

                _bank.IsActive = false;
                _bank.UpdatedAt = DateTime.Now;


                _context.Fieldo_Banks.Update(_bank);
                await _context.SaveChangesAsync();
                _response.Message = "Bank deleted successfully";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }

        public async Task<ResponseDto> GetBankByIdAndUserID(int bankId, int UserId, int domainId)
        {
            try
            {
                var res = await _context.Fieldo_WorkerBankDetails
                     .Include(wb => wb.Bank)
                .Select(wb => new
                {
                    wb.Id,
                    wb.UserId,
                    wb.FirstName,
                    wb.MiddleName,
                    wb.LastName,
                    wb.LastName2,
                    wb.AccountNumber,
                    wb.RoutingNumber,
                    wb.AccountType,
                    wb.IsActive,
                    wb.CreatedAt,
                    wb.UpdatedAt,
                    wb.OtherBankName,
                    wb.OtherAccountType,
                    wb.DomainId,
                    wb.BankId,
                    BankName = wb.Bank != null ? wb.Bank.BankName : wb.OtherBankName
                })
                    .ToListAsync();

                var bank = res.Where(x => x.UserId == UserId && x.BankId == bankId).FirstOrDefault();
                if (res != null)
                {
                    _response.Result = bank;
                    _response.Message = "Banks retrieved successfully";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }
    }
}
