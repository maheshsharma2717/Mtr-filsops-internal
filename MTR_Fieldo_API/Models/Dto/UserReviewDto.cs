namespace MTR_Fieldo_API.Models.Dto
{
    public class UserReviewDto
    {
        public string Review { get; set; }
        public string AddedBy { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
