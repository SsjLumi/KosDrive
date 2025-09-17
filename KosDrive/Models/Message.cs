namespace KosDrive.Models
{
    public class Message
    {
        public int Id { get; set; }
        public Conversation Conversation { get; set; }
        public string ConversationId { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }

        public bool Delivered { get; set; }
        public bool Seen { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? SeenAt { get; set; }
    }
}
