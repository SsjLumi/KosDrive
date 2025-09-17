using KosDrive.AdminDto;
using KosDrive.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KosDrive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly IPlatformSettingService _service;

        public SettingsController(IPlatformSettingService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSettings()
        {
            var result = await _service.GetSettingsAsync();
            return result.Successeded ? Ok(result) : StatusCode(result.Status, result);
        }

        [HttpPost("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSettings([FromBody] UpdatePlatformSettingDto dto)
        {
            var result = await _service.UpdateSettingsAsync(dto);
            return result.Successeded ? Ok(result) : StatusCode(result.Status, result);
        }
    }
}
