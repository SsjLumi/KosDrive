namespace KosDrive.Models
{
    public class AuditLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        public string Action { get; set; }
        public string Resource { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string IPAddress { get; set; }
    }
}
