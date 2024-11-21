using Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service;
using MTR_Fieldo_API.Service.IService;
using MTR_Fieldo_API.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MTR_Fieldo_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [ValidateDomainIdFilter]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IAuthenticateService _authenticateService;
        private readonly ResponseDto _responseDto;
        public TaskController(IAuthenticateService authenticateService, ITaskService taskService)
        {
            _taskService = taskService;
            _authenticateService = authenticateService;
            _responseDto = new();
        }


        [HttpGet]
        [Route("GetTaskById/{taskId}")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetTask(int taskId, int domainId)
        {
            return await _taskService.GetTaskById(taskId, domainId);
        }


        [HttpPut("updatetaskviewedbyid/{taskId}")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> UpdateTaskViewedById(int taskId, int domainId)
        {
            // Retrieve the Id claim
            string userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            // Retrieve the Role claim
            string roleClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            AdminUserType? adminUserType = _authenticateService.GetAdminUserTypeFromClaims(roleClaim);

            if (userId != null && roleClaim != null && adminUserType != null)
            {
                var userDetails = await _authenticateService.GetAdminSuperAdminUserDetailsAsync(HttpContext.User);
                if (userDetails != null)
                {
                    if (adminUserType != null)
                    {
                    
                        return await _taskService.UpdateTaskAsViewedById(taskId, Convert.ToInt32(userId), adminUserType);

                    }
                    else
                    {
                        _responseDto.IsSuccess = false;
                        _responseDto.Message = "Claims not found";
                        return _responseDto;
                    }
                }
                else
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Claims not found";
                    return _responseDto;
                }
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }





  
          
        }

        [HttpPost("CreateTask")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> CreateTask(TaskRequestDto taskRequest, int domainId)
        {
            //int userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type.Equals("id", StringComparison.InvariantCultureIgnoreCase))?.Value);
            int userId = GetCurrentUserId();
            return await _taskService.CreateTask(userId, taskRequest, domainId);
        }

        [HttpGet("GetAllTask")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetAllTask(TasksStatus tasksStatus, int domainId)
        {
            return await _taskService.GetAllTask(tasksStatus,domainId);
        }

        [HttpGet]
        [Route("GetUserTasksByUserIdAndByStatus")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetUserTasks(int userId, TasksStatus taskStatus, int domainId)
        {
            return await _taskService.GetUserTasks(userId, taskStatus, domainId);
        }

        [HttpGet]
        [Route("GetWorkerTasksByWorkerIdAndByStatus")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetWorkerTasks(int workerId, TasksStatus taskStatus, int domainId)
        {
            return await _taskService.GetWorkerTasks(workerId, taskStatus, domainId);
        }


        [HttpPost]
        //[Authorize(Policy = "Administrator")]
        [Authorize]
        [Route("AssignTask")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> AssignTaskTo(int taskId, int assignTo, int domainId)
        {
            // Retrieve the Id claim
            string userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            // Retrieve the Role claim
            string roleClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            AdminUserType? adminUserType = _authenticateService.GetAdminUserTypeFromClaims(roleClaim);

            if (userId != null && roleClaim != null && adminUserType != null)
            {
                var userDetails = await _authenticateService.GetAdminSuperAdminUserDetailsAsync(HttpContext.User);
                if (userDetails != null)
                {
                    if (adminUserType != null)
                    {
                       return await _taskService.AssignTask(taskId, assignTo, Convert.ToInt32(userId), adminUserType);

                    }
                    else
                    {
                        _responseDto.IsSuccess = false;
                        _responseDto.Message = "Claims not found";
                        return _responseDto;
                    }
                }
                else
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Claims not found";
                    return _responseDto;
                }
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }


        }

        [HttpPost]
        [Route("UpdateTaskStatus")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> UpdateTaskStatus(int taskId, TasksStatus taskStatus, int domainId)
        {
            //int updatedBy = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type.Equals("id", StringComparison.InvariantCultureIgnoreCase))?.Value);
            int updatedBy = GetCurrentUserId();
            return await _taskService.UpdateTaskStatus(updatedBy, taskId, taskStatus,domainId);
        }

        [HttpGet]
        [Route("WorkerTasksDetailsByWorkerId/{workerId}")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetWorkerTaskDetails(int workerId, int domainId)
        {
            return await _taskService.GetWorkerTasksDetails(workerId);
        }

        [HttpPost]
        [Route("UpdateWorkerTaskStatus")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> UpdateWorkerTaskStatus(int taskId, WorkerTaskStatus workerTaskStatus, int domainId)
        {
            return await _taskService.UpdateWorkerTaskStatus(taskId, workerTaskStatus);
        }

        [HttpPost]
        [Route("AddTaskCurrentStatus")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> AddTaskCurrentStatus(TaskStatusRequestDto taskStatusRequestDto, int domainId)
        {
            taskStatusRequestDto.UpdatedBy = GetCurrentUserId();
            return await _taskService.AddTaskProgressStatus(taskStatusRequestDto);
        }

        [HttpGet]
        [Route("GetTaskStatuses")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetTaskStatuses(int taskId, int domainId)
        {
            return await _taskService.GetTaskStatuses(taskId);
        }

        private int GetCurrentUserId()
        {
            return Convert.ToInt32(
                User.Claims
                .FirstOrDefault(c => c.Type.Equals("id", StringComparison.InvariantCultureIgnoreCase))?.Value);
        }

    }
}
