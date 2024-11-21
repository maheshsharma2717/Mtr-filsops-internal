namespace MTR_Fieldo_API.ViewModels
{
    public class UserReviewViewModel
    {
        public int UserId { get; set; }
        public string? ReviewerName { get; set; }
        public string? ReviewerPicture { get; set; }
        public string Review { get; set; }
        public int Rating { get; set; }
    }

    public class UserReviewViewModel2
    {
        public string Review { get; set; }
        public string AddedBy { get; set; }
        public int Rating { get; set; }
        public string? ReviewerPicture { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
