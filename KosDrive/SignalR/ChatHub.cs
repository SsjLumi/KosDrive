using KosDrive.Data;
using KosDrive.Models;
using Microsoft.AspNetCore.SignalR;

namespace KosDrive.Services
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string conversationId, string senderId, string message)
        {
            var msg = new Message
            {
                ConversationId = conversationId,
                SenderId = senderId,
                Content = message,
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(msg);
            _context.SaveChangesAsync();

            await Clients.Group(conversationId)
                .SendAsync("ReceiveMessage", conversationId, senderId, message);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var conversationId = httpContext.Request.Query["conversationId"];

            if(!string.IsNullOrEmpty(conversationId) )
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task ConfirmDelivered(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if(message != null && !message.Delivered)
            {
                message.Delivered = true;
                message.DeliveredAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                await Clients.User(message.SenderId)
                    .SendAsync("MessageDelivered", messageId, message.DeliveredAt);
            }
        }

        public async Task ConfirmSeen(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if(message != null && !message.Seen)
            {
                message.Seen = true;
                message.SeenAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                await Clients.User(message.SenderId)
                    .SendAsync("MessageSeen", messageId, message.SeenAt);
            }
        }
    }
}
