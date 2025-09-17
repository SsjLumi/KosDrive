using KosDrive.DTOs;

namespace KosDrive.Interfaces
{
    public interface IRideService
    {
        Task<List<RideHistoryDto>> GetRideHistoryAsync(string userId, string userType);
    }
}
