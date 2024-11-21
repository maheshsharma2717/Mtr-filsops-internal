
using Application.Models;
using MTR_Fieldo_API.Models.Dto;

namespace MTR_Fieldo_API.ViewModels
{
    public class UserDetailsViewModel
    {
        public int Id { get; set; }

        private string name;
        public string Name
        {
            get => name ?? string.Empty;
            set => name = value;
        }
        private string firstName;
        public string FirstName
        {
            get => firstName ?? string.Empty;
            set => firstName = value;
        }
        private string? middleName;
        public string MiddleName
        {
            get => middleName ?? string.Empty;
            set => middleName = value;
        }

        private string lastName;
        public string LastName
        {
            get => lastName ?? string.Empty;
            set => lastName = value;
        }

        private string phoneNumber;
        public string PhoneNumber
        {
            get => phoneNumber ?? string.Empty;
            set => phoneNumber = value;
        }

        private string email;
        public string Email
        {
            get => email ?? string.Empty;
            set => email = value;
        }

        private string role;
        public string Role
        {
            get => role ?? string.Empty;
            set => role = value;
        }

        public DateTime CreatedAt { get; internal set; }
        public DateTime LastUpdatedAt { get; internal set; }
        public float YearOfExperience { get; internal set; }
        public double Rating { get; internal set; }

        private string profileUrl;
        public string ProfileKey { get; set; }
        public string ProfileUrl
        {
            get => profileUrl ?? string.Empty;
            set => profileUrl = value;
        }

        public double TotalMoneySpent { get; set; }
        public int OngoingTasks { get; set; }
        public int PendingTasks { get; set; }
        public int CanceledTasks { get; set; }
        public int TotalBankAccounts { get; set; }

        private int? totalServiceDelivered;
        public int? TotalServiceDelivered
        {
            get => totalServiceDelivered ?? 0; // assuming 0 as a default for nullable int
            set => totalServiceDelivered = value;
        }
        private int? domainId;
        public int? DomainId
        {
            get => domainId ?? 0; // assuming 0 as a default for nullable int
            set => domainId = value;
        }

        private string paymentMethod;
        public string PaymentMethod
        {
            get => paymentMethod ?? string.Empty;
            set => paymentMethod = value;
        }

        private bool? isOnline;
        public bool? IsOnline
        {
            get => isOnline ?? false; // assuming false as a default for nullable bool
            set => isOnline = value;
        }

        private List<Fieldo_UserReview> reviews;
        public List<Fieldo_UserReview> Reviews
        {
            get => reviews ?? new List<Fieldo_UserReview>();
            set => reviews = value;
        }
        public string Password { get; set; }
        public int? ServiceCategoryId { get; set; }
        public string ServiceCategory { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }

        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }

        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string Email { get; set; }
        //public string Role { get; set; }
        //public DateTime CreatedAt { get; internal set; }
        //public DateTime LastUpdatedAt { get; internal set; }
        //public float YearOfExperience { get; internal set; }
        //public double Rating { get; internal set; }
        //public string ProfileUrl { get; set; }
        //public double TotalMoneySpent { get; set; }
        //public int ServicesRequested { get; set; }
        //public int? TotalServiceDelivered { get; set; }
        //public string PaymentMethod { get; set; }
        //public bool? IsOnline { get; set; }
        //public List<Fieldo_UserReview> Reviews { get; set; }

    }
}
