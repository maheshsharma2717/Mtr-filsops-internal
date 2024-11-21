using Application.Common;
using Application.Models;
using MTR_Fieldo_API.Models.Dto;

namespace MTR_Fieldo_API.Service.IService
{
    public interface IUserService
    {
        Task<ResponseDto> GetAllUser(int roleId, int domainId);
        Task<ResponseDto> GetUser(int userId);
        Task<ResponseDto> UpdateUserDetails(UpdateUserRequestDto requestDto);
        Task<ResponseDto> UpdateWorkerStatus(int workerId, bool isOnline);
        Task<ResponseDto> UpdateUserProfile(ProfileDto profile, Fieldo_UserDetails fieldo_User);
        Task<ResponseDto> UpdateUserProfilePic(string imageKey, Fieldo_UserDetails fieldo_User);
        Task<ResponseDto> AddUserAddress(int userId, AddressDto address);
        Task<ResponseDto> DeleteAddress(int id, int userId);
        Task<ResponseDto> GetUserAddress(int userId);
        Task<ResponseDto> UpdateUserAddress(int id, int userId, AddressDto address);
        Task<ResponseDto> GetTotalNumberOfServiceSeekers();
        Task<ResponseDto> GetTotalNumberOfServiceProviders();
        Task<ResponseDto> DeleteUser(int userId);
        Task<ResponseDto> AddReview(UserReviewRequestDto userReview, int addedBy);
        Task<ResponseDto> GetUserReviews(int userId);
        Task<ResponseDto> GetServiceProviderDashboard(Fieldo_UserDetails user);
        Task<ResponseDto> GetUserDashboard(int userId, int domainId); 
       Task<ResponseDto> GetAllNotifications(int userId);
        Fieldo_UserDetails GetUserById(int id);
        Fieldo_UserDetails GetUserByEmail(string email);
        Task<ResponseDto> RegisterDevice(DeviceRegistrationRequest request, Fieldo_UserDetails user, int domainId);
    }
}
