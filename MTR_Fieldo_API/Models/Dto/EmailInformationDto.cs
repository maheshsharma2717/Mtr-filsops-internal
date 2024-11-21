namespace MTR_Fieldo_API.Models.Dto
{
    public class EmailInformationDto
    {
        public string MacAddress { get; set; }
        public string IpAddress { get; set; }
        public string UserName { get; set; }
        public string MaximumWithdrawalLimit { get; set; }
        public string MinimumWithdrawalLimit { get; set; }
        public string OrderId { get; set; }
        public string Price { get; set; }
        public string Reason { get; set; }
        public string Browser { set; get; }
        public bool IsActive { get; set; }
        public string EmailAddress { get; set; }
        public string Location { get; set; }
        public string ResetPassLink { get; set; }
        public string ActivationLink { get; set; }
        public string Telegram { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Medium { get; set; }
        public string Reddit { get; set; }
        public string Youtube { get; set; }
        public string Slack { get; set; }
        public string Message { get; set; }
        public string FromEmail { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal DebitAmount { get; set; }
        public string RequestNumber { get; set; }
        public string Amount { get; set; }
        public string UserId { get; set; }
        public string TransactionId { get; set; }
        public string UnsubscribeLink { get; set; }
        public string NewsLetterTitle { get; set; }
        public string NewsLetterSummery { get; set; }
        public string NewsLetterLink { get; set; }
        public string Password { get; set; }
    }
}
