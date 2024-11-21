using Application.Models;
using MTR_Fieldo_API.Models;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service.IService;

namespace MTR_Fieldo_API.Service
{
    public class NotificationService : INotificationService
    {
        private readonly MtrContext _context;
        private readonly ResponseDto _response;
        //private readonly ITaskService _taskService;
        private readonly IMessageService _messageService;
        public NotificationService(MtrContext context, IMessageService messageService)
        {
            _context = context;
            _response = new();
            //_taskService = taskService;
            _messageService = messageService;
        }

        public async Task<ResponseDto> AddNotification(NotificationRequestDto nofication)
        {
            try
            {
                Fieldo_Notification _notification = new()
                {
                   UserId = nofication.UserId,
                    Subject = nofication.Subject,
                    Description = nofication.Description,
                    IsRead = false,
                    CreatedAt = DateTime.Now,
                };
                if(nofication.Task!=null )
                {
                    _notification.TaskId = nofication.Task.Id;
                }
                await _context.Fieldo_Notifications.AddAsync(_notification);
                await _context.SaveChangesAsync();
                _response.Message = "Success";

                await NotifyUser(nofication);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }

            return _response;
        }
        private async Task NotifyUser(NotificationRequestDto notificationRequest)
        {

            if (notificationRequest.Task != null)
            {

                MessageModel messageModel = new()
                {
                    UserId = notificationRequest.Task.CreatedBy,
                    Message = $"{notificationRequest.Subject} - {notificationRequest.Description}",
                };
                await _messageService.SendNotificationToUser(messageModel);

                messageModel.UserId = notificationRequest.Task.AssignedBy.Value;
                await _messageService.SendNotificationToUser(messageModel);

                messageModel.UserId = notificationRequest.Task.AssignedTo.Value;
                await _messageService.SendNotificationToUser(messageModel);
            }

        }
        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Fieldo_Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
