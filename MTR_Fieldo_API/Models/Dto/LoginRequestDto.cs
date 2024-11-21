namespace MTR_Fieldo_API.Models.Dto
{
    public class LoginRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int DomainId { get; set; }
        public int RoleId { get; set; }

    }
}
