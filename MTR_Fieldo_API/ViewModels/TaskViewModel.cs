using Application.Common;
using Application.Models;

namespace MTR_Fieldo_API.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string? ViewStatus { get; set; }
        public double Price { get; set; }
        public string? Documents { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool? IsViewed { get; set; }
        public string Address { get; set; }
        public DateTime? ViewedTime { get; set; }
        public DateTime? AssignedTime { get; set; }
        public DateTime? WorkStartTime { get; set; }
        public DateTime? WorkCompleteTime { get; set; }
        public UserDetailsViewModel? CreatedBy { get; set; }
        public UserDetailsViewModel? ViewedBy { get; set; }
        public UserDetailsViewModel? UpdatedBy { get; set; }
        public UserDetailsViewModel? AssignedTo { get; set; }
        public UserDetailsViewModel? AssignedBy { get; internal set; }
        public List<string> AttachmentUrls { get; set; }
        public string CategoryName { get; set; }
        public string? Currency { get; set; }
        public string? PaymentStatus { get; set; }

    }
}
