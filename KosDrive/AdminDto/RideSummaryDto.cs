namespace KosDrive.AdminDto
{
    public class RideSummaryDto
    {
        public Guid Id { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public string RiderName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

    }
}
