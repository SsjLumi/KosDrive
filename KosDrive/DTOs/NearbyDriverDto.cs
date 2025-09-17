namespace KosDrive.DTOs
{
    public class NearbyDriverDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double DistanceKm { get; set; }
    }
}
