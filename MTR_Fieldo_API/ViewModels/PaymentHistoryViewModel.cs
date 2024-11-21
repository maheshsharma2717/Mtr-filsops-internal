namespace MTR_Fieldo_API.ViewModels
{
    public class PaymentHistoryViewModel
    {
        public int Id { get; set; }
        public string? ServiceType { get; set; }
        public string? ServiceProviderOrCategoryIcon { get; set; }
        public string? ServiceProvider { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? PaymentDateTime { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
    }
}
