namespace KosDrive.Models
{
    public class FavoriteDriver
    {
        public Guid RiderId { get; set; }
        public User? Rider { get; set; }

        public Guid DriverId { get; set; }
        public User? Driver { get; set; }

        public DateTime FavoritedAt { get; set; } = DateTime.UtcNow;
        public string FavoritedBy { get; set; }
    }
}
