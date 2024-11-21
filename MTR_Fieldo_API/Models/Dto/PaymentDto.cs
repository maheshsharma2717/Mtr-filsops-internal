using Application.Common;

namespace MTR_Fieldo_API.Models.Dto
{
    public class PaymentDto
    {
        public long Amount { get; set; }
        public string currency { get; set; }
    }
    public class PaymentstatusDto
    {
        public string Intent { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }

}
