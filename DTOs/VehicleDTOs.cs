using System.ComponentModel.DataAnnotations;  // Add this

namespace BODE.API.DTOs
{
    public class VehicleRequest
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Make { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string Model { get; set; } = string.Empty;
        [Required]
        public int Year { get; set; }
        [Required]
        [MaxLength(20)]
        public string LicensePlate { get; set; } = string.Empty;
        public string? VIN { get; set; }
        public string? EngineType { get; set; }
        public int Mileage { get; set; }
    }

    public class VehicleResponse
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string? VIN { get; set; }
        public string? EngineType { get; set; }
        public int Mileage { get; set; }
        public string FullName => $"{Year} {Make} {Model}";
        public int ActiveOrders { get; set; }
    }
}