using Application.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTR_Fieldo_API.ViewModels
{
    public class UserBankDetailsViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string LastName2 { get; set; }
        public string? BankName { get; set; }
        public string OtherBankName { get; set; }
        public string AccountNumber { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string OtherAccountType { get; set; }
    }
}
