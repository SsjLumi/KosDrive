namespace KosDrive.DTOs
{
    public class RideRequestDto
    {
        public string RiderId { get; set; }
        public string DriverId { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
    }
}
