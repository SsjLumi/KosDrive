using Microsoft.AspNetCore.SignalR;

namespace KosDrive.Services
{
    public class RideRequestHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public async Task JoinAsDriver(string driverId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"driver_{driverId}" );
            Console.WriteLine($"Driver {driverId} joined group");
        }

        public async Task LeaveDriverGroup(string driverId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"driver_{driverId}");
            Console.WriteLine($"Driver {driverId} has been removed.");
        }
    }
}
