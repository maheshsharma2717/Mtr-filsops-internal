using Application.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Owin.Security.Provider;
using MTR_Fieldo_API.Models;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service.IService;
using MTR_Fieldo_API.ViewModels;

namespace MTR_Fieldo_API.Service
{
    public class MessageService : IMessageService
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly MtrContext _context;
        private readonly ResponseDto _response;
        private readonly UserConnectionManager _userConnectionManager;
        private readonly ICommonService _commonService;
        private readonly IConfiguration _configuration;
        private static string bucketName;

        public MessageService(MtrContext mtrContext, IHubContext<ChatHub> hubContext, UserConnectionManager userConnectionManager, ICommonService commonService, IConfiguration configuration)
        {
            _context = mtrContext;
            _hubContext = hubContext;
            _userConnectionManager = userConnectionManager;
            _response = new();
            _commonService = commonService;
            _configuration = configuration;
            bucketName = (Convert.ToBoolean(_configuration.GetSection("Environment:Staging").Value) ? _configuration.GetSection("AWS:DEVBucketName").Value.ToString() : _configuration.GetSection("AWS:BucketName").Value.ToString());

        }

        public async Task<ResponseDto> SendMessage(int sendBy, int sendTo, string message)
        {
            try
            {
                var connectionId = _userConnectionManager.GetConnectionId(sendTo);

                Fieldo_Message newMessage = new()
                {
                    SendBy = sendBy,
                    SendTo = sendTo,
                    SendTime = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Message = message
                };

                if (connectionId != null)
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", sendBy, message);
                    newMessage.IsReceived = true;
                }
                else
                {
                    newMessage.IsReceived = false;
                }

                await _context.Fieldo_Messages.AddAsync(newMessage);
                await _context.SaveChangesAsync();
                _response.Result = newMessage;
                _response.Message = "Message sent successfully";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }
        public async Task<ResponseDto> SendNotificationToUser(MessageModel message)
        {
            try
            {
                string connectionId = _userConnectionManager.GetConnectionId(message.UserId);
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", message.Message);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }
        public async Task<ResponseDto> SendNotificationToAllUsers(string message)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }

        public async Task<ResponseDto> GetUserMessages(int userId)
        {
            try
            {
                var data = _context.Fieldo_Messages
                    .Include(u => u.UserDetailsSendTo)
                    .Include(u => u.UserDetailsSendBy)
                    .Where(m => m.SendBy == userId || m.SendTo == userId)
                    .OrderBy(x => x.SendTime);
                _response.Result = data;
                _response.Message = "Messages retrieved";

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";

            }
            return _response;
        }
        public async Task<ResponseDto> GetChatHistoryWithUser(int userId, int otherUserId)
        {
            try
            {
                // Fetch all messages between the current user and the specified user
                var messages = await _context.Fieldo_Messages
                    .Where(m => (m.SendBy == userId && m.SendTo == otherUserId) || (m.SendBy == otherUserId && m.SendTo == userId))
                    .OrderByDescending(x => x.SendTime)
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
                _response.Result = chatMessages;
                _response.Message = "Messages retrieved";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }
        public async Task<ResponseDto> GetChatHistoryWithUser(int userId, int otherUserId, int pageNumber, int pageSize)
        {
            try
            {
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
            return _response;
        }

        public async Task<ResponseDto> GetChatHistory(int userId)
        {
            try
            {
                // Fetch all messages
                var messages = await _context.Fieldo_Messages
                    .Where(m => m.SendBy == userId || m.SendTo == userId)
                    .OrderByDescending(x => x.SendTime)
                    .ToListAsync();

                // Fetch unique user IDs from messages
                var userIds = messages.Select(m => m.SendBy).Distinct().ToList();

                // Fetch user details for the unique user IDs
                var userDetails = await _context.Fieldo_UserDetails
                    .Where(u => userIds.Contains(u.Id))
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
                _response.Result = chatMessages;
                _response.Message = "Messages retrieved";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }

        public async Task<ResponseDto> GetChatHistory(int userId, int pageNumber, int pageSize)
        {
            try
            {
                var query = _context.Fieldo_Messages
                    .Where(m => m.SendBy == userId || m.SendTo == userId)
                    .OrderByDescending(x => x.SendTime);

                var totalMessages = await query.CountAsync();

                var data = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(m => new
                    {
                        m.Id,
                        m.SendBy,
                        m.SendTo,
                        m.Message,
                        m.SendTime,
                        m.IsReceived,
                        SenderDetails = _context.Fieldo_UserDetails
                            .Where(u => u.Id == m.SendBy)
                            .Select(u => new
                            {
                                u.FirstName,
                                u.ProfileUrl
                            })
                            .FirstOrDefault()
                    })
                    .ToListAsync();

                var chatMessages = data.Select(m => new ChatMessageViewModel
                {
                    Id = m.Id,
                    SendBy = m.SendBy,
                    SenderName = m.SenderDetails.FirstName,
                    SenderPicture = !string.IsNullOrEmpty(m.SenderDetails.ProfileUrl) ? _commonService.GeneratePreSignedURL(bucketName, m.SenderDetails.ProfileUrl, 7200) : "",
                    SendTo = m.SendTo,
                    Message = m.Message,
                    SendTime = m.SendTime,
                    IsReceived = m.IsReceived
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
            return _response;
        }

        //public async Task<ResponseDto> GetChatUserList(int userId)
        //{
        //    try
        //    {
        //        // Fetch all messages where the user is sender or receiver, ordered by SendTime descending
        //        var messages = await _context.Fieldo_Messages
        //            .Where(m => m.SendBy == userId || m.SendTo == userId)
        //            .OrderByDescending(m => m.SendTime) // Order by SendTime descending
        //            .ToListAsync();

        //        // Group messages by SendTo to identify unique users involved in the chat
        //        var uniqueUsers = messages
        //            .GroupBy(m => m.SendBy == userId ? m.SendTo : m.SendBy)
        //            .Select(g => new
        //            {
        //                UserId = g.Key,
        //                LatestMessageTime = g.Max(m => m.SendTime), // Get the latest message time
        //                UnreadCount = g.Count(m => m.SendTo == userId && !m.IsReceived)
        //            })
        //            .OrderByDescending(u => u.LatestMessageTime) // Order by LatestMessageTime descending
        //            .ToList();

        //        // Fetch user details for the unique users
        //        var userDetails = await _context.Fieldo_UserDetails
        //            .Where(u => uniqueUsers.Select(uu => uu.UserId).Contains(u.Id))
        //            .ToDictionaryAsync(u => u.Id, u => new { u.FirstName, u.ProfileUrl });

        //        // Prepare the response with ordered user list
        //        var userList = uniqueUsers.Select(uu =>
        //        {
        //            var userDetail = userDetails.ContainsKey(uu.UserId) ? userDetails[uu.UserId] : null;
        //            return new
        //            {
        //                UserId = uu.UserId,
        //                UserName = userDetail?.FirstName ?? "Unknown",
        //                ProfilePicture = userDetail != null && !string.IsNullOrEmpty(userDetail.ProfileUrl)
        //                    ? _commonService.GeneratePreSignedURL(bucketName, userDetail.ProfileUrl, 7200)
        //                    : null,
        //                UnreadMessageCount = uu.UnreadCount
        //            };
        //        }).ToList();

        //        _response.IsSuccess = true;
        //        _response.Result = userList;
        //        _response.Message = "Chat user list retrieved successfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
        //    }
        //    return _response;
        //}
        public async Task<ResponseDto> GetChatUserList(int userId)
        {
            try
            {
                // Fetch all messages where the user is sender or receiver, ordered by SendTime descending
                var messages = await _context.Fieldo_Messages
                    .Where(m => m.SendBy == userId || m.SendTo == userId)
                    .OrderByDescending(m => m.SendTime)
                    .ToListAsync();

                // Group messages by SendTo to identify unique users involved in the chat
                var uniqueUsers = messages
                    .GroupBy(m => m.SendBy == userId ? m.SendTo : m.SendBy)
                    .Select(g => new
                    {
                        UserId = g.Key,
                        LatestMessageTime = g.Max(m => m.SendTime), // Get the latest message time
                        LastMessage = g.OrderByDescending(m => m.SendTime).FirstOrDefault(), // Get the last message
                        UnreadCount = g.Count(m => m.SendTo == userId && !m.IsReceived)
                    })
                    .OrderByDescending(u => u.LatestMessageTime)
                    .ToList();

                // Fetch user details for the unique users
                var userDetails = await _context.Fieldo_UserDetails
                    .Where(u => uniqueUsers.Select(uu => uu.UserId).Contains(u.Id))
                    .ToDictionaryAsync(u => u.Id, u => new { u.FirstName, u.ProfileUrl });

                // Prepare the response with ordered user list
                var userList = uniqueUsers.Select(uu =>
                {
                    var userDetail = userDetails.ContainsKey(uu.UserId) ? userDetails[uu.UserId] : null;
                    return new
                    {
                        UserId = uu.UserId,
                        UserName = userDetail?.FirstName ?? "Unknown",
                        ProfilePicture = userDetail != null && !string.IsNullOrEmpty(userDetail.ProfileUrl)
                            ? _commonService.GeneratePreSignedURL(bucketName, userDetail.ProfileUrl, 7200)
                            : null,
                        UnreadMessageCount = uu.UnreadCount,
                        LastMessage = new
                        {
                            Id = uu.LastMessage?.Id,
                            Message = uu.LastMessage?.Message,
                            SendTime = uu.LastMessage?.SendTime,
                            SendBy = uu.LastMessage?.SendBy
                        }
                    };
                }).ToList();

                _response.IsSuccess = true;
                _response.Result = userList;
                _response.Message = "Chat user list retrieved successfully";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }


        public async Task<ResponseDto> MarkMessageAsRead(int messageId)
        {
            try
            {
                var message = await _context.Fieldo_Messages.FindAsync(messageId);
                if (message != null)
                {
                    message.IsReceived = true;
                    message.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    _response.IsSuccess = true;
                    _response.Message = "Chat user list retrieved successfully";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Message not found with id {messageId} ";
                }

               
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = $"{ex.Message} - {ex.InnerException?.Message}";
            }
            return _response;
        }



    }
}
