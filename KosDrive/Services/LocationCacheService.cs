using KosDrive.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace KosDrive.Services
{
    public class LocationCacheService : ILocationCacheService
    {
        private readonly IDatabase _redisDb;

        public LocationCacheService(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }
        public async Task<(double lat, double lng)?> GetDriverLocationAsync(Guid driverId)
        {
            var value = await _redisDb.StringGetAsync($"driver:loc:{driverId}");
            if (!value.HasValue) return null;

            var obj = JsonSerializer.Deserialize<Dictionary<string, double>>(value!);
            return (obj["lat"], obj["lng"]);
        }

        public async Task UpdateDriverLocationAsync(Guid driverId, double lat, double lng)
        {
            var json = JsonSerializer.Serialize(new { lat, lng });
            await _redisDb.StringSetAsync($"driver:loc:{driverId}", json, TimeSpan.FromMinutes(10));
        }
    }
}
