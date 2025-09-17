using KosDrive.Data;
using KosDrive.DTOs;
using KosDrive.Interfaces;
using KosDrive.Models;
using Microsoft.EntityFrameworkCore;

namespace KosDrive.Services
{
    public class RideService : IRideService
    {
        private readonly ApplicationDbContext _context;

        public RideService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RideHistoryDto>> GetRideHistoryAsync(string userId, string userType)
        {
            var userGuid = Guid.Parse(userId);

            IQueryable<Ride> query = _context.Rides
                .Include(r => r.Rider)
                .Include(r => r.Driver)
                .Where(r => r.Status == RideStatus.Completed
                         || r.Status == RideStatus.CancelledByRider
                         || r.Status == RideStatus.CancelledByDriver);

            if (userType == "Rider")
            {
                query = query.Where(r => r.RiderId == userGuid);
            }
            else if (userType == "Driver")
            {
                query = query.Where(r => r.DriverId == userGuid);
            }
            else
            {
                return new List<RideHistoryDto>();
            }

            return await query
                .OrderByDescending(r => r.RequestedAt)
                .Select(r => new RideHistoryDto
                {
                    RideId = r.Id,
                    RequestedAt = r.RequestedAt,
                    Status = r.Status.ToString(),

                    PickupAddress = r.PickupAddress, 
                    DestinationAddress = r.DestinationAddress, 

                    RiderName = r.Rider != null ? $"{r.Rider.FirstName} {r.Rider.LastName}" : null,
                    DriverName = r.Driver != null ? $"{r.Driver.FirstName} {r.Driver.LastName}" : null,

                    Price = r.Price
                })
                .ToListAsync();
        }
    }
}
