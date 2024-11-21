using Application.Common;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service;
using MTR_Fieldo_API.Service.IService;
using System.Security.Claims;

//information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MTR_Fieldo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly ResponseDto _responseDto;
        private readonly IAdminService _adminService;
        public AdminController(IAuthenticateService authenticateService, IAdminService adminService)
        {
            _responseDto = new ResponseDto();
            _authenticateService = authenticateService;
            _adminService = adminService;
        }
        [HttpGet("getallservicecategories")]
        [ValidateDomainIdFilter]
        // [Authorize]
        public async Task<ResponseDto> GetAllServiceCategories(int domainId)
        {
            //Claim claim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
            //if (claim != null)
            //{
             //   Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _adminService.GetAllServiceCategories(domainId);
            //}
            //else
            //{
            //    _responseDto.IsSuccess = false;
            //    _responseDto.Message = "Claims not found";
            //    return _responseDto;
            //}

        }

        [HttpGet("getservicecategorybyid")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetServiceCategoryById(int id, int domainId)
        {
            Claim claim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _adminService.GetServiceCategoryById(id);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }

        [HttpPost("addservicecategory")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> AddServiceCategory(RequestCategoryDto Category, int domainId)
        {

            //Claim claim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
            //if (claim != null)
            //{
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync("naveenkr113@gmail.com");
                return await _adminService.AddServiceCategory(UserDetail, Category,domainId);
            //}
            //else
            //{
            //    _responseDto.IsSuccess = false;
            //    _responseDto.Message = "Claims not found";
            //    return _responseDto;
            //}
        }

        [HttpPut("updateservicecategoryName")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> UpdateServiceCategoryName(int id, string newCategoryName, string newDescription, int domainId)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                Fieldo_UserDetails userDetail = await _authenticateService.GetUserDetailsAsync(claim.Value);
                return await _adminService.UpdateServiceCategory(userDetail, id, newCategoryName, newDescription);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }

        [HttpPut("updateservicecategoryicon")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> UpdateServiceCategoryIcon(int id, string newCategoryIcon, int domainId)
        {
            Claim claim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
            if (claim != null)                                                                                                                   
            {                                                                    
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _adminService.UpdateServiceCategoryIcon(UserDetail, id, newCategoryIcon);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }

        [HttpDelete("deleteservicecategory")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> DeleteServiceCategory(int id, int domainId)
        {
            Claim claim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _adminService.DeleteServiceCategory(UserDetail, id);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }

        [HttpGet]
        [Route("GetDashboardDetails")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetDashboardDetals(int domainId)
        {
            return await _adminService.GetDashboardDetals( domainId);
        }

        [HttpPost]
        [Route("RegisterDevice")]
        [ValidateDomainIdFilter]
        [Authorize]
        public async Task<ResponseDto> RegisterDevice(DeviceRegistrationRequest request, int domainId)
        {
            // Retrieve the Id claim
            string userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            // Retrieve the Role claim
            string roleClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            AdminUserType? adminUserType= _authenticateService.GetAdminUserTypeFromClaims(roleClaim);

            if (userId!= null && roleClaim != null && adminUserType!=null)
            {
                var userDetails = await _authenticateService.GetAdminSuperAdminUserDetailsAsync(HttpContext.User);
                if (userDetails != null)
                {
                    if (adminUserType != null)
                    {
                        return _adminService.RegisterDevice(request, Convert.ToInt32(userId), adminUserType, domainId).GetAwaiter().GetResult();

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
