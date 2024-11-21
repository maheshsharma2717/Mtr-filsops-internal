using Square.Models;

namespace MTR_Fieldo_API.Models.Dto
{
    public class UpdatePaymentDto
    {
        public Money Money { get; set; }
        public Money Tip { get; set; }

        public Money AppFee { get; set; }

        public Money Approved { get; set; }

        public Money BuyerSupplied { get; set; }

        public Money ChangeBack { get; set; }
        public Square.Models.Payment Payment { get; set; }
    }
}
