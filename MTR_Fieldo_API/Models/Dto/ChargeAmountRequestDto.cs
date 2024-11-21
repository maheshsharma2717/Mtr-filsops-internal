namespace MTR_Fieldo_API.Models.Dto
{
    public class ChargeAmountRequestDto
    {
        public int? TaskId { get; set; }
        public long Amount { get; set; }
        public string Currency { get; set; }
    }
}
