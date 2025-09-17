using KosDrive.DTOs;
using KosDrive.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KosDrive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DisputesController : Controller
    {
        private readonly IDisputeService _disputeService;

        public DisputesController(IDisputeService disputeService)
        {
            _disputeService = disputeService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateDispute([FromBody] CreateDisputeDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _disputeService.CreateDisputeAsync(userId, dto);

            if (!result.Successeded)
                return BadRequest(result.Message);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetDisputes()
        {
            var disputes = await _disputeService.GetAllDisputesAsync();
            return Ok(disputes);
        }
    }
}
