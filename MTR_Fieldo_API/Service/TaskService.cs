using Application.Common;
using Application.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service.IService;
using MTR_Fieldo_API.ViewModels;
using Stripe;
using System.Diagnostics;
using System.Threading.Tasks;
using Fieldo_Task = Application.Models.Fieldo_Task;

namespace MTR_Fieldo_API.Service
{
    public class TaskService : ITaskService
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
        public TaskService(MtrContext context, IMapper mapper, IConfiguration configuration,
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

        public async Task<ResponseDto> GetTaskById(int taskId, int domainId)
        {
            try
            {
                var t = await _context.Fieldo_Task
                    .Include(t => t.UserDetailsAssingedBy)
                     .Include(t => t.UserDetailsViewedBy)
                    .Include(t => t.UserDetailsAssignedTo)
                    .Include(t => t.UserDetailsCreatedBy)
                    .Include(t => t.UserDetailsUpdatedBy)
                    .Include(c => c.TaskCategory).Where(x => x.DomainId == domainId)
                    .FirstAsync(t => t.Id == taskId);


                TaskViewModel task = new()
                {
                    Description = t.Description,
                    Id = t.Id,
                    Name = t.Name,
                    Address=t.Address,
                    Documents=t.Documents,
                    Price = t.Price,
                    Status = t.Status,
                    ViewStatus = t.ViewStatus.ToString(),
                    ViewedTime = t.ViewedTime,
                    AssignedTime = t.AssignedTime,
                    CategoryName = t.TaskCategory.Name,
                    IsViewed = t.IsViewed,
                    PaymentStatus= t.PaymentStatus,
                    WorkCompleteTime=t.WorkCompleteTime,
                    WorkStartTime=t.WorkStartTime,
                    //AssignedTo = t.UserDetailsAssignedTo != null ? t.UserDetailsAssignedTo.Name : "",
                    //CreatedBy = t.UserDetailsCreatedBy != null ? t.UserDetailsCreatedBy.Name : "",
                    //UpdatedBy = t.UserDetailsUpdatedBy != null ? t.UserDetailsUpdatedBy.Name : "",
                    
                    AssignedBy = t.AssignedByUserId != null ? new()
                    {
                        Id= (int)t.AssignedByUserId,
                        Name = t.AssignedByUserName,
                        Email = "",
                        Role = t.AssignedByAdminUserType.ToString(),
                        ProfileUrl = ""
                    } : null,
                    AssignedTo = t.UserDetailsAssignedTo != null ? new()
                    {
                        Id = t.UserDetailsAssignedTo.Id,
                        Name = t.UserDetailsAssignedTo.FirstName + " " + t.UserDetailsAssignedTo.LastName,
                        Email = t.UserDetailsAssignedTo.Email,
                        Role = t.UserDetailsAssignedTo.Role?.Name,
                        ProfileUrl = !string.IsNullOrEmpty(t.UserDetailsAssignedTo.ProfileUrl)
                        ? _commonService.GeneratePreSignedURL(bucketName, t.UserDetailsAssignedTo.ProfileUrl, 7200)
                        : ""
                    } : null,
                    CreatedBy = t.UserDetailsCreatedBy != null ? new()
                    {
                        Id = t.UserDetailsCreatedBy.Id,
                        Name = t.UserDetailsCreatedBy.FirstName + " " + t.UserDetailsCreatedBy.LastName,
                        Email = t.UserDetailsCreatedBy.Email,
                        Role = t.UserDetailsCreatedBy.Role?.Name,
                        ProfileUrl = !string.IsNullOrEmpty(t.UserDetailsCreatedBy.ProfileUrl)
                        ? _commonService.GeneratePreSignedURL(bucketName, t.UserDetailsCreatedBy.ProfileUrl, 7200)
                        : ""
                    } : null,
                    UpdatedBy = t.UserDetailsUpdatedBy != null ? new()
                    {
                        Id = t.UserDetailsUpdatedBy.Id,
                        Name = t.UserDetailsUpdatedBy.FirstName + " " + t.UserDetailsUpdatedBy.LastName,
                        Email = t.UserDetailsUpdatedBy.Email,
                        Role = t.UserDetailsUpdatedBy.Role?.Name
                    } : null,
                    ViewedBy = t.ViewedByUserId != null ? new()
                    {
                        Id = (int)t.ViewedByUserId,
                        Name = t.ViewedByUserName,
                        Email = "",
                        Role = t.ViewedByAdminUserType.ToString()
                    } : null,


                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,

                };

                task.AttachmentUrls = _context.Fieldo_TaskAttachments
                    .Where(x => x.TaskId == taskId)
                    .Select(x => x.AttachmentUrl).ToList();

                _response.Result = task;//_mapper.Map<TaskViewModel>(task);
                _response.Message = "Task retrieved successfully.";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }

            return _response;
        }

        public async Task<ResponseDto> UpdateTaskAsViewedById(int taskId, int userId,AdminUserType? adminUserType)
        {
            try
            {
                string name = "";
                if (adminUserType == AdminUserType.Admin)
                {
                    var AdminUser = _context.Taxi_Employees.Where(x => x.Id == userId).FirstOrDefault();
                    name = AdminUser.FirstName + " " + AdminUser.LastName;
                }
                if (adminUserType == AdminUserType.SuperAdmin)
                {
                    var SuperAdminUser = _context.Taxi_User.Where(x => x.usr_id == userId).FirstOrDefault();
                    name = SuperAdminUser.usr_name;
                }

                var t = await _context.Fieldo_Task
                    .FirstAsync(t => t.Id == taskId);
                t.ViewedByUserId = userId;
                t.ViewedByUserName = name;
                t.ViewedByAdminUserType = adminUserType;
                t.IsViewed = true;
                t.ViewedTime = DateTime.Now;
             //   t.ViewedBy = userId;
                t.ViewStatus = Application.Common.RequestStatus.Viewed;
                _context.Update(t);
                await _context.SaveChangesAsync();
                _response.IsSuccess = true;
                _response.Message = "Task view status updated successfully.";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }

            return _response;
        }
        public async Task<ResponseDto> AssignTask(int taskId, int assignTo, int assignedBy, AdminUserType? adminUserType)
        {
            try
            {
                string name = "";
                if(adminUserType == AdminUserType.Admin)
                {
                   var AdminUser= _context.Taxi_Employees.Where(x => x.Id == assignedBy).FirstOrDefault();
                    name = AdminUser.FirstName + " " + AdminUser.LastName;
                }
                if (adminUserType == AdminUserType.SuperAdmin)
                {
                    var SuperAdminUser = _context.Taxi_User.Where(x => x.usr_id == assignedBy).FirstOrDefault();
                    name = SuperAdminUser.usr_name;
                }

                var task = _context.Fieldo_Task.FirstOrDefault(t => t.Id == taskId);
                if (task != null)
                {
                    task.AssignedTo = assignTo;
                    //task.UpdatedBy = assignedBy;
                    task.AssignedByUserId = assignedBy;
                    task.AssignedByUserName = name;
                    task.AssignedByAdminUserType = adminUserType;
                    task.Status = TasksStatus.Pending.ToString();
                    task.ViewStatus = Application.Common.RequestStatus.Assigned;
                    task.UpdatedAt = DateTime.Now;
                    task.AssignedAt = DateTime.Now;
                    _context.Fieldo_Task.Update(task);
                    await _context.SaveChangesAsync();

                    NotificationRequestDto notification = new()
                    {
                        Task = task,
                        UserId = assignTo,
                        Subject = "Task Assigned",
                        Description = $"{task.Name} has been assigned",
                    };

                    NotificationRequestDto notification2 = new()
                    {
                        Task = task,
                        UserId = task.CreatedBy,
                        Subject = "Your Task has been Assigned",
                        Description = $"{task.Name} has been assigned",
                    };


                    await _notificationService.AddNotification(notification);
                    await _notificationService.AddNotification(notification2);

                    await _firebaseNotifications.SendFirebaseNotification(notification.UserId, notification.Subject, notification.Description, WorkerNotificationType.TaskAssigned.ToString(), task.Id.ToString());

                    await _firebaseNotifications.SendFirebaseNotification(notification2.UserId, notification2.Subject, notification2.Description, CustomerNotificationType.TaskAssigned.ToString(), task.Id.ToString());
                    _response.Message = "Task assigned successfully";
                    _response.Result = notification;
                    _response.IsSuccess= true;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }
        public async Task<ResponseDto> UpdateTaskStatus(int userId, int taskId, TasksStatus status, int domainId)
        {
            try
            {
                var task = _context.Fieldo_Task
                    .FirstOrDefault(c => c.Id == taskId);
                if (task != null)
                {
                    if (status == TasksStatus.Requested)
                    {
                        task.ViewStatus = Application.Common.RequestStatus.NotViewed;
                    }
                    if (status == TasksStatus.Completed)
                    {
                        task.WorkCompleteTime = DateTime.Now;
                        task.ViewStatus = Application.Common.RequestStatus.Completed;
                    }
                    if (status == TasksStatus.Pending)
                    {
                        task.ViewStatus = Application.Common.RequestStatus.Assigned;
                    }
                    if (status == TasksStatus.Inprogress)
                    {
                        task.WorkStartTime = DateTime.Now;
                        task.ViewStatus = Application.Common.RequestStatus.Working;

                    }

                    string oldStatus = task.Status;

                    task.Status = status.ToString();
                    task.UpdatedAt = DateTime.Now;
                    task.UpdatedBy = userId;

                    _context.Fieldo_Task.Update(task);
                    await _context.SaveChangesAsync();

                    NotificationRequestDto notification = new()
                    {
                        Task = task,
                        UserId = task.CreatedBy,
                        Subject = "Task Status Updated",
                        Description = $"Task {task.Name} status updated to {status}",
                    };

                    await _notificationService.AddNotification(notification);
                    await _firebaseNotifications.SendFirebaseNotification(notification.UserId, notification.Subject, notification.Description, CustomerNotificationType.TaskUpdate.ToString(), task.Id.ToString());
                   
                   await _adminFirebaseNotifications.SendAdminFirebaseNotification(AdminUserType.Admin ,domainId, notification.Subject, notification.Description, AdminNotificationType.TaskUpdate.ToString());

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
        public async Task<ResponseDto> CreateTask(int userId, TaskRequestDto taskRequest, int domainId)
        {
            try
            {
                   Fieldo_Task task = new()
                    {
                        Description = taskRequest.Description,
                        Name = taskRequest.Name,
                        Price = taskRequest.Price,
                        Status = TasksStatus.Requested.ToString(),
                        ViewStatus = Application.Common.RequestStatus.NotViewed,
                        CategoryId = taskRequest.CategoryId,
                        //CreatedBy = userId,
                        CreatedBy = 260,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        Address = taskRequest.Address,
                        DomainId = domainId,
                        PaymentStatus = Application.Common.PaymentStatus.Pending.ToString(),
                        Documents = taskRequest.Documents != null ? string.Join(",", taskRequest.Documents) : null,
                        IsTaskCreatedByAdmin= false
                    };

                    await _context.Fieldo_Task.AddAsync(task);
                    await _context.SaveChangesAsync();

                    NotificationRequestDto notification = new()
                    {
                        Task = task,
                        UserId = task.CreatedBy,
                        Subject = "Task Created",
                        Description = "Task Created",
                    };

                    await _notificationService.AddNotification(notification);
                await _adminFirebaseNotifications.SendAdminFirebaseNotification(AdminUserType.Admin ,domainId, notification.Subject, notification.Description, AdminNotificationType.NewTask.ToString());

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
        public async Task<ResponseDto> GetAllTask(TasksStatus tasksStatus, int domainId)
        {
            try
            {
                var tasks = await _context.Fieldo_Task
                    .Include(x => x.UserDetailsAssignedTo)
                     .Include(x => x.UserDetailsViewedBy)
                    .Include(x => x.UserDetailsCreatedBy)
                    .Include(x => x.UserDetailsUpdatedBy)
                    .Include(u => u.UserDetailsAssingedBy)
                    .Include(u => u.TaskCategory).Where(x => x.DomainId == domainId)
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();

                if (tasksStatus != 0)
                {
                    tasks = tasks
                                .Where(t => t.Status == tasksStatus.ToString())
                                .ToList();
                }

                //List<string> docs = new List<string>();
                foreach (var task in tasks)
                {
                    string preSignedUrls = "";
                    if (!string.IsNullOrEmpty(task.Documents))
                    {
                        var documentList = task.Documents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        preSignedUrls = string.Join(", ", documentList
                            .Select(doc => _commonService.GeneratePreSignedURL(bucketName, doc.Trim(), 1200))
                            .ToList());
                    }

                    if (!string.IsNullOrEmpty(preSignedUrls))
                    {

                    }

                    task.Documents = preSignedUrls;
                }

                _response.Result = GetTasksViewModelList(tasks);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        public async Task<ResponseDto> GetUserTasks(int userId, TasksStatus tasksStatus, int domainId)
        {
            try
            {
                var tasks = await _context.Fieldo_Task
                                          .Include(u => u.UserDetailsCreatedBy)
                                          .Include(u => u.UserDetailsAssignedTo)
                                           .Include(u => u.UserDetailsViewedBy)
                                          .Include(u => u.UserDetailsUpdatedBy)
                                          .Include(u => u.UserDetailsAssingedBy)
                                           .Include(u => u.TaskCategory)
                                          .Where(t => t.CreatedBy == userId && t.DomainId == domainId)
                                          .ToListAsync();
                // List<string> docs = new List<string>();
                if (tasksStatus != 0)
                {
                    tasks = tasks
                                .Where(t => t.Status == tasksStatus.ToString())
                                .ToList();
                }
                foreach (var task in tasks)
                {
                    string preSignedUrls = "";
                    if (!string.IsNullOrEmpty(task.Documents))
                    {
                        var documentList = task.Documents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        preSignedUrls = string.Join(", ", documentList
                            .Select(doc => _commonService.GeneratePreSignedURL(bucketName, doc.Trim(), 1200))
                            .ToList());
                    }

                    if (!string.IsNullOrEmpty(preSignedUrls))
                    {

                    }

                    task.Documents = preSignedUrls;
                }
                _response.Result = GetTasksViewModelList(tasks);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        public async Task<ResponseDto> GetWorkerTasks(int workerId, TasksStatus tasksStatus, int domainId)
        {
            try
            {

                var tasks = await _context.Fieldo_Task
                                       .Include(u => u.UserDetailsCreatedBy)
                                        .Include(u => u.UserDetailsViewedBy)
                                       .Include(u => u.UserDetailsAssignedTo)
                                       .Include(u => u.UserDetailsUpdatedBy)
                                       .Include(u => u.UserDetailsAssingedBy)
                                       .Include(u => u.TaskCategory)
                                       .Where(t => t.AssignedTo == workerId && t.DomainId == domainId)
                                       .ToListAsync();

                if (tasksStatus != 0)
                {
                    tasks = tasks
                                .Where(t => t.Status == tasksStatus.ToString())
                                .ToList();
                }

                // List<string> docs = new List<string>();
                foreach (var task in tasks)
                {
                    string preSignedUrls = "";
                    if (!string.IsNullOrEmpty(task.Documents))
                    {
                        var documentList = task.Documents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        preSignedUrls = string.Join(", ", documentList
                            .Select(doc => _commonService.GeneratePreSignedURL(bucketName, doc.Trim(), 1200))
                            .ToList());
                    }

                    if (!string.IsNullOrEmpty(preSignedUrls))
                    {

                    }

                    task.Documents = preSignedUrls;
                }

                _response.Result = GetTasksViewModelList(tasks);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        public async Task<ResponseDto> GetWorkerTasksDetails(int workerId)
        {
            try
            {
                var workerTasks = await _context.Fieldo_Task
                       .Include(u => u.UserDetailsCreatedBy)
                                        .Include(u => u.UserDetailsViewedBy)
                                       .Include(u => u.UserDetailsAssignedTo)
                                       .Include(u => u.UserDetailsUpdatedBy)
                                       .Include(u => u.UserDetailsAssingedBy)
                                       .Include(u => u.TaskCategory)
                                    .Where(t => t.AssignedTo == workerId).ToListAsync();

                foreach (var task in workerTasks)
                {
                    string preSignedUrls = "";
                    if (!string.IsNullOrEmpty(task.Documents))
                    {
                        var documentList = task.Documents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        preSignedUrls = string.Join(", ", documentList
                            .Select(doc => _commonService.GeneratePreSignedURL(bucketName, doc.Trim(), 1200))
                            .ToList());
                    }

                    if (!string.IsNullOrEmpty(preSignedUrls))
                    {

                    }

                    task.Documents = preSignedUrls;
                }

                TaskWorkerViewModel workerTasksDetails = new()
                {
                    CompletedTask = workerTasks.Where(t => t.Status == TasksStatus.Completed.ToString()).Count(),
                    InProgressTask = workerTasks.Where(t => t.Status == TasksStatus.Inprogress.ToString()).Count(),
                    PendingTask = workerTasks.Where(t => t.Status == TasksStatus.Pending.ToString()).Count(),
                    TotalEarning = workerTasks.Where(t => t.Status == TasksStatus.Completed.ToString()).Sum(x => x.Price),
                    TotalTask = workerTasks.Count()
                };

                _response.Result = workerTasksDetails;
                _response.Message = "Details retrieved successfully";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }

            return _response;
        }
        public async Task<ResponseDto> UpdateWorkerTaskStatus(int taskId, WorkerTaskStatus workerTaskStatus)
        {
            try
            {
                var task = _context.Fieldo_Task.FirstOrDefault(x => x.Id == taskId);

                if (task != null)
                {
                    if (workerTaskStatus == WorkerTaskStatus.Accept)
                    {
                        task.AssignedTime = DateTime.Now;
                        task.WorkStartTime = DateTime.Now;
                        task.Status = TasksStatus.Inprogress.ToString();
                        task.ViewStatus = Application.Common.RequestStatus.Assigned;
                        _response.Message = "Task Accepted";
                    }
                    else
                    {
                        task.Status = TasksStatus.Requested.ToString();
                        _response.Message = "Task Rejected";
                    }
                    task.UpdatedAt = DateTime.Now;

                    Fieldo_WorkerTasks fieldo_WorkerTasks = new()
                    {
                        TaskId = task.Id,
                        AssignedByUserId = task.AssignedByUserId,
                        AssignedByUserName = task.AssignedByUserName,
                        AssignedTo = task.AssignedTo.Value,
                        TaskStatus = workerTaskStatus.ToString(),
                        CreatedDate = DateTime.Now,
                        UpdatedDated = DateTime.Now,
                        Task = task,
                    };

                    await _context.Fieldo_WorkerTasks.AddAsync(fieldo_WorkerTasks);
                    _context.Fieldo_Task.Update(task);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    throw new Exception("Task not found");
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }
        public async Task<ResponseDto> AddTaskProgressStatus(TaskStatusRequestDto taskStatusRequestDto)
        {
            try
            {
                var task = _context.Fieldo_Task.FirstOrDefault(t => t.Id == taskStatusRequestDto.TaskId);


                if (task != null)
                {

                    Fieldo_TaskStatus taskStatus = new()
                    {
                        TaskId = taskStatusRequestDto.TaskId,
                        UpdatedBy = taskStatusRequestDto.UpdatedBy,
                        Description = taskStatusRequestDto.Description,
                        Status = task.Status,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        TaskDetail = task
                    };

                    await _context.Fieldo_TaskStatus.AddAsync(taskStatus);
                    await _context.SaveChangesAsync();
                    await UploadFiles(taskStatusRequestDto, taskStatus.Id);

                    ;
                    NotificationRequestDto notification = new()
                    {
                        UserId = task.CreatedBy,
                        Task = task,
                        Subject = "Task Progress Updated",
                        Description = $"{taskStatusRequestDto.Description}",
                    };

                    await _notificationService.AddNotification(notification);
                    await _firebaseNotifications.SendFirebaseNotification(notification.UserId, notification.Subject, notification.Description,CustomerNotificationType.TaskUpdate.ToString(), task.Id.ToString());


                    _response.Message = "Status updated successfully";
                }
                else
                {
                    throw new Exception("Task not found");
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }
        public async Task<ResponseDto> GetTaskStatuses(int taskId)
        {
            try
            {
                var taskStatuses = await _context.Fieldo_TaskStatus
                    .Include(x => x.UserDetail)
                    .Where(x => x.TaskId == taskId).ToListAsync();

                List<TaskStatusViewModel> taskStatusesViewModels = new();
                List<String> attachments = new List<String>();
                foreach (var item in taskStatuses)
                {
                    var attachmentUrls = _context.Fieldo_TaskAttachments
                              .Where(x => x.TaskStatusId == item.Id)
                              .Select(x => x.AttachmentUrl)
                              .ToList();

                    foreach (var attachment in attachmentUrls)
                    {
                        var url = _commonService.GeneratePreSignedURL(bucketName, attachment.Trim(), 1200);
                        attachments.Add(url);
                    }
                    TaskStatusViewModel taskstatus = new()
                    {
                        TaskId = item.TaskId,
                        UpdatedBy = item.UserDetail.FirstName + " " + item.UserDetail.LastName,
                        Description = item.Description,
                        Status = item.Status,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt
                    };
                    taskstatus.Attachments = attachments;
                    taskStatusesViewModels.Add(taskstatus);
                }

                _response.Result = taskStatusesViewModels;
                _response.Message = "Data Retrieved successfully";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }
        private async Task UploadFiles(TaskStatusRequestDto taskStatusRequestDto, int taskStatusId)
        {
            Fieldo_UserDetails user = new()
            {
                Id = taskStatusRequestDto.UpdatedBy,
            };

            foreach (var item in taskStatusRequestDto.Files)
            {
                UploadClientFileDto upload = new()
                {
                    File = item,
                    FileName = item.FileName,
                };
                ResponseDto response = await _commonService.UploadFile(upload, user);
                if (response.Result != null)
                {
                    Dictionary<object, object> a = (Dictionary<object, object>)response.Result;

                    object url;
                    a.TryGetValue("key", out url);


                    Fieldo_TaskAttachment taskAttachment = new()
                    {
                        AddedBy = taskStatusRequestDto.UpdatedBy,
                        TaskId = taskStatusRequestDto.TaskId,
                        TaskStatusId = taskStatusId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        AttachmentUrl = url.ToString(),//response.Result.ToString()
                    };

                    await _context.Fieldo_TaskAttachments.AddAsync(taskAttachment);
                    await _context.SaveChangesAsync();

                }
            }
        }
        public List<TaskViewModel> GetTasksViewModelList(List<Fieldo_Task> tasks)
        {
            //var oldtask = tasks;
            //tasks = oldtask;
            List<TaskViewModel> tasksList = new List<TaskViewModel>();
            //tasks = tasks.ToList().Skip(56).Take(1).ToList();

            DateTime defaultDateTime = DateTime.MinValue;
            try
            {

                tasks.ForEach(x =>
                {
                    TaskViewModel task = new()
                    {
                        Description = x.Description,
                        Id = x.Id,
                        Address = x.Address,
                        Name = x.Name,
                        Price = x.Price,
                        Status = x.Status,
                        ViewStatus = x.ViewStatus.ToString(),
                        PaymentStatus = x.PaymentStatus,
                        AssignedBy = x.AssignedByUserId != null ? new()
                        {
                            Id= (int)x.AssignedByUserId,
                            Name = x.AssignedByUserName,
                            Email = "",
                            Role = x.AssignedByAdminUserType.ToString(),
                        //    ProfileUrl = !string.IsNullOrEmpty(x.UserDetailsCreatedBy.ProfileUrl)
                        //? _commonService.GeneratePreSignedURL(bucketName, x.UserDetailsCreatedBy.ProfileUrl, 7200)
                        //: ""
                        } : new UserDetailsViewModel(),
                        AssignedTo = x.UserDetailsAssignedTo != null ? new()
                        {
                            Id=x.UserDetailsAssignedTo.Id,
                            Name = x.UserDetailsAssignedTo.FirstName + " " + x.UserDetailsAssignedTo.LastName,
                            Email = x.UserDetailsAssignedTo.Email,
                            Role = x.UserDetailsAssignedTo.Role?.Name,
                            ProfileUrl = !string.IsNullOrEmpty(x.UserDetailsCreatedBy.ProfileUrl)
                        ? _commonService.GeneratePreSignedURL(bucketName, x.UserDetailsCreatedBy.ProfileUrl, 7200)
                        : ""
                        } : new UserDetailsViewModel(),
                        CreatedBy = x.UserDetailsCreatedBy != null ? new()
                        {
                            Id = x.UserDetailsCreatedBy.Id,
                            Name = x.UserDetailsCreatedBy.FirstName + " " + x.UserDetailsCreatedBy.LastName,
                            Email = x.UserDetailsCreatedBy.Email,
                            Role = x.UserDetailsCreatedBy.Role?.Name,
                            ProfileUrl = !string.IsNullOrEmpty(x.UserDetailsCreatedBy.ProfileUrl)
                        ? _commonService.GeneratePreSignedURL(bucketName, x.UserDetailsCreatedBy.ProfileUrl, 7200)
                        : ""
                        } : new UserDetailsViewModel(),
                        UpdatedBy = x.UserDetailsUpdatedBy != null ? new()
                        {
                            Id = x.UserDetailsUpdatedBy.Id,
                            Name = x.UserDetailsUpdatedBy.FirstName + " " + x.UserDetailsUpdatedBy.LastName,
                            Email = x.UserDetailsUpdatedBy.Email,
                            Role = x.UserDetailsUpdatedBy.Role?.Name,
                            ProfileUrl = !string.IsNullOrEmpty(x.UserDetailsCreatedBy.ProfileUrl)
                        ? _commonService.GeneratePreSignedURL(bucketName, x.UserDetailsCreatedBy.ProfileUrl, 7200)
                        : ""
                        } : new UserDetailsViewModel(),
                        //ViewedBy = x.UserDetailsViewedBy != null ? new()
                        //{
                        //    Name = x.UserDetailsViewedBy.FirstName + " " + x.UserDetailsUpdatedBy.LastName,
                        //    Email = x.UserDetailsViewedBy.Email,
                        //    Role = x.UserDetailsViewedBy.Role?.Name,
                        //    ProfileUrl = !string.IsNullOrEmpty(x.UserDetailsViewedBy.ProfileUrl)
                        //? _commonService.GeneratePreSignedURL(bucketName, x.UserDetailsViewedBy.ProfileUrl, 7200)
                        //: ""
                        //} : new UserDetailsViewModel(),

                        ViewedBy = x.ViewedByUserId != null ? new()
                        {
                            Id = (int)x.ViewedByUserId,
                            Name = x.ViewedByUserName,
                            Email = "",
                            Role = x.ViewedByAdminUserType.ToString()

                            //Name = (x.UserDetailsViewedBy?.FirstName ?? "") + " " + (x.UserDetailsUpdatedBy?.LastName ?? ""),
                            //Email = x.UserDetailsViewedBy?.Email ?? "",
                            //Role = x.UserDetailsViewedBy?.Role?.Name ?? "",
                            //ProfileUrl = !string.IsNullOrEmpty(x.UserDetailsViewedBy?.ProfileUrl)
                            //         ? _commonService.GeneratePreSignedURL(bucketName, x.UserDetailsViewedBy.ProfileUrl, 7200)
                            //         : ""
                        } : new UserDetailsViewModel(),


                        IsViewed = x.IsViewed,
                        Documents = x.Documents != null ? x.Documents : "",
                        ViewedTime = x.ViewedTime,
                        AssignedTime = x.AssignedTime,
                        WorkStartTime = x.WorkStartTime,
                        WorkCompleteTime = x.WorkCompleteTime,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt,
                        CategoryName = x.TaskCategory.Name,
                    };

                    tasksList.Add(task);
                });
            }
            catch (Exception ex)
            {

                throw;
            }
            return tasksList;
        }
        public async Task<ResponseDto> GetTasksByStatus(TasksStatus tasksStatus)
        {
            try
            {
                var tasks = await _context.Fieldo_Task
                                          .Include(u => u.UserDetailsCreatedBy)
                                          .Include(u => u.UserDetailsAssignedTo)
                                           .Include(u => u.UserDetailsViewedBy)
                                          .Include(u => u.UserDetailsUpdatedBy)
                                          .Include(u => u.UserDetailsAssingedBy)
                                           .Include(u => u.TaskCategory)
                                          .Where(t => t.Status == tasksStatus.ToString())
                                          .ToListAsync();
                List<string> docs = new();
                foreach (var task in tasks)
                {
                    docs = task.Documents != null && task.Documents.Any()
                        ? task.Documents
                        .Select(doc => doc.ToString()) // Convert each element to a string
                        .Select(doc => !string.IsNullOrEmpty(doc) ? _commonService.GeneratePreSignedURL(bucketName, doc, 7200) : "")
                        .ToList()
                        : new List<string>();
                    string concatenatedDocuments = string.Join(",", docs);
                    task.Documents = concatenatedDocuments;
                }
                _response.Result = GetTasksViewModelList(tasks);
                _response.Message = "Task retrieved successfully";
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
