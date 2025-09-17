namespace KosDrive.Interfaces
{
    public interface ILocationCacheService
    {
        Task UpdateDriverLocationAsync(Guid driverId, double lat, double lng);
        Task<(double lat, double lng)?> GetDriverLocationAsync(Guid driverId);
    }
}
