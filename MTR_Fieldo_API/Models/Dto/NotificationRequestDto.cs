using Application.Models;

namespace MTR_Fieldo_API.Models.Dto
{
    public class NotificationRequestDto
    {
        //public int TaskId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public Fieldo_Task? Task { get; set; }
        public int? UserId { get; set; }
      //  public string? NotificationImage { get; set; }
    }

    public class NotificationDto
    {
        //public int TaskId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }
        //  public string? NotificationImage { get; set; }
    }
}
