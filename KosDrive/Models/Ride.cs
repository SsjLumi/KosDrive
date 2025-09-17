using System.ComponentModel.DataAnnotations;

namespace KosDrive.Models
{
    public enum RideStatus
    {
        Requested,
        Pending,
        Accepted,
        EnRoute,
        InProgress,
        Completed,
        CancelledByRider,
        CancelledByDriver,
        NoShow
    }

    public class Ride
    {
        [Key] public Guid Id { get; set; }

        public Guid RiderId { get; set; }
        public User? Rider { get; set; }

        public Guid DriverId { get; set; }
        public User? Driver { get; set; }
        
        public string City { get; set; }
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public double DestinationLatitude { get; set; }
        public double DestinationLongitude { get; set; }

        public string? PickupAddress { get; set; }
        public string? DestinationAddress { get; set; }

        public RideStatus Status { get; set; } = RideStatus.Requested;

        public DateTime RequestedAt { get; set; } = DateTime.Now;
        public DateTime? AcceptedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? CancelledAt { get; set; }

        public string PaymentMethod { get; set; }
        public decimal Price { get; set; }

        public int? RiderRating { get; set; }
        public int? RiderReview { get; set; }
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        public int? DriverRating { get; set; }
        public string? DriverReview { get; set; }

        public Guid? VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
    }
}
