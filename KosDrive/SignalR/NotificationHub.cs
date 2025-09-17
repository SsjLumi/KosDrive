using Microsoft.AspNetCore.SignalR;

namespace KosDrive.SignalR
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            Console.WriteLine($"User connected: {userId}");

            if(Context.User?.Identity?.IsAuthenticated == true)
            {
                var userType = Context.User.FindFirst("userType")?.Value;

                if (userType == "Driver")
                    await Groups.AddToGroupAsync(Context.ConnectionId, "drivers");
                else if (userType == "Rider")
                    await Groups.AddToGroupAsync(Context.ConnectionId, "riders");

                if (!string.IsNullOrEmpty(userId))
                    await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            Console.WriteLine($"User disconnected: {userId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
