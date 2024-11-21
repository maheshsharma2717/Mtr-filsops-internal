namespace MTR_Fieldo_API.ViewModels
{
    public class PaymentIntentsResponse
    {
        public string IntentId { get; set; }
        public string paymentIntent { get; set; }
        public string ephemeralKey { get; set; }
        public string customer { get; set; }
        public string publishableKey { get; set; }
    }
}
