using System.ComponentModel.DataAnnotations;

namespace KosDrive.Models
{
    public enum VehicleType
    {
        Car, 
        Suv,
        Truck
    }

    public class Vehicle
    {
        [Key] public Guid Id { get; set; }
        public string? LicensePlate { get; set; }
        public string? Model { get; set; }
        public VehicleType Type { get; set; }

        public Guid? CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        public Guid? DriverId { get; set; }
        public User? AssignedDriver { get; set; }
    }
}
