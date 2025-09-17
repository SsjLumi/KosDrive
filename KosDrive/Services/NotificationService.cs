using KosDrive.Models;
using KosDrive.SignalR;
using Microsoft.AspNetCore.SignalR;
using System.Reflection;

namespace KosDrive.Services
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public async Task NotifyDriverNewRide(string driverId, Ride ride)
        {
            await _hubContext.Clients.User(driverId).SendAsync("ReceiverNotification", new {
                Title = "New Ride Request",
                Message = $"You have a new ride request from {ride.PickupAddress} to {ride.DestinationAddress}.",
                RideId = ride.Id
            });
        }

    public async Task NotifyUserTripStarted(string userId, Ride ride)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", new
            {
                Title = "Trip Started",
                Message = $"Your driver has started the trip",
                RideId = ride.Id
            });
        }
    }
}
