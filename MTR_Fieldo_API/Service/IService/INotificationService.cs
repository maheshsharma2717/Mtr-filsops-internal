using MTR_Fieldo_API.Models.Dto;

namespace MTR_Fieldo_API.Service.IService
{
    public interface INotificationService
    {
        Task<ResponseDto> AddNotification(NotificationRequestDto nofication);
        Task MarkAsReadAsync(int notificationId);
    }
}
