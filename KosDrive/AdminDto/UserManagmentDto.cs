using KosDrive.Models;

namespace KosDrive.AdminDto
{
    public class UserManagmentDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserType UserType { get; set; }
        public bool IsBlocked { get; set; }
        public bool? IsVerified { get; set; }
        public double Rating { get; set; }
        public int NumberOfRatings { get; set; }

        public string? DriversLicensePicture { get; set; }
        public string? FrontCarLicensePlatePicture { get; set; }
        public string? BackCarLicensePlatePicture { get; set; }

        public string? IdentificationCardPicture { get; set; }

        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
