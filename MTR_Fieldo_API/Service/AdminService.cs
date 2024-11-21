using Application.Common;
using Application.Models;
using Microsoft.EntityFrameworkCore;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service.IService;
using MTR_Fieldo_API.ViewModels;

namespace MTR_Fieldo_API.Service
{
    public class AdminService : IAdminService
    {
        private readonly MtrContext _context;
        private readonly ResponseDto _responseDto;
        private readonly IConfiguration _configuration;
        private static string bucketName;
        private readonly ICommonService _commonService;
        private readonly ITaskService _taskService;
        public AdminService(MtrContext db, IConfiguration configuration, ICommonService CommonService, ITaskService taskService)
        {
            _context = db;
            _responseDto = new ResponseDto();
            _configuration = configuration;
            bucketName = (Convert.ToBoolean(_configuration.GetSection("Environment:Staging").Value) ? _configuration.GetSection("AWS:DEVBucketName").Value.ToString() : _configuration.GetSection("AWS:BucketName").Value.ToString());
            _commonService = CommonService;
            _taskService = taskService;

        }

        public async Task<ResponseDto> AddServiceCategory(Fieldo_UserDetails user, RequestCategoryDto requestCategory, int domainId)
        {
            try
            {
                if(await _context.Fieldo_RequestCategory.Where(x=>x.Name.ToLower().Trim() == requestCategory.Name.ToLower().Trim() && x.IsActive==true&&x.IsDeleted==false&& x.DomainId==domainId).AnyAsync())
                {
                    _responseDto.Result = "";
                    _responseDto.Message = "Category Already Exist";
                    _responseDto.IsSuccess = false;
                    return _responseDto;
                }
                Fieldo_RequestCategory category = new Fieldo_RequestCategory
                {
                    Name = requestCategory.Name,
                    Icon = requestCategory.Icon,
                    //CreatedBy = user.Id,
                    CreatedAt = DateTime.Now,
                    DomainId = domainId,
                    IsActive = true,
                    IsDeleted = false,
                    Description = requestCategory.Description,


                };

                await _context.Fieldo_RequestCategory.AddAsync(category);
                await _context.SaveChangesAsync();
                _responseDto.Result = "";
                _responseDto.Message = "Category created successfully";
                _responseDto.IsSuccess = true;

            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> DeleteServiceCategory(Fieldo_UserDetails user, int id)
        {
            try
            {
                var result = await _context.Fieldo_RequestCategory.FindAsync(id);
                if (result != null)
                {
                    result.IsDeleted = true;
                    result.IsActive = false;
                    _context.Update(result);
                    _context.SaveChanges();
                    _responseDto.IsSuccess = true;
                    _responseDto.Message = "Category deleted successfully";
                    return _responseDto;
                }
                else
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Record not found with this Id";
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;


        }

        public async Task<ResponseDto> GetAllServiceCategories(int domainId)
        {
            try
            {
                var result = await _context.Fieldo_RequestCategory.Where(x => x.IsActive == true && x.IsDeleted == false &&x.DomainId==domainId)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        item.Icon = !string.IsNullOrEmpty(item.Icon) ? _commonService.GeneratePreSignedURL(bucketName, item.Icon, 7200) : "";
                    }
                    _responseDto.Result = result;
                    _responseDto.IsSuccess = true;
                    _responseDto.Message = "";
                    return _responseDto;
                }
                else
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> GetServiceCategoryById(int id)
        {
            try
            {
                var result = await _context.Fieldo_RequestCategory.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
                if (result != null)
                {
                    result.Icon = !string.IsNullOrEmpty(result.Icon) ? _commonService.GeneratePreSignedURL(bucketName, result.Icon, 7200) : "";
                    _responseDto.Result = result;
                    _responseDto.IsSuccess = true;
                    _responseDto.Message = "";
                    return _responseDto;
                }
                else
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;
        }

     public async Task<ResponseDto> UpdateServiceCategory(Fieldo_UserDetails user, int id, string newCategoryName, string newDescription)
{
    try
    {
        var result = await _context.Fieldo_RequestCategory.FindAsync(id);
        if (result != null)
        {
            result.Name = newCategoryName;
            result.Description = newDescription;
            result.UpdatedAt = DateTime.Now; 

            _context.Update(result);
            await _context.SaveChangesAsync();

            _responseDto.IsSuccess = true;
            _responseDto.Message = "Category updated successfully";
            return _responseDto;
        }
        else
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Record not found with this Id";
        }
    }
    catch (Exception ex)
    {
        _responseDto.IsSuccess = false;
        _responseDto.Message = $"{ex.Message}\n {ex.InnerException?.Message}";
    }
    return _responseDto;
}

        public async Task<ResponseDto> UpdateServiceCategoryIcon(Fieldo_UserDetails user, int id, string newCategoryIcon)
        {
            try
            {
                var result = await _context.Fieldo_RequestCategory.FindAsync(id);
                if (result != null)
                {
                    result.Icon = newCategoryIcon;
                    _context.Update(result);
                    _context.SaveChanges();
                    _responseDto.IsSuccess = true;
                    _responseDto.Message = "Category Icon Updated Successfully";
                    return _responseDto;
                }
                else
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Record not found with this Id";
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;
        }
        public async Task<ResponseDto> GetDashboardDetals(int domainId)
        {
            try
            {
                //await _context.Fieldo_Task.Where(t => t.Status == TasksStatus.Completed.ToString() && t.DomainId == domainId).CountAsync(),
                //
                var tasks = await _context.Fieldo_Task.AsQueryable().Select(x => new { x.Status, x.DomainId }).ToListAsync();
                //var created = tasks.Count(x => x.Status == "Created");
                DashboardDataViewModel dashboardData = new()
                {
                    TotalCustomer = await _context.Fieldo_UserDetails.Where(u => u.IsActive && u.RoleId == 2 && u.DomainId == domainId).CountAsync(),
                    TotalWorkers = await _context.Fieldo_UserDetails.Where(u => u.RoleId == 3 && u.IsActive && u.DomainId == domainId).CountAsync(),
                    CompletedTasks = tasks.Count(t => t.Status == TasksStatus.Completed.ToString() && t.DomainId == domainId),
                    TotalPendingTasks = tasks.Count(t => (t.Status == TasksStatus.Requested.ToString() || t.Status == TasksStatus.Pending.ToString() || t.Status == "Created") && t.DomainId == domainId),
                    OngoingTasks = tasks.Count(t => t.Status == TasksStatus.Inprogress.ToString() && t.DomainId == domainId),
                    CanceledTasks = tasks.Count(t => t.Status == TasksStatus.Cancelled.ToString()),
                    RecentTasks = [],
                };

                var recentTasks = await _context.Fieldo_Task
                    .OrderByDescending(t => t.CreatedAt)
                    .Take(5)
                    .Include(x => x.UserDetailsAssignedTo)
                    .Include(x => x.UserDetailsCreatedBy)
                    .Include(x => x.UserDetailsUpdatedBy)
                    .Include(u => u.UserDetailsAssingedBy)
                    .Include(c => c.TaskCategory)
                    .ToListAsync();

                dashboardData.RecentTasks = _taskService.GetTasksViewModelList(recentTasks);

                _responseDto.Result = dashboardData;
                _responseDto.Message = "Dashboard data retrieved successfully";

            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;

        }
        public async Task<ResponseDto> RegisterDevice(DeviceRegistrationRequest request, int userId, AdminUserType? adminUserType, int domainId)
        {
            try
            {
                // Check if this token already exists for the user
                var existingToken =await _context.FieldOps_AdminDeviceToken.FirstOrDefaultAsync(t => t.UserID == userId && t.DeviceToken == request.DeviceToken);

                if (existingToken!= null)
                {
                    if(existingToken.DeviceToken == request.DeviceToken)
                    {
                       _responseDto.Message = "device token is same";
                        _responseDto.IsSuccess = true;
                    }
                    // Add a new token if it's not already in the database
                    
                }
                else
                {
                    _context.FieldOps_AdminDeviceToken.Add(new FieldOps_AdminDeviceToken
                    {
                        UserID = userId,
                        DeviceToken = request.DeviceToken,
                        DeviceType = request.DeviceType,
                        LastUpdated = DateTime.Now,
                        DomainId = domainId,
                        AdminUserType = adminUserType

                    });
                    _context.SaveChanges();
                    _responseDto.Message = "device added successfully";
                    _responseDto.IsSuccess = true;
                }
               

               
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;
            

        }
          
        
    }
}
