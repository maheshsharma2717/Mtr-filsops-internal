using Application.Models;

namespace MTR_Fieldo_API.Service.IService
{
    public interface IFirebaseNotifications
    {
        Task SendFirebaseNotification(int? userId, string title, string body, string? notificationType, string? relatedId = null);
    }
}
