namespace MTR_Fieldo_API.Models.Dto
{
    public class UserReviewRequestDto
    {
        public int UserId { get; set; }
        public string Review { get; set; }
        public int Rating { get; set; }
    }
}
