using KosDrive.DTOs;
using KosDrive.Services;
using Microsoft.AspNetCore.Mvc;

namespace KosDrive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RideRequestController : ControllerBase
    {
        private readonly RideRequestService _rideService;

        public RideRequestController(RideRequestService rideService)
        {
            _rideService = rideService;
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookDriver([FromBody] RideRequestDto dto)
        {
            var requestId = await _rideService.CreateRideRequestAsync(dto);
            return Ok(new { RequestId = requestId });
        }

        [HttpPost("accept/{id}")]
        public async Task<IActionResult> AcceptRide(int id)
        {
            await _rideService.AcceptRequestAsync(id);
            return Ok();
        }

        [HttpPost("decline/{id}")]
        public async Task<IActionResult> DeclineRide(int id)
        {
            await _rideService.DeclineRequestAsync(id);
            return Ok();
        }
    }
}
