using KosDrive.AnalyticsDto;
using KosDrive.Data;
using KosDrive.Interfaces;
using KosDrive.Models;
using Microsoft.EntityFrameworkCore;

namespace KosDrive.Services
{
    public class AdminAnalyticsService : IAdminAnalyticsService
    {
        private readonly ApplicationDbContext _context;
        
        public AdminAnalyticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<DateCountDto>>> GetCancellationTrendsAsync(DateTime? from = null)
        {
            try
            {
                var start = from ?? DateTime.UtcNow.AddDays(-30);

                var cancellations = await _context.Rides
                    .Where(r =>
                        (r.Status == RideStatus.CancelledByRider || r.Status == RideStatus.CancelledByDriver) &&
                        r.RequestedAt >= start)
                    .GroupBy(r => r.RequestedAt.Date)
                    .Select(g => new DateCountDto
                    {
                        Date = g.Key,
                        Count = g.Count()
                    })
                    .OrderBy(d => d.Date)
                    .ToListAsync();

                if (cancellations.Count == 0)
                    return OperationResult<List<DateCountDto>>.Failure("No cancellation data found");

                return OperationResult<List<DateCountDto>>.Success(cancellations);
            }catch(Exception ex)
            {
                return OperationResult<List<DateCountDto>>.Failure("An error occurred");
            }
        }


        public async Task<OperationResult<List<DriverRideCountDto>>> GetMostActiveDriversAsync(int top = 5)
        {
            try
            {
                var data = await _context.Rides
                    .Where(r => r.Status == Models.RideStatus.Completed)
                    .GroupBy(r => new
                    {
                        r.DriverId,
                        r.Driver.FirstName,
                        r.Driver.LastName,
                    }).Select(g => new DriverRideCountDto
                    {
                        DriverId = g.Key.DriverId,
                        FirstName = g.Key.FirstName,
                        LastName = g.Key.LastName,
                        RideCount = g.Count()
                    }).OrderByDescending(d => d.RideCount)
                    .Take(top).ToListAsync();

                return OperationResult<List<DriverRideCountDto>>.Success(data);
            }catch(Exception ex)
            {
                return OperationResult<List<DriverRideCountDto>>.Failure("An error occurred");
            }
        }

        public async Task<OperationResult<List<HourlyRideCountDto>>> GetPeakHoursAsync()
        {
            try
            {
                var peakHours = await _context.Rides
                    .GroupBy(r => r.RequestedAt.Hour)
                    .Select(g => new HourlyRideCountDto
                    {
                        Hour = g.Key,
                        Count = g.Count()
                    }).OrderBy(g => g.Hour).ToListAsync();

                if (peakHours.Count == 0)
                    return OperationResult<List<HourlyRideCountDto>>.Failure("No peak hour data found");

                return OperationResult<List<HourlyRideCountDto>>.Success(peakHours);
            }catch(Exception ex)
            {
                return OperationResult<List<HourlyRideCountDto>>.Failure("An error occurred");
            }
        }


        public async Task<OperationResult<List<LocationCountDto>>> GetTopCitiesAsync(int top = 5)
        {
            try
            {
                var cities = await _context.Rides
                    .Where(r => !string.IsNullOrEmpty(r.City))
                    .GroupBy(r => r.City)
                    .Select(g => new LocationCountDto
                    {
                        Location = g.Key,
                        Count = g.Count()
                    }).OrderByDescending(c => c.Count)
                    .Take(top).ToListAsync();

                if (cities.Count == 0)
                    return OperationResult<List<LocationCountDto>>.Failure("No city data found");

                return OperationResult<List<LocationCountDto>>.Success(cities);
            }catch(Exception ex)
            {
                return OperationResult<List<LocationCountDto>>.Failure("An error occurred");
            }
        }

        public async Task<OperationResult<List<LocationCountDto>>> GetTopPickupLocationsAsync(int top = 5)
        {
            try
            {
                var locations = await _context.Rides
                    .Where(r => !string.IsNullOrEmpty(r.PickupAddress))
                    .GroupBy(r => r.PickupAddress)
                    .Select(g => new LocationCountDto
                    {
                        Location = g.Key,
                        Count = g.Count()
                    }).OrderByDescending(l => l.Count)
                    .Take(top).ToListAsync();

                if (locations.Count == 0)
                    return OperationResult<List<LocationCountDto>>.Failure("No pickup location");

                return OperationResult<List<LocationCountDto>>.Success(locations);
            }catch(Exception ex)
            {
                return OperationResult<List<LocationCountDto>>.Failure("An error occurred");
            }
        }
    }
}
