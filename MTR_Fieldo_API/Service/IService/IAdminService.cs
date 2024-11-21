using Application.Common;
using Application.Models;
using MTR_Fieldo_API.Models.Dto;

namespace MTR_Fieldo_API.Service.IService
{
    public interface IAdminService
    {
        Task<ResponseDto> AddServiceCategory(Fieldo_UserDetails user, RequestCategoryDto category, int domainId);
        Task<ResponseDto> GetAllServiceCategories(int domainId);
        Task<ResponseDto> GetServiceCategoryById(int id);
        Task<ResponseDto> UpdateServiceCategory(Fieldo_UserDetails user, int id, string newCategoryName,string newDescription);
        Task<ResponseDto> UpdateServiceCategoryIcon(Fieldo_UserDetails user, int id, string newCategoryIcon);
        Task<ResponseDto> DeleteServiceCategory(Fieldo_UserDetails user, int id);
        Task<ResponseDto> GetDashboardDetals(int domainId);
        Task<ResponseDto> RegisterDevice(DeviceRegistrationRequest request, int user, AdminUserType? adminUserType, int domainId);
    }
}
