using System.ComponentModel.DataAnnotations;

namespace KosDrive.Models
{
    public enum DisputeStatus
    {
        Pending,
        InReview,
        Resolved,
        Rejected
    }

    public class Dispute
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }

        public Guid? RideId { get; set; }
        public Ride? Ride { get; set; }

        public DisputeStatus Status { get; set; }
    }
}
