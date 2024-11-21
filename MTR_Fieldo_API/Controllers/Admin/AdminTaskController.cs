using Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service.IService;

namespace MTR_Fieldo_API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminTaskController : ControllerBase
    {


        private readonly IAdminTaskService _adminTaskService;
        private readonly IAuthenticateService _authenticateService;
        private readonly ResponseDto _responseDto;

        public AdminTaskController(IAuthenticateService authenticateService, IAdminTaskService  adminTaskService)
        {
            _adminTaskService = adminTaskService;
            _authenticateService = authenticateService;
            _responseDto = new();
        }
        [HttpPost]
        //[Authorize(Policy = "Administrator")]
        [Authorize]
        [Route("createtask")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> CreateTask(int TaskCreatedForUserId, TaskRequestDto taskRequest, int domainId)
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
                        return await _adminTaskService.CreateTask (Convert.ToInt32(userId) , TaskCreatedForUserId, taskRequest, domainId, adminUserType);

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

        [HttpGet]
        //[Authorize(Policy = "Administrator")]
        [Authorize]
        [Route("canceltask")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> CancelTask(int taskId, int domainId)
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
                        return await _adminTaskService.CancelTask(Convert.ToInt32(userId), taskId, domainId, adminUserType);

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
    }
}
