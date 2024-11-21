using Application.Common;
using Application.Models;
using AutoMapper;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service.IService;

namespace MTR_Fieldo_API.Service
{
    public class AdminTaskService : IAdminTaskService
    {
        private readonly MtrContext _context;
        private readonly ResponseDto _response;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ICommonService _commonService;
        private readonly INotificationService _notificationService;
        private readonly IFirebaseNotifications _firebaseNotifications;
        private readonly IAdminFirebaseNotifications _adminFirebaseNotifications;

        private static string bucketName;

        public AdminTaskService(MtrContext context, IMapper mapper, IConfiguration configuration,
            ICommonService commonService, IFirebaseNotifications firebaseNotifications, INotificationService notificationService, IAdminFirebaseNotifications adminFirebaseNotifications)
        {
            _context = context;
            _mapper = mapper;
            _response = new ResponseDto();
            _configuration = configuration;
            bucketName = (Convert.ToBoolean(_configuration.GetSection("Environment:Staging").Value) ? _configuration.GetSection("AWS:DEVBucketName").Value.ToString() : _configuration.GetSection("AWS:BucketName").Value.ToString());
            _commonService = commonService;
            _notificationService = notificationService;
            _firebaseNotifications = firebaseNotifications;
            _adminFirebaseNotifications = adminFirebaseNotifications;
        }
        public async Task<ResponseDto> CreateTask(int adminUserId, int userId, TaskRequestDto taskRequest, int domainId, AdminUserType? adminUserType)
        {
            try
            {
                string name = "";
                if (adminUserType == AdminUserType.Admin)
                {
                    var AdminUser = _context.Taxi_Employees.Where(x => x.Id == adminUserId).FirstOrDefault();
                    name = AdminUser.FirstName + " " + AdminUser.LastName;
                }
                if (adminUserType == AdminUserType.SuperAdmin)
                {
                    var SuperAdminUser = _context.Taxi_User.Where(x => x.usr_id == adminUserId).FirstOrDefault();
                    name = SuperAdminUser.usr_name;
                }

                Fieldo_Task task = new()
                {
                    Description = taskRequest.Description,
                    Name = taskRequest.Name,
                    Price = taskRequest.Price,
                    Status = TasksStatus.Requested.ToString(),
                    ViewStatus = Application.Common.RequestStatus.NotViewed,
                    CreatedBy = userId,
                    CategoryId = taskRequest.CategoryId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Address = taskRequest.Address,
                    DomainId = domainId,
                    PaymentStatus = Application.Common.PaymentStatus.Pending.ToString(),
                    Documents = taskRequest.Documents != null ? string.Join(",", taskRequest.Documents) : null,
                    CreatedByAdminUserType = adminUserType,
                    CreatedByAdminUserId = adminUserId,
                    IsTaskCreatedByAdmin=true,
                    CreatedByAdminUserName= name
                };

                await _context.Fieldo_Task.AddAsync(task);
                await _context.SaveChangesAsync();

                NotificationRequestDto notification = new()
                {
                    Task = task,
                    UserId = task.CreatedBy,
                    Subject = "Task Created",
                    Description = "Task Created By Admin on behalf yof you",
                };

                await _notificationService.AddNotification(notification);
                await _firebaseNotifications.SendFirebaseNotification(notification.UserId, notification.Subject, notification.Description, CustomerNotificationType.TaskUpdate.ToString());

                _response.Result = task;
                _response.Message = "Task created successfully";
                //  }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }
        public async Task<ResponseDto> CancelTask(int adminUserId, int taskId, int domainId, AdminUserType? adminUserType)
        {
            try
            {
                string name = "";
                if (adminUserType == AdminUserType.Admin)
                {
                    var AdminUser = _context.Taxi_Employees.Where(x => x.Id == adminUserId).FirstOrDefault();
                    name = AdminUser.FirstName + " " + AdminUser.LastName;
                }
                if (adminUserType == AdminUserType.SuperAdmin)
                {
                    var SuperAdminUser = _context.Taxi_User.Where(x => x.usr_id == adminUserId).FirstOrDefault();
                    name = SuperAdminUser.usr_name;
                }
                var task = _context.Fieldo_Task
                    .FirstOrDefault(c => c.Id == taskId);
                if (task != null)
                {
                    
                     
               task.ViewStatus = Application.Common.RequestStatus.Cancelled;
                  task.Status = TasksStatus.Cancelled.ToString();
                    task.UpdatedAt = DateTime.Now;
                    task.IsTaskCancelledByAdmin = true;
                    task.CancellationDateTime = DateTime.Now;
                    task.CancelledByAdminUserId = adminUserId;
                    task.CancelledByAdminUserType = adminUserType;
                    task.CancelledByAdminUserName = name;

                    _context.Fieldo_Task.Update(task);
                    await _context.SaveChangesAsync();

                    NotificationRequestDto notification = new()
                    {
                        Task = task,
                        UserId = task.CreatedBy,
                        Subject = "Task has been cancelled",
                        Description = $"Task {task.Name} has been cancelled",
                    };

                    await _notificationService.AddNotification(notification);
                    await _firebaseNotifications.SendFirebaseNotification(notification.UserId, notification.Subject, notification.Description, CustomerNotificationType.TaskUpdate.ToString(), task.Id.ToString());

                   
                    _response.IsSuccess = true;
                    _response.Message = "Task status changed successfully";
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
