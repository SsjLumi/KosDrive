namespace KosDrive.Models
{
    public class WithdrawalRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User User { get; set; }
        public decimal Amount { get; set; }
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";
    }
}
