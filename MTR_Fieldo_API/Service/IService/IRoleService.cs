using MTR_Fieldo_API.Models.Dto;

namespace MTR_Fieldo_API.Service.IService
{
    public interface IRoleService
    {
        Task<ResponseDto> AddRole(RoleRequestDto roleRequest);
    }
}
