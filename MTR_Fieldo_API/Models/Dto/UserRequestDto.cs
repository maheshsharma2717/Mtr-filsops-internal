namespace MTR_Fieldo_API.Models.Dto
{
    public class UserRequestDto
    {
        //  public int Id { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleId { get; set; }
        public int DomainId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int? ServiceCategoryId { get; set; }
        public float YearOfExperience { get; set; }
        public string? ProfileUrl { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? PhoneCountryCode { get; set; }
        //public IFormFile? ProfilePic { get; set; }
    }

    public class UpdateUserRequestDto
    {
        public int DomainId { get; set; }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int ServiceCategoryId { get; set; }
        public float YearOfExperience { get; set; }
        //public string Address { get; set; }
        public IFormFile? ProfilePic { get; set; }
        public string? ProfileUrl { get; set; }
        public bool IsActive { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }
}
