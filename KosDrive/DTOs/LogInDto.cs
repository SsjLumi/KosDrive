using System.ComponentModel.DataAnnotations;

namespace KosDrive.DTOs
{
    public class LogInDto
    {
        [Required(ErrorMessage = "Email required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
