using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  // Add this

namespace BODE.API.DTOs
{
    public class CustomerRequest
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }

    public class CustomerResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public int VehicleCount { get; set; }
        public List<VehicleResponse>? Vehicles { get; set; }
    }

    public class CustomerStatsResponse
    {
        public int TotalVehicles { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal AverageOrderValue { get; set; }
        public DateTime? LastVisit { get; set; }
        public string? MostCommonService { get; set; }
    }
}