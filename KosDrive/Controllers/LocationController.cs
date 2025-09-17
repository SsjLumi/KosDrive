using KosDrive.Data;
using KosDrive.DTOs;
using KosDrive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KosDrive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public LocationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> UpdateLocation([FromBody] LocationUpdateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var driver = await _context.Users.FindAsync(Guid.Parse(userId));

            if (driver == null || driver.UserType != UserType.Driver)
                return BadRequest("Invalid driver");

            driver.CurrentLatitude = dto.Latitude;
            driver.CurrentLongitude = dto.Longitude;
            driver.LastActivatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("nearby-drivers")]
        [Authorize]
        public async Task<IActionResult> GetNearbyDrivers([FromQuery] double lat, [FromQuery] double lng, [FromQuery] double radiusKm = 10)
        {
            var drivers = await _context.Users
                .Where(u => u.UserType == UserType.Driver && u.IsAvailable && !u.IsDeleted)
                .Where(u => u.CurrentLatitude != null && u.CurrentLongitude != null)
                .ToListAsync();

            var nearbyDrivers = drivers
                .Select(u => new NearbyDriverDto
                {
                    Id = u.Id,
                    FullName = u.FirstName + " " + u.LastName,
                    ProfilePictureUrl = u.ProfilePictureUrl,
                    Latitude = u.CurrentLatitude!.Value,
                    Longitude = u.CurrentLongitude!.Value,
                    DistanceKm = Haversine(u.CurrentLatitude.Value, u.CurrentLongitude.Value, lat, lng)
                })
                .Where(d => d.DistanceKm <= radiusKm)
                .OrderBy(d => d.DistanceKm)
                .ToList();

            return Ok(nearbyDrivers);
        }

        private double Haversine(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371;
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
        private double ToRadians(double angle) => angle * Math.PI / 180;
    }
}
