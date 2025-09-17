namespace KosDrive.Models
{
    public class PlatformSetting
    {
        public Guid Id { get; set; }

        public decimal BaseFare { get; set; }
        public decimal PricePerKm { get; set; }
        public decimal PricePerMinute { get; set; }
        public decimal SurgeMultiplier { get; set; }

        public double MinimumDriverRating { get; set; }
        public bool IsCashPaymentEnabled { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
