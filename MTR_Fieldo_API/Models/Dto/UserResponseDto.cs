using MTR_Fieldo_API.ViewModels;

namespace MTR_Fieldo_API.Models.Dto
{
    public class UserResponseDto
    {
        public string Token { get; set; }
        public UserDetailsViewModel User { get; set; }
    }
}
