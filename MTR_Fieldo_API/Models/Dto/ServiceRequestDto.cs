namespace MTR_Fieldo_API.Models.Dto
{
    public class ServiceRequestDto
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string>? Documents { get; set; }
        public Decimal? OfferPrice { get; set; }
    }
}
