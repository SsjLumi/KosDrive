namespace KosDrive.AnalyticsDto
{
    public class DriverRideCountDto
    {
        public Guid DriverId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RideCount { get; set; }
        public int CancellationCount { get; set; }
    }
}
