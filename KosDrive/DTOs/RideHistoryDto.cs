namespace KosDrive.DTOs
{
    public class RideHistoryDto
    {
        public Guid RideId { get; set; }
        public DateTime RequestedAt { get; set; }
        public string Status { get; set; }
        
        public string PickupAddress { get; set; }
        public string DestinationAddress { get; set; }

        public string? RiderName { get; set; }
        public string? DriverName { get; set; }

        public decimal? Price { get; set; }
    }
}
