using Application.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MTR_Fieldo_API.Models;
using MTR_Fieldo_API.Service.IService;
using MTR_Fieldo_API.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MTR_Fieldo_API.Service
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;
        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateToken(Fieldo_UserDetails user)
        {
            string token = string.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            var claim = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,user.Email ),
                new ("Name", user.FirstName + " "+ user.LastName),
                new ("Email", user.Email),
                new ("Id", user.Id.ToString()),
                new ("Role", (string)Enum.GetName(typeof(Application.Common.Role), user.RoleId))
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            token = tokenHandler.WriteToken(securityToken);
            return token;
        }

        public string GenerateAdminToken(taxi_employee user)
        {
            string token = string.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            var claim = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,user.Email ),
                new ("Name", user.FirstName + " "+ user.LastName),
                new ("Email", user.Email),
                new ("Id", user.Id.ToString()),
                new ("Role", Application.Common.Role.Admin.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            token = tokenHandler.WriteToken(securityToken);
            return token;
        }

        public string GenerateSuperAdminToken(taxi_user user)
        {
            string token = string.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            var claim = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,user.usr_email),
                new ("Name",user.usr_name),
                new ("Email", user.usr_email),
                new ("Id", user.usr_id.ToString()),
                new ("Role", Application.Common.Role.SuperAdmin.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            token = tokenHandler.WriteToken(securityToken);
            return token;
        }
    }
}
