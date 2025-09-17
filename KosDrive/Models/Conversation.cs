namespace KosDrive.Models
{
    public class Conversation
    {
        public int Id { get; set; }
        public string RiderId { get; set; }
        public string DriverId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Message> Messages { get; set; }
    }
}
