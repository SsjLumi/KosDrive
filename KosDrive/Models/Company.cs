using System.ComponentModel.DataAnnotations;

namespace KosDrive.Models
{

    public enum SubscriptionTier
    {
        Basic,
        Standart,
        Premium
    }

    public class Company
    {
        [Key] public Guid Id { get; set; }
        public string? CompanyName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? LogoUrl { get; set; }
        public int RegistrationNumber { get; set; }

        public string? BusinessLicensePicture { get; set; }
        public string? RepresentantiveIdPicture { get; set; }
        public string? CompanyLogoPicture { get; set; }


        public SubscriptionTier Subscription { get; set; }
        public int MaxAllowedVehicles { get; set; }
        public DateTime SubscriptionExpiresAt { get; set; }

        public bool IsApproved { get; set; } = false;

        public ICollection<User>? Drivers { get; set; }
        public ICollection<Vehicle>? Vehicles { get; set; }
    }
}
