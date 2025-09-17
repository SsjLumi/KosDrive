using KosDrive.Models;

namespace KosDrive.DTOs
{
    public class NotificationDto
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationTarget Target { get; set; }
        public Guid? TargetUserId { get; set; }
    }
}
