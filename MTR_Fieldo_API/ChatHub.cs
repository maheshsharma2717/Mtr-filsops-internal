
using Application.Common;
using Application.Models;
using FirebaseAdmin.Messaging;
using Microsoft.AspNet.SignalR.Hosting;
using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MTR_Fieldo_API.Models;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service;
using MTR_Fieldo_API.Service.IService;
using MTR_Fieldo_API.ViewModels;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MTR_Fieldo_API
{
    [Authorize]
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly UserConnectionManager _userConnectionManager;
        private readonly MtrContext _context;
        private readonly IUserService _userService;
        private readonly ICommonService _commonService;
        private readonly IConfiguration _configuration;
        private static string bucketName;
        private readonly IFirebaseNotifications _firebaseNotifications;
        public ChatHub(UserConnectionManager connectionManager, MtrContext mtrContext, IUserService userService,ICommonService commonService, IConfiguration configuration, IFirebaseNotifications firebaseNotifications)
        {
            _userConnectionManager = connectionManager;
            _context = mtrContext;
            _userService = userService;
            _commonService = commonService;
            _configuration = configuration;
            bucketName = (Convert.ToBoolean(_configuration.GetSection("Environment:Staging").Value) ? _configuration.GetSection("AWS:DEVBucketName").Value.ToString() : _configuration.GetSection("AWS:BucketName").Value.ToString());
            _firebaseNotifications = firebaseNotifications;
        }
        public async Task SendMessage(string user, int sendTo, string message)
        {
            var connectionId = _userConnectionManager.GetConnectionId(sendTo);
            if (connectionId != null)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", user, message);
            }
        }
        public override async Task OnConnectedAsync()
        {
            int userId = Convert.ToInt32(Context.User.Claims
                .FirstOrDefault(c => c.Type.Equals("id", StringComparison.InvariantCultureIgnoreCase))?.Value);

            //var userId = UserConnectionManager.userCount++;
            _userConnectionManager.AddConnection(userId, Context.ConnectionId);

            await base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string connectionId = Context.ConnectionId;
            _userConnectionManager.RemoveConnection(connectionId);

            return base.OnDisconnectedAsync(exception);
        }
        public async Task<int?> SendNewMessage(int sendTo, string message)
        {
            int sendBy = Convert.ToInt32(Context.User.Claims
               .FirstOrDefault(c => c.Type.Equals("id", StringComparison.InvariantCultureIgnoreCase))?.Value);

            var sender = _userService.GetUserById(sendBy);
            var receiver = _userService.GetUserById(sendTo);


            var connectionId = _userConnectionManager.GetConnectionId(sendTo);
            if (sender != null && receiver != null)
            {
                var chatMessage = new Fieldo_Message
                {
                    SendBy = sendBy,
                    SendTo = sendTo,
                    Message = message,
                    SendTime = DateTime.UtcNow,
                    IsReceived = false,
                    UpdatedAt = DateTime.UtcNow
                };
               
                _context.Fieldo_Messages.Add(chatMessage);
                await _context.SaveChangesAsync();
                NotificationDto notification = new NotificationDto()
                {
                    Subject = $"New message from {sender.FirstName}",
                    Description="New Nessage Recieved",
                    UserId = receiver.Id,
                };
                await _firebaseNotifications.SendFirebaseNotification(notification.UserId, notification.Subject, notification.Description, WorkerNotificationType.NewMessage.ToString(), notification.UserId.ToString());

                ChatMessageViewModel chat = new ChatMessageViewModel
                {
                    Id = chatMessage.Id,
                    SendBy = sendBy,
                    IsReceived = chatMessage.IsReceived,
                    Message = chatMessage.Message,
                    SenderName = sender.FirstName,
                    SendTime = DateTime.UtcNow,
                    SendTo = sendTo,
                    SenderPicture = !string.IsNullOrEmpty(sender.ProfileUrl) ? _commonService.GeneratePreSignedURL(bucketName, sender.ProfileUrl, 7200) : ""
                };
               
                if (connectionId != null)
                {

                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", sender.Id, JsonConvert.SerializeObject(chat));
                }
                return chat.Id;
              //  Clients.User(receiver.Id.ToString()).SendAsync("ReceiveMessage", sender.Id, message);
            }
            return null;
        }
        public async Task MarkMessageAsRead(int messageId)
        {
            var message = await _context.Fieldo_Messages.FindAsync(messageId);
            if (message != null)
            {
                message.IsReceived = true;
                message.UpdatedAt = DateTime.UtcNow;
              if(await _context.SaveChangesAsync() > 0)
                {
                    var senderConnectionId = _userConnectionManager.GetConnectionId(message.SendBy);
                    if (senderConnectionId != null)
                    {
                        await Clients.Client(senderConnectionId).SendAsync("MessageRead", messageId);
                    }
                }  ;
            }

           
        }

        public async Task GetChatHistoryWithUser(int otherUserId, int pageNumber, int pageSize)
        {
         
            ResponseDto _response = new ResponseDto();
            try
            {
                int userId = Convert.ToInt32(Context.User.Claims
      .FirstOrDefault(c => c.Type.Equals("id", StringComparison.InvariantCultureIgnoreCase))?.Value);

                // Fetch all messages between the current user and the specified user
                var messagesQuery = _context.Fieldo_Messages
                    .Where(m => (m.SendBy == userId && m.SendTo == otherUserId) || (m.SendBy == otherUserId && m.SendTo == userId))
                    .OrderByDescending(x => x.SendTime);

                // Get the total count of messages
                var totalMessages = await messagesQuery.CountAsync();

                // Apply paging
                var messages = await messagesQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // Fetch user details for userId and otherUserId
                var userDetails = await _context.Fieldo_UserDetails
                    .Where(u => u.Id == userId || u.Id == otherUserId)
                    .ToDictionaryAsync(u => u.Id, u => new { u.FirstName, u.ProfileUrl });

                // Project the final result
                var chatMessages = messages.Select(m => new ChatMessageViewModel
                {
                    Id = m.Id,
                    SendBy = m.SendBy,
                    SenderName = userDetails.ContainsKey(m.SendBy) ? userDetails[m.SendBy].FirstName : string.Empty,
                    SenderPicture = userDetails.ContainsKey(m.SendBy) && !string.IsNullOrEmpty(userDetails[m.SendBy].ProfileUrl)
                        ? _commonService.GeneratePreSignedURL(bucketName, userDetails[m.SendBy].ProfileUrl, 7200)
                        : string.Empty,
                    SendTo = m.SendTo,
                    Message = m.Message,
                    SendTime = m.SendTime,
                    IsReceived = m.IsReceived // Adjust this if necessary
                }).ToList();

                _response.IsSuccess = true;
                _response.Result = new PagedResult<ChatMessageViewModel>
                {
                    Items = chatMessages,
                    TotalCount = totalMessages,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                _response.Message = "Messages retrieved";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            await Clients.Caller.SendAsync("ReceiveChatHistory", _response);
        }

        public async Task MarkMessageAsReadByUser(int otherUserId)
        {
            int userId = Convert.ToInt32(Context.User.Claims
     .FirstOrDefault(c => c.Type.Equals("id", StringComparison.InvariantCultureIgnoreCase))?.Value);
            var messages = _context.Fieldo_Messages
                    .Where(m => (m.SendBy == otherUserId && m.SendTo == userId))
                    .ToList();
           // var messages = await _context.Fieldo_Messages.Where(x=>x.SendBy==userId).ToListAsync();
            if (messages != null)
            {
                foreach(var message in messages)
                {
                    message.IsReceived = true;
                    message.UpdatedAt = DateTime.UtcNow;
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        var senderConnectionId = _userConnectionManager.GetConnectionId(message.SendBy);
                        if (senderConnectionId != null)
                        {
                            await Clients.Client(senderConnectionId).SendAsync("MessageRead", message.Id);
                        }
                    };

                }
               
            }


        }
    }
}
