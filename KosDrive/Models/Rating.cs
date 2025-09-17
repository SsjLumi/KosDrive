namespace KosDrive.Models
{
    public class Rating
    {
        public Guid Id { get; set; }

        public Guid FromUserId { get; set; }
        public User FromUser { get; set; } = null!;

        public Guid ToUserId { get; set; }
        public User ToUser { get; set; } = null!;

        public Guid RideId { get; set; }
        public Ride Ride { get; set; } = null!;

        public int Stars { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
