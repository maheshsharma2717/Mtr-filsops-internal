namespace MTR_Fieldo_API.Models.Dto
{
    public class AddUpdateBankDto
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string BankName { get; set; }
        public string AccountNumber {  get; set; }
        public string RoutingNumber { get; set; }
        public string AccountType { get; set; }

    }
}
