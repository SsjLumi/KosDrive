using KosDrive.DTOs;
using KosDrive.Models;
using Microsoft.AspNetCore.SignalR;

namespace KosDrive.Services
{
    public class RideRequestService
    {
        private readonly IHubContext<RideRequestHub> _hubContext;
        private static readonly Dictionary<int, CancellationTokenSource> _timers = new();

        public async Task<int> CreateRideRequestAsync(RideRequestDto dto)
        {
            var request = new RideRequest
            {
                RiderId = dto.RiderId,
                DriverId = dto.DriverId,
                PickupLocation = dto.PickupLocation,
                DropoffLocation = dto.DropoffLocation,
                Status = RideStatus.Pending,
                RequestedAt = DateTime.UtcNow
            }; 
            int requestId = 1;

            await _hubContext.Clients.Group($"driver_{dto.DriverId}").SendAsync("ReceiveRideRequest", new
            {
                RequestId = requestId,
                RiderId = dto.RiderId,
                PickupLocation = dto.PickupLocation,
                DropoffLocation = dto.DropoffLocation
            });

            var cts = new CancellationTokenSource();
            _timers[requestId] = cts;
            _ = Task.Delay(TimeSpan.FromMinutes(1), cts.Token).ContinueWith(async task =>
            {
                if (!task.IsCanceled)
                {
                    await ExpireRequestAsync(requestId);
                }
            });

            return requestId;
        }

        public async Task AcceptRequestAsync(int requestId)
        {
            CancelTimer(requestId);
            await _hubContext.Clients.Group($"ride_{requestId}").SendAsync("RideAccepted");
        }

        public async Task DeclineRequestAsync(int requestId)
        {
            CancelTimer(requestId);
            await _hubContext.Clients.Group($"ride_{requestId}").SendAsync("RideDeclined");
        }

        private async Task ExpireRequestAsync(int requestId)
        {
            Console.WriteLine($"Ride request {requestId} expired");
            await _hubContext.Clients.Group($"ride_{requestId}").SendAsync("RideExpired");
        }

        private void CancelTimer(int requestId)
        {
            if (_timers.TryGetValue(requestId, out var cts))
            {
                cts.Cancel();
                _timers.Remove(requestId);
            }
        }

    }
}
