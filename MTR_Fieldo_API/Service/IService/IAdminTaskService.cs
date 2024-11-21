using Application.Common;
using MTR_Fieldo_API.Models.Dto;

namespace MTR_Fieldo_API.Service.IService
{
    public interface IAdminTaskService
    {
        Task<ResponseDto> CreateTask(int adminUserId, int userId, TaskRequestDto taskRequest, int domainId, AdminUserType? adminUserType);
        Task<ResponseDto> CancelTask(int adminUserId, int taskId, int domainId, AdminUserType? adminUserType);
    }
}
