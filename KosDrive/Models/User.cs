using Microsoft.AspNetCore.Identity;

namespace KosDrive.Models
{
    public enum UserType
    {
        Admin,
        Rider, 
        Driver,
        CompanyAdmin
    }

    public class User : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? PhoneNumber { get; set; }

        public UserType UserType { get; set; }

        public double Rating { get; set; }
        public int NumberOfRatings { get; set; }

        public double? CurrentLatitude { get; set; }
        public double? CurrentLongitude { get; set; }
        public bool IsAvailable { get; set; } = false;

        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastActivatedAt { get; set; }

        public ICollection<Ride>? RideHistory { get; set; }

        //Driver
        public string? DriversLicensePicture { get; set; }
        public string? FrontCarLicensePlatePicture { get; set; }
        public string? BackCarLicensePlatePicture { get; set; }

        public bool? IsVerified { get; set; }
        public bool? IsActive { get; set; }
        public DateTime DriversLicenseExpirationDate { get; set; }

        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        //Passenger
        public long Birthday { get; set; }
        public ICollection<FavoriteDriver>? FavoriteDrivers { get; set; }
        public string? PreferedPaymentMethod { get; set; }

        //CompanyAdmin
        public string? IdentificationCardPicture { get; set; }
    }
}
