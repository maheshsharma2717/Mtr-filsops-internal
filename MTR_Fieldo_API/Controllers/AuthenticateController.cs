using Microsoft.AspNetCore.Mvc;
using MTR_Fieldo_API.Models;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service;
using MTR_Fieldo_API.Service.IService;

namespace MTR_Fieldo_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;

        public AuthenticateController(IAuthenticateService authService)
        {
            _authenticateService = authService;
        }

        [HttpPost("register")]
        public async Task<ResponseDto> Register([FromForm] UserRequestDto user)
        {
            return await _authenticateService.Register(user);
        }

        [HttpPost("login")]
        public async Task<ResponseDto> Login(LoginRequestDto user)
        {
            return await _authenticateService.Login(user);
        }
        [HttpPost("adminlogin")]
        public async Task<ResponseDto> AdminLogin(LoginRequestDto user)
        {
            return await _authenticateService.AdminLogin(user);
        }

        [HttpPost("resetpassword")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> ResetPassword(ResetPasswordDto resetPassword, int domainId)
        {
            return await _authenticateService.ResetPassword(resetPassword);
        }

        [HttpPost("forgetpasslink")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> ForgetPassLink(string email, int domainId)
        {
            return await _authenticateService.ForgetPassLink(email);
        }

        [HttpPost("resetforgetpassword")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> ResetForgetPassword(ForgetPasswordDto forgetPasswordDto, int domainId)
        {
            return await _authenticateService.ResetForgetPassword(forgetPasswordDto);
        }


    }
}
