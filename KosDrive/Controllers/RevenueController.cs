using KosDrive.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KosDrive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RevenueController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public RevenueController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("platform-revenue")]
        public async Task<IActionResult> GetPlatformRevenue()
        {
            var totalRevenue = await _context.Rides
                .Where(r => r.Status == Models.RideStatus.Completed)
                .SumAsync(r => r.Price);

            return Ok(new { totalRevenue });
        }

        [HttpGet("driver-earnings/{driverId}")]
        public async Task<IActionResult> GetDriverEarnings(Guid driverId)
        {
            var total = await _context.Rides
                .Where(r => r.DriverId == driverId && r.Status == Models.RideStatus.Completed)
                .SumAsync(r => r.Price);

            return Ok(new { driverId, earnings = total });
        }

        [HttpGet("earnings-by-city")]
        public async Task<IActionResult> GetEarningsByCity()
        {
            var data = await _context.Rides
                .Where(r => r.Status == Models.RideStatus.Completed)
                .GroupBy(r => r.City).Select(g => new
                {
                    City = g.Key,
                    Total = g.Sum(r => r.Price)
                }).ToListAsync();

            return Ok(data);
        }
    }
}
