using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace WhatsappWeb.Api.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private static readonly Dictionary<string, string> ConnectedUsers = new(StringComparer.OrdinalIgnoreCase);

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                lock (ConnectedUsers)
                {
                    ConnectedUsers[userId] = Context.ConnectionId;
                }

                await Clients.Others.SendAsync("UserConnected", userId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                lock (ConnectedUsers)
                {
                    ConnectedUsers.Remove(userId);
                }

                await Clients.Others.SendAsync("UserDisconnected", userId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public static string? GetConnectionIdForUser(string userId)
        {
            lock (ConnectedUsers)
            {
                return ConnectedUsers.TryGetValue(userId, out var connectionId) ? connectionId : null;
            }
        }
    }
}