using Application.Common;

namespace MTR_Fieldo_API.Service.IService
{
    public interface IAdminFirebaseNotifications
    {
        Task SendAdminFirebaseNotification(AdminUserType role, int? domainId, string title, string body, string? notificationType, string? relatedId = null);
    }
}
