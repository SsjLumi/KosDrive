using KosDrive.DTOs;
using KosDrive.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KosDrive.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuditLoggerService _auditService;

        public UserController(IUserService userService, IAuditLoggerService auditService)
        {
            _userService = userService;
            _auditService = auditService;

        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
                return Unauthorized();

            var result = await _userService.UpdateUserProfileAsync(userId, dto);
            if(!result.Successeded)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpPost("delete-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteAsync(id);

            if (result.Successeded)
                await _auditService.LogAsync(User.Identity?.Name, $"Delete user with Id {id}", null);

            return Ok(result.Data);
        }
    }
}
