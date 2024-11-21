using Application.Common;
using Application.Models;
using MTR_Fieldo_API.Models.Dto;
using System.Security.Claims;

namespace MTR_Fieldo_API.Service.IService
{
    public interface IAuthenticateService
    {
        Task<ResponseDto> Register(UserRequestDto user);
        Task<ResponseDto> Login(LoginRequestDto user);
        Task<ResponseDto> AdminLogin(LoginRequestDto userCred);
        Task<ResponseDto> ResetPassword(ResetPasswordDto resetPassword);
        Task<ResponseDto> ForgetPassLink(string email);
        Task<ResponseDto> ResetForgetPassword(ForgetPasswordDto forgetPassword);
        Task<ResponseDto> GetAllUser();
        Task<Fieldo_UserDetails> GetUserDetailsAsync(string claimValue);
        Task<Fieldo_UserDetails> GetUserDetailssAsync(string claimValue);
         Task<taxi_employee?> GetAdminUserDetailsAsync(string claimValue);
         Task<taxi_user?> GetSuperAdminUserDetailsAsync(string claimValue);
        Task<object?> GetAdminSuperAdminUserDetailsAsync(ClaimsPrincipal user);
        AdminUserType? GetAdminUserTypeFromClaims(string? roleClaim);

    }
}
