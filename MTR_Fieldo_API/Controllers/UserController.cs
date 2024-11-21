using Application.Common;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service;
using MTR_Fieldo_API.Service.IService;
using System.Security.Claims;

namespace MTR_Fieldo_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly IUserService _userServices;
        private readonly INotificationService _notificationService;
        private readonly ResponseDto _responseDto;

        public UserController(INotificationService notificationService, IUserService userService, IAuthenticateService authenticateService/*, IUserService userServicecs*//*, ResponseDto responseDto*/)
        {
            _userServices = userService;
            _authenticateService = authenticateService;
            // _userServicecs = userServicecs;
            _responseDto = new ResponseDto();
            _notificationService = notificationService;
        }
        [HttpPost("adduseraddress")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> AddUserAddress(AddressDto address, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _userServices.AddUserAddress(UserDetail.Id, address);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }
        [HttpPut("updateuseraddress")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> UpdateUserAddress(int id, AddressDto address, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _userServices.UpdateUserAddress(id, UserDetail.Id, address);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }

        [HttpDelete("deleteaddress")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> DeleteAddress(int id, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _userServices.DeleteAddress(id, UserDetail.Id);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }

        [HttpGet("getuseraddress")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetUserAddress(int id, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _userServices.GetUserAddress(UserDetail.Id);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }

        [HttpGet("gettotalnumberofserviceseekers")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetTotalNumberOfServiceSeekers(int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _userServices.GetTotalNumberOfServiceSeekers();
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }

        [HttpGet("gettotalnumberofserviceproviders")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetTotalNumberOfServiceProviders(int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _userServices.GetTotalNumberOfServiceProviders();
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }
        [HttpGet]
        [Route("GetUsersByRole/{roleId}")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetAllUser(int roleId, int domainId)
        {
            return await _userServices.GetAllUser(roleId, domainId);
        }
        [HttpGet]
        [Route("GetUserByUserId/{userId}")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetUser(int userId, int domainId)
        {
            return await _userServices.GetUser(userId);
        }

        [HttpPost]
        [Route("UpdateUserDetails")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> UpdateUserDetails(UpdateUserRequestDto requestDto, int domainId)
        {
            return await _userServices.UpdateUserDetails(requestDto);
        }

        [HttpPost]
        [Route("UpdateWorkerStatus")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> UpdateWorkerStatus(int workerId, bool isOnline, int domainId)
        {
            return await _userServices.UpdateWorkerStatus(workerId, isOnline);
        }

        [HttpPost]
        [Route("UpdateUserProfile")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> UpdateUserProfile(ProfileDto profile, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _userServices.UpdateUserProfile(profile, UserDetail);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
       
        }
        [HttpPost]
        [Route("updateuserprofilepic")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> UpdateUserProfilePic(string profilePicKey, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _userServices.UpdateUserProfilePic(profilePicKey, UserDetail);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }

        }
        
        [HttpDelete]
        [Route("DeleteUser/{userId}")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> DeleteUser(int userId, int domainId)
        {
            return await _userServices.DeleteUser(userId);
        }

        [HttpPost]
        [Route("AddReview")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> AddReview(UserReviewRequestDto userReview, int domainId)
        {
            int addedBy = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type.Equals("id", StringComparison.InvariantCultureIgnoreCase))?.Value);

            return await _userServices.AddReview(userReview, addedBy);
        }

        [HttpGet]
        [Route("GetUserReviewsByUserId/{userId}")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetUserReviews(int userId, int domainId)
        {
            return await _userServices.GetUserReviews(userId);
        }
        [HttpGet]
        [Route("getserviceproviderdashboard")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetServiceProviderDashboard(int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _userServices.GetServiceProviderDashboard(UserDetail);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }

        }
        [HttpGet]
        [Route("getuserdashboard")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetUserDashboard(int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _userServices.GetUserDashboard(UserDetail.Id, domainId);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }

        }

        [HttpGet]
        [Route("getallnotifications")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetAllNotifications(int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _userServices.GetAllNotifications(UserDetail.Id);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }

        }
        [HttpPut("{id}")]
        [ValidateDomainIdFilter]
        public async Task<IActionResult> MarkAsRead(int id, int domainId)
        {
            await _notificationService.MarkAsReadAsync(id);
            return Ok();
        }

        [HttpPost]
        [Route("RegisterDevice")]
        [ValidateDomainIdFilter]
        [Authorize]
        public async Task<ResponseDto> RegisterDevice(DeviceRegistrationRequest request, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _userServices.RegisterDevice(request, UserDetail, domainId);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }

    }
}
