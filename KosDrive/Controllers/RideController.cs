using KosDrive.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KosDrive.Controllers
{
    [ApiController]
    [Route("ride/[controller]")]
    public class RideController : Controller
    {
        private readonly IRideService _rideService;
        
        public RideController(IRideService rideService)
        {
            _rideService = rideService;
        }

        [Authorize]
        [HttpGet("history")]
        public async Task<IActionResult> GetRideHistory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userType = User.FindFirstValue("userType");

            if (string.IsNullOrEmpty(userType))
                return Unauthorized("User type not found");

            var history = await _rideService.GetRideHistoryAsync(userId, userType);

            return Ok(history);
        }
    }
}
