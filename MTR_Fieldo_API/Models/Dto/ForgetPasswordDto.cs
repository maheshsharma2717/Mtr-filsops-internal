using System.ComponentModel.DataAnnotations;

namespace MTR_Fieldo_API.Models.Dto
{
    public class ForgetPasswordDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
