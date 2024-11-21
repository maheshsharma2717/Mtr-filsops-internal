namespace MTR_Fieldo_API.ViewModels
{
    public class ChatMessageViewModel
    {
        public int Id { get; set; }
        public int SendBy { get; set; }
        public string? SenderName { get; set; }
        public string? SenderPicture { get; set; }
        public int SendTo { get; set; }
        public string Message { get; set; }
        public DateTime SendTime { get; set; }
        public bool IsReceived { get; set; }
 
    }
}
