using System.ComponentModel.DataAnnotations;

namespace KosDrive.DTOs
{
    public class CreateDisputeDto
    {
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid? RideId { get; set; }
    }
}
