namespace KosDrive.Models
{

    public class RideRequest
    {
        public int Id { get; set; }
        public string RiderId { get; set; }
        public string DriverId { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public RideStatus Status { get; set; }
        public DateTime RequestedAt { get; set; }
    }
}
