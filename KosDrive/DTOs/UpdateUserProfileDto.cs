using System.ComponentModel.DataAnnotations;

namespace KosDrive.DTOs
{
    public class UpdateUserProfileDto
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Phone] public string Phone { get; set; }
        [EmailAddress] public string Email { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }
}
