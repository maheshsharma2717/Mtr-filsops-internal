using Application.Models;
using MTR_Fieldo_API.ViewModels;

namespace MTR_Fieldo_API.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Fieldo_UserDetails user);
        string GenerateAdminToken(taxi_employee user);
        string GenerateSuperAdminToken(taxi_user user);
    }
}
