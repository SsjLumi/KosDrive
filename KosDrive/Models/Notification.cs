using System.ComponentModel.DataAnnotations;

namespace KosDrive.Models
{
    public enum NotificationTarget
    {
        AllDrivers,
        AllRiders,
        SpecificUser
    }

    public class Notification
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationTarget Target { get; set; }
        public Guid? TargetUserId { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
