using Amazon.Runtime.Internal.Transform;
using Application.Models;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service.IService;
using Polly;

namespace MTR_Fieldo_API.Service
{
    public class FirebaseNotifications : IFirebaseNotifications
    {
        private readonly MtrContext _context;

        public FirebaseNotifications(MtrContext context)
        {
            _context = context;
        }

        public async Task SendFirebaseNotification(int? userId, string title, string body, string? notificationType, string? relatedId = null)
        {
            var deviceTokens = _context.Fieldo_DeviceToken
                .Where(t => t.UserID == userId)
                .Select(t => t.DeviceToken)
                .ToList();

            if (!deviceTokens.Any())
            {
                return;
              //  throw new Exception("No device tokens found for the user.");
            }

            var messaging = FirebaseMessaging.DefaultInstance;
            // Handle null notificationType by providing a default value (e.g., "general")
            var type = notificationType ?? "general";// Default to "general" if notificationType is null
            foreach (var token in deviceTokens)
            {
                var message = new Message
                {
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body,
                    },
                    Token = token,
                    Data = new Dictionary<string, string>
            {
                { "type", type }, // Use the non-null notificationType
                { "relatedId", relatedId ?? string.Empty }, // Handle nullable relatedId,
                        {"Title", title },
                        {"Body", body}
            }

                };

                try
                {
                    // Send the notification
                    var result = await messaging.SendAsync(message);

                    // Message sent successfully, you can log success here if needed
                }
                catch (FirebaseMessagingException ex)
                {
                    // Handle token invalidation and other errors
                    if (IsTokenInvalid(ex))
                    {
                        await RemoveInvalidTokenFromDatabase(token);
                    }
                }
            }
        }

        private bool IsTokenInvalid(FirebaseMessagingException error)
        {
            return error.MessagingErrorCode == MessagingErrorCode.Unregistered ||
                   error.MessagingErrorCode == MessagingErrorCode.InvalidArgument ||
                   error.MessagingErrorCode == MessagingErrorCode.SenderIdMismatch ||
                   error.MessagingErrorCode == MessagingErrorCode.ThirdPartyAuthError;
        }

        private async Task RemoveInvalidTokenFromDatabase(string token)
        {
            var tokenEntry = _context.Fieldo_DeviceToken.FirstOrDefault(t => t.DeviceToken == token);

            if (tokenEntry != null)
            {
                _context.Fieldo_DeviceToken.Remove(tokenEntry);
                await _context.SaveChangesAsync();
            }
        }
    }
}
