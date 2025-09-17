using KosDrive.AnalyticsDto;
using KosDrive.DTOs;
using KosDrive.Services;

namespace KosDrive.Interfaces
{
    public interface IAdminAnalyticsService
    {
        Task<OperationResult<List<DriverRideCountDto>>> GetMostActiveDriversAsync(int top = 5);
        Task<OperationResult<List<LocationCountDto>>> GetTopPickupLocationsAsync(int top = 5);
        Task<OperationResult<List<LocationCountDto>>> GetTopCitiesAsync(int top = 5);
        Task<OperationResult<List<HourlyRideCountDto>>> GetPeakHoursAsync();
        Task<OperationResult<List<DateCountDto>>> GetCancellationTrendsAsync(DateTime? from = null);
    }
}
