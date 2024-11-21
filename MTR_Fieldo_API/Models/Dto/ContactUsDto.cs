using System.ComponentModel.DataAnnotations;

namespace MTR_Fieldo_API.Models.Dto
{
    public class ContactUsDto
    {
        [Required]
        [EmailAddress]
        public string FromEmail { get; set; }
        [Required]
        [EmailAddress]
        public string ToEmail { get; set; }
        public string Message { get; set; }
        public string Name { get; set; }
        public string DepartmentName { get; set; }
    }
}
