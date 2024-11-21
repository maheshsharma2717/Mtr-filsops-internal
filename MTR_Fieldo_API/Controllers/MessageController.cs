using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MTR_Fieldo_API.Models;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service.IService;
using System.Drawing.Printing;
using System.Runtime.CompilerServices;

namespace MTR_Fieldo_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        [Route("SendNotificationToAllUsers")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> SendNotificationToAllUsers(string notification, int domainId)
        {
            return await _messageService.SendNotificationToAllUsers(notification);
        }

        [HttpPost]
        [Route("SendNotificationToUser")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> SendNotificationToUser(MessageModel message, int domainId)
        {
            return await _messageService.SendNotificationToUser(message);
        }

        [HttpPost]
        [Route("SendMessage")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> SendMessage(int sendTo, string message, int domainId)
        {
            int sendBy = GetCurrentUserId();
            return await _messageService.SendMessage(sendBy, sendTo, message);
        }


        [HttpGet]
        [Route("GetMessagesByUserId/{userId}")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetUserMessages(int userId, int domainId)
        {
            int sendBy = GetCurrentUserId();
            return await _messageService.GetUserMessages(userId);
        }
        [HttpGet]
        [Route("getchathistory/{pageNumber}/{pageSize}")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetChatHistory(int pageNumber, int pageSize, int domainId)
        {
            int sendBy = GetCurrentUserId();
            return await _messageService.GetChatHistory(sendBy, pageNumber, pageSize);
        }
        [HttpGet]
        [Route("getallchathistory")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetAllChatHistory(int domainId)
        {
            int sendBy = GetCurrentUserId();
            return await _messageService.GetChatHistory(sendBy);
        }
        [HttpGet]
        [Route("getchathistorywithuser")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetChatHistoryWithUser(int otherUserId, int pageNumber, int pageSize, int domainId)
        {
            int sendBy = GetCurrentUserId();
            return await _messageService.GetChatHistoryWithUser(sendBy, otherUserId, pageNumber, pageSize);
        }
        [HttpGet]
        [Route("getallchathistorywithuser")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetAllChatHistoryWithUser(int otherUserId, int domainId)
        {
            int sendBy = GetCurrentUserId();
            return await _messageService.GetChatHistoryWithUser(sendBy, otherUserId);
        }
        [HttpGet]
        [Route("getchatuserlist")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetChatUserList(int domainId)
        {
            int userId = GetCurrentUserId();
            return await _messageService.GetChatUserList(userId);
        }

        [HttpGet]
        [Route("markmessageasread")]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> MarkMessageAsRead(int messageId, int domainId)
        {
            int userId = GetCurrentUserId();
            return await _messageService.MarkMessageAsRead(messageId);
        }
        private int GetCurrentUserId()
        {
            return Convert.ToInt32(
                User.Claims
                .FirstOrDefault(c => c.Type.Equals("id", StringComparison.InvariantCultureIgnoreCase))?.Value);
        }


    }
}
