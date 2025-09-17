namespace KosDrive.AdminDto
{
    public class LiveRideDto
    {
        public Guid RideId { get; set; }
        public Guid DriverId { get; set; }
        public Guid RiderId { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public string RiderName { get; set; } = string.Empty;
        public double? PickupLat { get; set; }
        public double? PickupLng { get; set; }
        public double? DestinationLat { get; set; }
        public double? DestinationLng { get; set; }
        public double? DriverLat { get; set; }
        public double? DriverLng { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? StartedAt { get; set; }
    }
}
