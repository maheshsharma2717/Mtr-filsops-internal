namespace MTR_Fieldo_API.ViewModels
{
    public class TaskStatusViewModel
    {
        public int TaskId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string>? Attachments { get; set; }
    }
}
