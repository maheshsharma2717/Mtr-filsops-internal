using Application.Common;
using Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTR_Fieldo_API.Models;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service.IService;
using MTR_Fieldo_API.ViewModels;
using Polly;
using Square.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MTR_Fieldo_API.Service
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly ICommonService _commonService;
        private readonly IConfiguration _configuration;
        private readonly MtrContext _db;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService;
        private readonly PasswordHasher<Fieldo_UserDetails> _passwordHasher;
        private static string bucketName;

        private readonly ResponseDto _response;

        public AuthenticateService(MtrContext db, IJwtTokenGenerator jwtTokenGenerator, IPaymentService paymentService, ICommonService commonService, IConfiguration configuration, IUserService userService)
        {
            _db = db;
            _jwtTokenGenerator = jwtTokenGenerator;
            _response = new ResponseDto();
            _paymentService = paymentService;
            _commonService = commonService;
            _configuration = configuration;
            _userService = userService;
            _passwordHasher = new PasswordHasher<Fieldo_UserDetails>();
            bucketName = (Convert.ToBoolean(_configuration.GetSection("Environment:Staging").Value) ? _configuration.GetSection("AWS:DEVBucketName").Value.ToString() : _configuration.GetSection("AWS:BucketName").Value.ToString());

        }

        public async Task<ResponseDto> Register(UserRequestDto user)
        {

            try
            {

                if (user.Password.ToLower() != user.ConfirmPassword.ToLower())
                {
                    throw new Exception("Password and confirm password does not match");
                }

                var isUserExist = await _db.Fieldo_UserDetails.AnyAsync(x => x.Email.ToLower() == user.Email.ToLower() && x.IsActive && x.RoleId==user.RoleId);

                if (isUserExist)
                {
                    throw new Exception("Email already used with another account");
                }
                //UploadClientFileDto fileDto = new UploadClientFileDto
                //{
                //    FileName = "ProfilePic",
                //    File = user.ProfilePic
                //};
                // var uploadResult=    await _commonService.UploadFile(fileDto);
                Fieldo_UserDetails newUser = new()
                {
                    Email = user.Email,
                    FirstName = user.FirstName.Trim(),
                    LastName = user.LastName.Trim(),
                    MiddleName = user.MiddleName?.Trim(),
                    PhoneNumber = user.PhoneNumber,
                    ServiceCategoryId = user.ServiceCategoryId,
                    Password = user.Password,
                    RoleId = user.RoleId,
                    CreatedAt = DateTime.Now,
                    LastUpdatedAt = DateTime.Now,
                    IsActive = true,
                    DomainId = user.DomainId,
                    IsOnline = true,
                    ProfileUrl = user.ProfileUrl,
                    YearOfExperience = user.YearOfExperience,
                    CountryCode= user.PhoneCountryCode
                    //  ProfileUrl = uploadResult.key
                };

                if (user.RoleId == 3)
                {
                    newUser.IsOnline = false;
                }

                bool hasNullOrEmptyField = string.IsNullOrEmpty(user.StreetAddress) ||
                          string.IsNullOrEmpty(user.City) ||
                          string.IsNullOrEmpty(user.State) ||
                          string.IsNullOrEmpty(user.Country) ||
                          string.IsNullOrEmpty(user.PostalCode);

              

                UserDetailsViewModel userDetailsView = new()
                {
                    Email = user.Email,
                    Name = $"{user.FirstName} {(string.IsNullOrEmpty(user.MiddleName) ? "" : user.MiddleName + " ")}{user.LastName}".Trim(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsOnline = newUser.IsOnline,
                    MiddleName = user.MiddleName,
                    CreatedAt = DateTime.Now,
                    Id = newUser.Id,
                    LastUpdatedAt = DateTime.Now,
                    Rating = 0,
                    IsActive= newUser.IsActive,
                    YearOfExperience = user.YearOfExperience
                };

                var stripeCustomer = await _paymentService.CreateStripeCustomer(user.FirstName + " " + user.LastName, user.Email);
                newUser.StripeCustomerId = stripeCustomer.Result.ToString();
                SquareCustomerDto createSquareCustomer = new SquareCustomerDto();
                createSquareCustomer.Name = user.FirstName + " " + user.LastName;
                createSquareCustomer.EmailAddress = user.Email;
                var squareCustomer = await _paymentService.CreateSquareCustomer(createSquareCustomer);
                newUser.SquareCustomerId = stripeCustomer.Result.ToString();
                await _db.Fieldo_UserDetails.AddAsync(newUser);
                await _db.SaveChangesAsync();
                if (!hasNullOrEmptyField)
                {
                    AddressDto address = new()
                    {
                        StreetAddress = user.StreetAddress,
                        City = user.City,
                        State = user.State,
                        Country = user.Country,
                        PostalCode = user.PostalCode
                    };
                    await _userService.AddUserAddress(newUser.Id, address);
                }

                _response.Result = userDetailsView;
                _response.Message = "User created successfully";
                EmailInformationDto emailInformation = new EmailInformationDto
                {
                    EmailAddress = userDetailsView.Email
                };

                var (succeded, err) = _commonService.SendEmail("REG", emailInformation);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message}\n {ex.InnerException?.Message}";
            }
            return _response;
        }
        public async Task<ResponseDto> Login(LoginRequestDto userCred)
        {
            try
            {
                var u = await _db.Fieldo_UserDetails
                    .Include(x => x.Role).Include(x => x.Category)
                    .FirstOrDefaultAsync(x =>
                    (x.Email.ToLower() == userCred.UserName.ToLower() ||
                     x.FirstName.ToLower() + " " + x.LastName.ToLower() == userCred.UserName.ToLower())
                    && x.Password == userCred.Password
                    && x.IsActive && x.RoleId== userCred.RoleId
                    // && x.DomainId == userCred.DomainId
                    );
                

                if (u != null)
                {
                    UserDetailsViewModel userDetails = new()
                    {
                        Email = u.Email,
                        Name = $"{u.FirstName} {(string.IsNullOrEmpty(u.MiddleName) ? "" : u.MiddleName + " ")}{u.LastName}".Trim(),

                        FirstName = u.FirstName,
                        MiddleName = u.MiddleName,
                        LastName = u.LastName,
                        CreatedAt = u.CreatedAt,
                        PhoneNumber = u.PhoneNumber,
                        Id = u.Id,
                        Role = (string)Enum.GetName(typeof(Application.Common.Role), u.RoleId),
                        YearOfExperience = u.YearOfExperience,
                        ProfileUrl = !string.IsNullOrEmpty(u.ProfileUrl) ? _commonService.GeneratePreSignedURL(bucketName, u.ProfileUrl, 7200) : "",
                        DomainId = u.DomainId,
                        ServiceCategoryId= (int)u.ServiceCategoryId,
                        ServiceCategory= u?.Category != null ? u.Category.Name : "",
                        TotalServiceDelivered = await _db.Fieldo_Task
    .CountAsync(task => task.Status != null && task.Status == "Completed" && task.AssignedTo == u.Id)
                    };

                    UserResponseDto userResponseDto = new()
                    {
                        User = userDetails,
                        Token = _jwtTokenGenerator.GenerateToken(u)
                    };


                    _response.Result = userResponseDto;
                    _response.Message = "Login success!";
                }
                else
                {
                    throw new Exception("User email or password is incorrect");
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }


        public async Task<ResponseDto> AdminLogin(LoginRequestDto userCred)
        {
            try
            {
                string hashedPassword = HashPassword1(userCred.Password);
                var u = await _db.Taxi_Employees.Where(x =>
                    x.LoginId.ToLower() == userCred.UserName.ToLower()
                    && x.Password == hashedPassword
                     && x.domainid == userCred.DomainId).FirstOrDefaultAsync();

                if (u != null)
                {
                    UserDetailsViewModel userDetails = new()
                    {
                        Email = u.Email,
                        Name = $"{u.FirstName + " "}{u.LastName}".Trim(),

                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Id = u.Id,
                        Role = Application.Common.Role.Admin.ToString(),
                        DomainId = u.domainid,

                    };

                    UserResponseDto userResponseDto = new()
                    {
                        User = userDetails,
                        Token = _jwtTokenGenerator.GenerateAdminToken(u)
                    };

                    _response.IsSuccess = true;
                    _response.Result = userResponseDto;
                    _response.Message = "Login success!";
                }
                else
                {


                    var u2 = await _db.Taxi_User

                   .FirstOrDefaultAsync(x =>
                   (x.usr_email.ToLower() == userCred.UserName.ToLower() ||
                    x.usr_name.ToLower() == userCred.UserName.ToLower())
                   && x.usr_password == hashedPassword
                   // && x.DomainId == userCred.DomainId
                   );

                    if (u2 != null)
                    {
                        var domains = await _db.Taxi_Domains.ToListAsync();
                        if (!await _db.Taxi_Domains.Where(x => x.id == userCred.DomainId).AnyAsync())
                        {
                            _response.IsSuccess = false;
                            _response.Message = "Invalid Domain Id!";
                            return _response;
                        }
                        UserDetailsViewModel userDetails = new()
                        {
                            Email = u2.usr_email,
                            Name = u2.usr_name,

                            Id = u2.usr_id,
                            Role = Application.Common.Role.SuperAdmin.ToString(),
                            DomainId = userCred.DomainId,

                        };

                        UserResponseDto userResponseDto = new()
                        {
                            User = userDetails,
                            Token = _jwtTokenGenerator.GenerateSuperAdminToken(u2)
                        };

                        _response.IsSuccess = true;
                        _response.Result = userResponseDto;
                        _response.Message = "Login success!";
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Please check credentials!";
                    }
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
        public async Task<ResponseDto> ResetPassword(ResetPasswordDto resetPassword)
        {
            try
            {
                if (resetPassword.Password != resetPassword.ConfirmPassword)
                {
                    throw new Exception("Password and confirm password does not match");
                }

                var user = await _db.Fieldo_UserDetails.FirstOrDefaultAsync(u =>
                    u.Email.ToLower() == resetPassword.Email.ToLower()
                    && u.IsActive);

                if (user != null)
                {
                    user.Password = resetPassword.Password;
                    user.LastUpdatedAt = DateTime.Now;

                    _response.Message = "Password reset successfully";
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        public async Task<ResponseDto> GetAllUser()
        {
            try
            {
                var data = await _db.Fieldo_UserDetails.Where(x => x.IsActive).ToListAsync();

                List<UserDetailsViewModel> users = [];

                data.ForEach(u =>
                {
                    UserDetailsViewModel user = new UserDetailsViewModel()
                    {
                        Email = u.Email,
                        Name = u.FirstName + " " + u.LastName,
                    };
                    users.Add(user);
                });
                _response.Result = users;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException.Message}";
            }

            return _response;
        }

        public async Task<Fieldo_UserDetails> GetUserDetailsAsync(string claimValue)
        {
            return await _db.Fieldo_UserDetails.Where(x => x.Email == claimValue && x.IsActive).FirstOrDefaultAsync();

        }
        public async Task<taxi_employee?> GetAdminUserDetailsAsync(string claimValue)
        {
            return await _db.Taxi_Employees.Where(x => x.Id.ToString() == claimValue).FirstOrDefaultAsync();

        }
        public async Task<taxi_user?> GetSuperAdminUserDetailsAsync(string claimValue)
        {
            return await _db.Taxi_User.Where(x => x.usr_id.ToString() == claimValue).FirstOrDefaultAsync();

        }
        public async Task<object?> GetAdminSuperAdminUserDetailsAsync(ClaimsPrincipal user)
        {
            // Retrieve the Id and Role claims
            string? userId = user.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            string? userRole = user.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            if (userId == null || userRole == null)
            {
                // Handle cases where claims are missing
                return null;
            }

            if (userRole == Application.Common.Role.SuperAdmin.ToString())
            {
                // Fetch SuperAdmin user details
                return await GetSuperAdminUserDetailsAsync(userId);
            }
            else if (userRole == Application.Common.Role.Admin.ToString())
            {
                // Fetch Admin user details
                return await GetAdminUserDetailsAsync(userId);
            }

            // Return null if the role is neither SuperAdmin nor Admin
            return null;
        }
        public AdminUserType? GetAdminUserTypeFromClaims(string? roleClaim)
        {

            if (!string.IsNullOrEmpty(roleClaim))
            {
                // Convert the string role claim to AdminUserType enum
                if (Enum.TryParse(roleClaim, out AdminUserType userType))
                {
                    // Successfully converted to AdminUserType enum
                    return userType;
                }
            }

            // Return null if the claim is not found or conversion fails
            return null;
        }

        //Added for testing only
        public async Task<Fieldo_UserDetails> GetUserDetailssAsync(string claimValue)
        {
            Fieldo_UserDetails user = new Fieldo_UserDetails();
            //return await _db.Fieldo_UserDetails.Where(x => x.Email == claimValue && x.IsActive).FirstOrDefaultAsync();
            var res = await _db.Taxi_Employees.Where(x => x.Email == claimValue).FirstOrDefaultAsync();
            if (res == null) { }
            else
            {
                user.Id = res.Id;
                user.Email = res.Email;
                user.FirstName = res.FirstName;
                user.LastName = res.LastName;
                user.PhoneNumber = res.CellPhone;
            }
            return user;

        }

        public async Task<ResponseDto> ForgetPassLink(string email)
        {
            var user = _db.Fieldo_UserDetails.FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                _response.Message = "Email Id not Registered, Kindly check your entry.";
                return _response;
            }

            // Generate a secure token
            var token = GenerateSecureToken();
            user.ResetToken = token;
            user.ResetTokenExpiry = DateTime.UtcNow.AddDays(1); // Token valid for 1 day
            _db.SaveChanges();

            string forgetpasswordlink = (Convert.ToBoolean(_configuration.GetSection("Environment:Staging").Value)
                ? _configuration.GetSection("WebUrl:DevResetPassLink").Value
                : _configuration.GetSection("WebUrl:ResetPassLink").Value);
            forgetpasswordlink += $"{user.Email.Replace("@", ":")}&token={token}";

            EmailInformationDto emailInformation = new EmailInformationDto
            {
                EmailAddress = email,
                ResetPassLink = forgetpasswordlink
            };

            var (Succeeded, Error) = _commonService.SendEmail("RESPAS", emailInformation);

            if (Succeeded)
            {
                //_response.Result = forgetpasswordlink;
                _response.Message = "Password reset link has been sent to your registered email id.";
                _response.IsSuccess = true;
            }
            else
            {
                _response.Message = "Some server error has occurred. Please try again!";
                _response.IsSuccess = false;
            }

            return _response;
        }

        public async Task<ResponseDto> ResetForgetPassword(ForgetPasswordDto forgetPassword)
        {
            var user = _db.Fieldo_UserDetails.FirstOrDefault(x => x.Email == forgetPassword.Email);

            if (user == null || user.ResetToken != forgetPassword.Token || user.ResetTokenExpiry < DateTime.UtcNow)
            {
                _response.Message = "Invalid or expired token.";
                _response.IsSuccess = false;
                return _response;
            }

            // Hash the new password
            //user.Password = _passwordHasher.HashPassword(user, newPassword); for future use
            user.Password = forgetPassword.NewPassword;
            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            _db.SaveChanges();

            _response.Message = "Password has been reset successfully.";
            _response.IsSuccess = true;

            return _response;
        }

        private string GenerateSecureToken()
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData);
            }
        }
        public static string HashPassword1(string password)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = md5.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }

        }
    }
}

