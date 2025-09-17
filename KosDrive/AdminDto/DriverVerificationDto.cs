namespace KosDrive.AdminDto
{
    public class DriverVerificationDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? DriversLicensePicture { get; set; }
        public string? FrontCarLicensePlatePicture { get; set; }
        public string? BackCarLicensePlatePicture { get; set; }
        public bool? IsVerified { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
