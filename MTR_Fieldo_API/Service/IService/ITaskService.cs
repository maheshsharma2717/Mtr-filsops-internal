using Application.Common;
using Application.Models;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.ViewModels;

namespace MTR_Fieldo_API.Service.IService
{
    public interface ITaskService
    {
        Task<ResponseDto> AssignTask(int taskId, int assignTo, int assignedBy, AdminUserType? adminUserType);
        Task<ResponseDto> CreateTask(int userId, TaskRequestDto taskRequest, int domainId);
        Task<ResponseDto> GetAllTask(TasksStatus tasksStatus, int domainId);
        Task<ResponseDto> GetUserTasks(int userId, TasksStatus tasksStatus, int domainId);
        Task<ResponseDto> GetWorkerTasks(int workerId, TasksStatus tasksStatus, int domainId);
        Task<ResponseDto> UpdateTaskStatus(int userId, int taskId, TasksStatus changeStatus, int domainId);
        Task<ResponseDto> GetTaskById(int taskId, int domainId);
        Task<ResponseDto> UpdateTaskAsViewedById(int taskId, int userId, AdminUserType? adminUserType);
        List<TaskViewModel> GetTasksViewModelList(List<Fieldo_Task> tasks);
        Task<ResponseDto> GetWorkerTasksDetails(int workerId);
        Task<ResponseDto> UpdateWorkerTaskStatus(int taskId, WorkerTaskStatus workerTaskStatus);
        Task<ResponseDto> AddTaskProgressStatus(TaskStatusRequestDto taskStatusRequestDto);
        Task<ResponseDto> GetTaskStatuses(int taskId);

    }
}
