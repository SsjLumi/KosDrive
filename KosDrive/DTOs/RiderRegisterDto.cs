using System.ComponentModel.DataAnnotations;

namespace KosDrive.DTOs
{
    public class RiderRegisterDto
    {
        [Required(ErrorMessage = "Username should be added")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "First Name should be added")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name should be added")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email should be added")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number should be added")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Birthdate should be added")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Password should be added")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password should be added and match with the Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
