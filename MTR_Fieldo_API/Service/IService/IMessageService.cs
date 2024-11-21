using Microsoft.AspNet.SignalR.Messaging;
using MTR_Fieldo_API.Models;
using MTR_Fieldo_API.Models.Dto;

namespace MTR_Fieldo_API.Service.IService
{
    public interface IMessageService
    {
        Task<ResponseDto> SendMessage(int sendBy, int sendTo, string message);
        Task<ResponseDto> SendNotificationToAllUsers(string notification);
        Task<ResponseDto> SendNotificationToUser(MessageModel message);
        Task<ResponseDto> GetUserMessages(int userId);
        Task<ResponseDto> GetChatHistory(int userId);
        Task<ResponseDto> GetChatHistory(int userId, int pageNumber, int pageSize);
        Task<ResponseDto> GetChatHistoryWithUser(int userId, int otherUserId);
        Task<ResponseDto> GetChatHistoryWithUser(int userId, int otherUserId, int pageNumber, int pageSize);
        Task<ResponseDto> GetChatUserList(int userId);
        Task<ResponseDto> MarkMessageAsRead(int messageId);
    }
}
