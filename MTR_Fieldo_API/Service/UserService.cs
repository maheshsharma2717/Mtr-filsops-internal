using Application.Common;
using Application.Models;
using AutoMapper;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service.IService;
using MTR_Fieldo_API.ViewModels;
using Polly;
using Stripe;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MTR_Fieldo_API.Service
{
    public class UserService : IUserService
    {
        private readonly MtrContext _context;
        private readonly ResponseDto _response;
        private readonly ICommonService _commonService;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _configuration;

        private static string bucketName;

        public UserService(MtrContext context, IMapper mapper, ICommonService commonService, IPaymentService paymentService, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _commonService = commonService;
            _response = new ResponseDto();
            _paymentService = paymentService;
            _configuration = configuration;
            bucketName = (Convert.ToBoolean(_configuration.GetSection("Environment:Staging").Value) ? _configuration.GetSection("AWS:DEVBucketName").Value.ToString() : _configuration.GetSection("AWS:BucketName").Value.ToString());
        }
        public async Task<ResponseDto> GetAllUser(int roleId, int domainId)
        {
            try
            {
                var data = await _context.Fieldo_UserDetails
                                .Include(u => u.Role)
                                .Where(u => u.RoleId == roleId && domainId == domainId).OrderByDescending(x => x.CreatedAt)
                                .ToListAsync();
                var userReviews = await _context.Fieldo_UserReview
                              .ToListAsync();
                List<UserDetailsViewModel> users = [];

                foreach (var u in data)
                {
                    UserDetailsViewModel user = new()
                    {
                        Id = u.Id,
                        Email = u.Email,
                        Name = u.FirstName.Trim() + " " + u.LastName.Trim(),
                        Role = u.Role.Name,
                        PhoneNumber = u.PhoneNumber,
                        CreatedAt = u.CreatedAt,
                        LastUpdatedAt = u.LastUpdatedAt,
                        YearOfExperience = u.YearOfExperience,
                        Rating = await GetUserRatingAsync(u.Id, userReviews),
                        ProfileUrl = u.ProfileUrl,
                        IsOnline = u.IsOnline,
                        IsActive = u.IsActive,
                        //Address = u.Address
                    };
                    users.Add(user);
                };
                _response.Result = users;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }

            return _response;
        }
        public async Task<ResponseDto> GetUser(int userId)
        {
            try
            {
                var u = await _context.Fieldo_UserDetails
                    .Include(u => u.Role)
                    .Include(c => c.Category)
                    .FirstAsync(u => u.Id == userId);

                var totalTasksAssigned = _context.Fieldo_Task.AsQueryable()
                                            .Where(task => task.AssignedTo == userId);
                var completedTask = totalTasksAssigned.AsEnumerable()
                                    .Where(task => task.Status != null && task.Status == TasksStatus.Completed.ToString());

                //Get total bank accounts count
                int totalBankAccounts = await _context.Fieldo_WorkerBankDetails.AsQueryable()
                                                .CountAsync(b => b.UserId == userId);
                var userReviews = _context.Fieldo_UserReview.AsQueryable().Where(x => x.UserId == userId).ToList();

                var userAddress = await _context.Fieldo_Address.AsQueryable()
                                            .Where(ua => ua.UserId == userId && ua.IsActive && !ua.IsDeleted).FirstOrDefaultAsync();
                if (userAddress == null)
                {
                    userAddress = new();
                }


                UserDetailsViewModel user = new()
                {
                    Id = userId,
                    Email = u.Email,
                    Name = $"{u.FirstName.Trim()} {(string.IsNullOrEmpty(u.MiddleName) ? "" : u.MiddleName.Trim() + " ")}{u.LastName.Trim()}".Trim(),
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    LastName = u.LastName,
                    PhoneNumber = u.PhoneNumber,
                    Role = u.Role.Name,
                    YearOfExperience = u.YearOfExperience,
                    CreatedAt = u.CreatedAt,
                    LastUpdatedAt = u.LastUpdatedAt,
                    ProfileUrl = !string.IsNullOrEmpty(u.ProfileUrl) ? _commonService.GeneratePreSignedURL(bucketName, u.ProfileUrl, 7200) : "",
                    ProfileKey = u.ProfileUrl,
                    Rating = await GetUserRatingAsync(u.Id, userReviews),
                    PaymentMethod = "",
                    Password = u.Password,
                    ServiceCategoryId = u.ServiceCategoryId != null ? u.ServiceCategoryId.Value : 0,
                    ServiceCategory = u.Category != null ? u.Category.Name : "",

                    TotalServiceDelivered = completedTask.Count(),
                    TotalMoneySpent = completedTask.Sum(x => x.Price),
                    OngoingTasks = totalTasksAssigned.Count(task => task.Status == TasksStatus.Inprogress.ToString()),
                    PendingTasks = totalTasksAssigned.Count(task => task.Status == TasksStatus.Pending.ToString() || task.Status == TasksStatus.Requested.ToString() || task.Status == "Created"),
                    CanceledTasks = totalTasksAssigned.Count(task => task.Status == TasksStatus.Cancelled.ToString()),
                    TotalBankAccounts = totalBankAccounts,
                    IsOnline = u.IsOnline,
                    //Address = u.Address,
                    IsActive = u.IsActive,

                    StreetAddress = userAddress != null ? userAddress.StreetAddress : "",
                    City = userAddress != null ? userAddress.City : "",
                    State = userAddress != null ? userAddress.State : "",
                    PostalCode = userAddress != null ? userAddress.PostalCode : "",
                    Country = userAddress != null ? userAddress.Country : "",
                    Reviews = [.. userReviews]
                };
                _response.Result = user;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }
        public async Task<ResponseDto> UpdateUserDetails(UpdateUserRequestDto requestDto)
        {
            try
            {
                var user = await _context.Fieldo_UserDetails.FirstOrDefaultAsync(u => u.Id == requestDto.Id);
                string? profileUrl = user.ProfileUrl;

                if (requestDto.ProfilePic != null)
                {
                    profileUrl = await UploadFile(requestDto);
                }
                else
                {
                    profileUrl = requestDto.ProfileUrl;
                }


                if (user != null)
                {
                    user.Email = requestDto.Email;
                    user.FirstName = requestDto.FirstName;
                    user.LastName = requestDto.LastName;
                    user.MiddleName = requestDto.MiddleName;
                    user.Password = requestDto.Password;
                    user.LastUpdatedAt = DateTime.Now;
                    user.YearOfExperience = requestDto.YearOfExperience;
                    user.ProfileUrl = profileUrl;
                    user.ServiceCategoryId = requestDto.ServiceCategoryId;
                    user.RoleId = requestDto.RoleId;
                    user.PhoneNumber = requestDto.PhoneNumber;
                    //user.Address = requestDto.Address;
                    user.IsActive = requestDto.IsActive;
                    _context.Fieldo_UserDetails.Update(user);
                    await _context.SaveChangesAsync();
                    _response.Message = "Details updated successfully";

                }
                else
                {
                    throw new Exception("User not found");
                }
                var userAddress = await _context.Fieldo_Address.AsQueryable()
                                          .Where(ua => ua.UserId == requestDto.Id && ua.IsActive && !ua.IsDeleted).FirstOrDefaultAsync();
                AddressDto address = new()
                {
                    StreetAddress = requestDto.StreetAddress,
                    City = requestDto.City,
                    State = requestDto.State,
                    Country = requestDto.Country,
                    PostalCode = requestDto.PostalCode
                };

                if (userAddress == null)
                {
                    await AddUserAddress(requestDto.Id, address);
                }
                else
                {
                    await UpdateUserAddress(userAddress.Id, requestDto.Id, address);
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }
        public async Task<ResponseDto> UpdateWorkerStatus(int workerId, bool isOnline)
        {
            try
            {
                Fieldo_UserDetails worker = await _context.Fieldo_UserDetails
                                            .FirstOrDefaultAsync(u => u.Id == workerId);
                if (worker != null)
                {
                    //worker.Status = status.ToString();
                    worker.IsOnline = isOnline;
                    worker.LastUpdatedAt = DateTime.Now;
                    _context.Fieldo_UserDetails.Update(worker);
                    await _context.SaveChangesAsync();
                    _response.Message = "Your status has been changed";
                }
                else
                {
                    throw new Exception("Worker not found");
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }
        public async Task<ResponseDto> AddUserAddress(int userId, AddressDto address)
        {
            try
            {
                var data = await _context.Fieldo_Address.Where(x => x.UserId == userId && x.IsActive == true && x.IsDeleted == false).FirstOrDefaultAsync();
                if (data != null)
                {
                    _response.Message = "Address Already Added";
                    _response.IsSuccess = false;
                }
                else
                {
                    Fieldo_Address Fieldo_Address = new Fieldo_Address()
                    {
                        Latitude = address.Latitude,
                        Longitude = address.Longitude,
                        UserId = userId,
                        IsActive = true,
                        FormattedAddress = address.FormattedAddress,
                        PlaceName = address.PlaceName,
                        StreetAddress = address.StreetAddress,
                        City = address.City,
                        State = address.State,
                        PostalCode = address.PostalCode,
                        Country = address.Country,
                        AdditionalMetadata = address.AdditionalMetadata,
                        CreatedAt = DateTime.Now,
                        CreatedBy = userId,
                        IsDeleted = false

                    };
                    await _context.Fieldo_Address.AddAsync(Fieldo_Address);
                    await _context.SaveChangesAsync();
                    _response.Message = "Address Added successfully";
                    _response.IsSuccess = true;
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;

        }
        public async Task<ResponseDto> DeleteAddress(int id, int userId)
        {
            try
            {
                var data = await _context.Fieldo_Address.Where(x => x.UserId == userId && x.Id == id && x.IsActive == true && x.IsDeleted == false).FirstOrDefaultAsync();
                if (data != null)
                {
                    data.IsDeleted = true;
                    data.IsActive = false;
                    _response.Result = data;
                    _response.IsSuccess = false;
                    _context.Fieldo_Address.Update(data);
                    await _context.SaveChangesAsync();
                    _response.Message = "Address Delted Successfully";
                    _response.IsSuccess = true;
                }
                else
                {
                    _response.Message = "Address not found";
                    _response.IsSuccess = false;
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;

        }
        public async Task<ResponseDto> GetUserAddress(int userId)
        {
            try
            {
                var data = await _context.Fieldo_Address.Where(x => x.UserId == userId && x.IsActive == true && x.IsDeleted == false).FirstOrDefaultAsync();
                if (data != null)
                {
                    _response.Result = data;
                    _response.IsSuccess = false;
                }
                else
                {
                    _response.Message = "Address not found";
                    _response.IsSuccess = true;
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;

        }
        public async Task<ResponseDto> UpdateUserAddress(int id, int userId, AddressDto address)
        {
            try
            {
                var data = await _context.Fieldo_Address.Where(x => x.UserId == userId && x.Id == id && x.IsActive == true && x.IsDeleted == false).FirstOrDefaultAsync();
                if (data != null)
                {
                    data.Longitude = address.Longitude;
                    data.Latitude = address.Latitude;
                    data.FormattedAddress = address.FormattedAddress;
                    data.PlaceName = address.PlaceName;
                    data.StreetAddress = address.StreetAddress;
                    data.City = address.City;
                    data.State = address.State;
                    data.PostalCode = address.PostalCode;
                    data.Country = address.Country;
                    data.AdditionalMetadata = address.AdditionalMetadata;
                    data.UpdatedAt = DateTime.Now;

                    _context.Fieldo_Address.Update(data);
                    await _context.SaveChangesAsync();
                    _response.Message = "Address Updated successfully";
                    _response.IsSuccess = true;


                }
                else
                {
                    _response.Message = "Address not found";
                    _response.IsSuccess = true;
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }
        public async Task<ResponseDto> GetTotalNumberOfServiceSeekers()
        {
            try
            {
                var data = await _context.Fieldo_UserDetails.Where(x => x.RoleId == 2).CountAsync();
                _response.Result = data;
                _response.IsSuccess = true;
            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }
        public async Task<ResponseDto> GetTotalNumberOfServiceProviders()
        {
            try
            {
                var data = await _context.Fieldo_UserDetails.Where(x => x.RoleId == 3).CountAsync();
                _response.Result = data;
                _response.IsSuccess = true;
            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }
        public async Task<ResponseDto> DeleteUser(int userId)
        {
            try
            {
                var user = _context.Fieldo_UserDetails.FirstOrDefault(x => x.Id == userId && x.IsActive);

                if (user != null)
                {
                    user.IsActive = false;
                    user.LastUpdatedAt = DateTime.Now;

                    _context.Fieldo_UserDetails.Update(user);
                    await _context.SaveChangesAsync();
                    _response.Message = "User deleted successfully";
                }
                else
                {
                    throw new Exception("User not found!");
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }
        public async Task<ResponseDto> AddReview(UserReviewRequestDto userReview, int addedBy)
        {
            try
            {
                Fieldo_UserReview _userReview = new()
                {
                    Rating = userReview.Rating,
                    Review = userReview.Review,
                    UserId = userReview.UserId,
                    AddedBy = addedBy,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
                await _context.Fieldo_UserReview.AddAsync(_userReview);
                await _context.SaveChangesAsync();
                _response.Message = "Review added successfully";

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }
        public async Task<ResponseDto> GetUserReviews(int userId)
        {
            try
            {
                var reviews = await _context.Fieldo_UserReview.Where(u => u.Id == userId).Include(x => x.UserDetailsAddedBy).ToListAsync();
                List<UserReviewViewModel> userReviews = [];

                reviews.ForEach(review =>
                {
                    UserReviewViewModel userReview = new()
                    {
                        Rating = review.Rating,
                        Review = review.Review,
                        UserId = review.UserId,
                        ReviewerName = review.UserDetailsAddedBy.FirstName + " " + review.UserDetailsAddedBy.LastName,
                        ReviewerPicture = !string.IsNullOrEmpty(review.UserDetailsAddedBy.ProfileUrl) ? _commonService.GeneratePreSignedURL(bucketName, review.UserDetailsAddedBy.ProfileUrl, 7200) : "",
                    };

                    userReviews.Add(userReview);
                });

                _response.Result = userReviews;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }

        private async Task<double> GetUserRatingAsync(int userId, List<Fieldo_UserReview> userReviews)
        {
            double rating = 0;
            int count = 0;
            List<Fieldo_UserReview> reviews = userReviews.Where(u => u.UserId == userId).ToList();

            foreach (var item in reviews)
            {
                count++;
                rating += item.Rating;
            }
            if (count != 0)
                rating /= count;
            return rating;

            //.AverageAsync(x => x.Rating);

        }

        private async Task<string> UploadFile(UpdateUserRequestDto userRequestDto)
        {
            object url = "";
            Fieldo_UserDetails user = new()
            {
                Id = userRequestDto.Id,
            };

            UploadClientFileDto upload = new()
            {
                File = userRequestDto.ProfilePic,
                FileName = userRequestDto.ProfilePic.FileName,
            };
            ResponseDto response = await _commonService.UploadFile(upload, user);
            if (response.Result != null)
            {
                Dictionary<object, object> a = (Dictionary<object, object>)response.Result;

                a.TryGetValue("fileUrl", out url);

            }
            return url.ToString();
        }

        public async Task<ResponseDto> UpdateUserProfile(ProfileDto profile, Fieldo_UserDetails fieldo_User)
        {
            try
            {
                if (profile.FirstName != null && profile.FirstName != "")
                {
                    fieldo_User.FirstName = profile.FirstName;
                }
                if (profile.LastName != null && profile.LastName != "")
                {
                    fieldo_User.LastName = profile.LastName;
                }
                if (profile.MiddleName != null && profile.MiddleName != "")
                {
                    fieldo_User.MiddleName = profile.MiddleName;
                }
                if (profile.Password != null && profile.Password != "")
                {
                    fieldo_User.Password = profile.Password;
                }
                if (profile.Email != null && profile.Email != "")
                {
                    fieldo_User.Email = profile.Email;
                }
                if (profile.PhoneNumber != null && profile.PhoneNumber != "")
                {
                    fieldo_User.PhoneNumber = profile.PhoneNumber;
                }
                if (profile.PhoneNumberCountryCode != null && profile.PhoneNumberCountryCode != "")
                {
                    fieldo_User.CountryCode = profile.PhoneNumberCountryCode;
                }
                if (profile.ProfilePicKey != null && profile.ProfilePicKey != "")
                {
                    fieldo_User.ProfileUrl = profile.ProfilePicKey;
                }
                if (profile.ServiceCategoryId != null && profile.ServiceCategoryId != 0)
                {
                    fieldo_User.ServiceCategoryId = profile.ServiceCategoryId;
                }
                if (profile.YearOfExperience != null && profile.YearOfExperience != 0.0f)
                {
                    fieldo_User.YearOfExperience = (float)profile.YearOfExperience;
                }


                _response.Message = "Profile updated successfully";

                _context.Update(fieldo_User);
                await _context.SaveChangesAsync();
                _response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }

        public async Task<ResponseDto> UpdateUserProfilePic(string imageKey, Fieldo_UserDetails fieldo_User)
        {
            try
            {

                fieldo_User.ProfileUrl = imageKey;

                _response.Message = "Profile picture updated successfully";
                _context.Update(fieldo_User);
                await _context.SaveChangesAsync();
                _response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }

        public async Task<ResponseDto> GetServiceProviderDashboard(Fieldo_UserDetails user)
        {
            try
            {
                UserDashboardDataViewModel dashboardData = new()
                {
                    IsOnline = user.IsOnline,
                    PendingTaksCount = await _context.Fieldo_Task
                       .CountAsync(task => task.Status != null && task.Status == "Pending" && task.AssignedTo == user.Id),
                    CompletedTasksCount = await _context.Fieldo_Task
                       .CountAsync(task => task.Status != null && task.Status == "Completed" && task.AssignedTo == user.Id),
                    InprogressTaskCount = await _context.Fieldo_Task
                       .CountAsync(task => task.Status != null && task.Status == "Inprogress" && task.AssignedTo == user.Id),
                    TotalTasks = await _context.Fieldo_Task.Where(t => t.AssignedTo == user.Id).CountAsync(),
                    Reviews = [],
                    MyEarning = await _paymentService.GetMyEarningByUserId(user.Id)
                };

                dashboardData.Reviews = await _context.Fieldo_UserReview
                                    .Where(u => u.UserId == user.Id)
                                    .Select(u => new UserReviewViewModel2
                                    {
                                        CreatedAt = u.CreatedAt,
                                        Rating = u.Rating,
                                        UpdatedAt = u.UpdatedAt,
                                        Review = u.Review,
                                        ReviewerPicture = !string.IsNullOrEmpty(u.UserDetailsAddedBy.ProfileUrl) ? _commonService.GeneratePreSignedURL(bucketName, u.UserDetailsAddedBy.ProfileUrl, 7200) : "",
                                        AddedBy = u.UserDetailsAddedBy != null ? u.UserDetailsAddedBy.FirstName + " " + u.UserDetailsAddedBy.LastName : "Unknown"
                                    })
                                    .ToListAsync();
                dashboardData.AverageRating = 0;
                if (dashboardData.Reviews.Any())
                {
                    dashboardData.AverageRating = Math.Round(dashboardData.Reviews.Average(u => u.Rating), 1);
                }
                _response.Result = dashboardData;
                _response.Message = "Dashboard data retrieved successfully";

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _response;

        }

        public async Task<ResponseDto> GetUserDashboard(int userId, int domainId)
        {
            try
            {
                DashboardViewModel dashboardData = new()
                {
                    Categories = await _context.Fieldo_RequestCategory.Where(x => x.IsDeleted == false && domainId == domainId).ToListAsync(),
                    NumberOfServicesRequested = await _context.Fieldo_Task
                       .CountAsync(task => task.Status != null && task.CreatedBy == userId),
                    TotalMoneySpent = await _context.Fieldo_Payments
    .Where(x => x.UserId == userId && x.Status == "success")
    .SumAsync(x => x.Amount ?? 0),

                    Reviews = [],

                };
                if (dashboardData.Categories.Count() > 0)
                {
                    foreach (var item in dashboardData.Categories)
                    {
                        item.Icon = !string.IsNullOrEmpty(item.Icon) ? _commonService.GeneratePreSignedURL(bucketName, item.Icon, 7200) : "";
                    }
                }
                dashboardData.Reviews = await _context.Fieldo_UserReview
                                    .Where(u => u.UserId == userId)
                                    .Select(u => new UserReviewViewModel2
                                    {
                                        CreatedAt = u.CreatedAt,
                                        Rating = u.Rating,
                                        UpdatedAt = u.UpdatedAt,
                                        Review = u.Review,
                                        ReviewerPicture = !string.IsNullOrEmpty(u.UserDetailsAddedBy.ProfileUrl) ? _commonService.GeneratePreSignedURL(bucketName, u.UserDetailsAddedBy.ProfileUrl, 7200) : "",
                                        AddedBy = u.UserDetailsAddedBy != null ? u.UserDetailsAddedBy.FirstName + " " + u.UserDetailsAddedBy.LastName : "Unknown"

                                    })
                                    .ToListAsync();
                dashboardData.AverageRating = 0;
                if (dashboardData.Reviews.Any())
                {
                    dashboardData.AverageRating = Math.Round(dashboardData.Reviews.Average(u => u.Rating), 1);
                }
                _response.Result = dashboardData;
                _response.Message = "Dashboard data retrieved successfully";
                _response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _response;

        }

        public async Task<ResponseDto> GetAllNotifications(int userId)
        {
            try
            {


                var notifications = await _context.Fieldo_Notifications.Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAt)
                                    .ToListAsync();

                _response.Result = notifications;
                _response.Message = "notificatiion data retrieved successfully";
                _response.IsSuccess = false;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _response;

        }

        public Fieldo_UserDetails GetUserById(int id)
        {
            return _context.Fieldo_UserDetails.FirstOrDefault(u => u.Id == id);
        }

        public Fieldo_UserDetails GetUserByEmail(string email)
        {
            return _context.Fieldo_UserDetails.FirstOrDefault(u => u.Email == email);
        }

        public async Task<ResponseDto> RegisterDevice(DeviceRegistrationRequest request, Fieldo_UserDetails user, int domainId)
        {
            try
            {

                // Check if this token already exists for the user
                var existingToken = await _context.Fieldo_DeviceToken.FirstOrDefaultAsync(t => t.UserID == user.Id && t.DeviceToken == request.DeviceToken);

                if (existingToken != null)
                {
                    if (existingToken.DeviceToken == request.DeviceToken)
                    {
                        _response.Message = "device token is same";
                        _response.IsSuccess = true;
                    }
                    // Add a new token if it's not already in the database

                }
                else
                {
                    // Add a new token if it's not already in the database
                    _context.Fieldo_DeviceToken.Add(new Fieldo_DeviceToken
                    {
                        UserID = user.Id,
                        DeviceToken = request.DeviceToken,
                        DeviceType = request.DeviceType, // Optional: Store the device type (e.g., iOS, Android)
                        LastUpdated = DateTime.Now,
                        DomainId = domainId
                    });
                    _context.SaveChanges();

                    _response.Message = "device added successfully";
                    _response.IsSuccess = true;
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _response;
        }

    }

}