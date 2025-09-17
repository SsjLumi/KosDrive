using KosDrive.Data;
using KosDrive.DTOs;
using KosDrive.Models;
using KosDrive.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace KosDrive.Controllers
{
    [ApiController]
    [Route("api/admin/notifications")]
    public class AdminNotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public AdminNotificationsController(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationDto dto)
        {
            if (dto.Target == Models.NotificationTarget.SpecificUser && dto.TargetUserId == null)
                return BadRequest("TargetUserId is required for SpecificUser notifications");

            var notification = new Notification
            {
                Title = dto.Title,
                Message = dto.Message,
                Target = dto.Target,
                TargetUserId = dto.TargetUserId
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            switch (dto.Target)
            {
                case NotificationTarget.AllDrivers:
                    await _hubContext.Clients.Group("drivers").SendAsync("ReceiveNotification", dto.Title, dto.Message);
                    break;
                case NotificationTarget.AllRiders:
                    await _hubContext.Clients.Group("riders").SendAsync("ReceiveNotification", dto.Title, dto.Message);
                    break;
                case NotificationTarget.SpecificUser:
                    await _hubContext.Clients.User(dto.TargetUserId!.ToString()).SendAsync("ReceiverNotification", dto.Title, dto.Message);
                    break;
            }
            return Ok(new { message = "Notification sent." });
        }
    }
}
