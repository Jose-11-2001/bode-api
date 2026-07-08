using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  // Add this for validation attributes

namespace BODE.API.DTOs
{
    // ========== REQUESTS ==========

    public class CreateServiceOrderRequest
    {
        [Required]
        public int VehicleId { get; set; }
        public string? CustomerNotes { get; set; }
        public List<ServiceTaskRequest>? InitialTasks { get; set; }
    }

    public class ServiceTaskRequest
    {
        [Required]
        public ServiceCategory Category { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Range(0, 100)]
        public decimal EstimatedLaborHours { get; set; }
        [Range(0, 10000)]
        public decimal PartsCost { get; set; }
    }

    public class UpdateStatusRequest
    {
        [Required]
        public ServiceStatus Status { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateTaskStatusRequest
    {
        [Required]
        public TaskStatus Status { get; set; }
        public decimal ActualLaborHours { get; set; }
    }

    public class AddPartRequest
    {
        [Required]
        public string PartName { get; set; } = string.Empty;
        public string? PartNumber { get; set; }
        [Range(1, 100)]
        public int Quantity { get; set; }
        [Range(0, 10000)]
        public decimal UnitPrice { get; set; }
        public string? Supplier { get; set; }
    }

    // ========== RESPONSES ==========

    public class ServiceOrderResponse
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string VehicleInfo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public decimal GrandTotal { get; set; }
        public List<DiagnosticCodeResponse>? DiagnosticCodes { get; set; }
        public List<ServiceTaskResponse>? Tasks { get; set; }
        public List<PartUsedResponse>? PartsUsed { get; set; }
        public string? CustomerNotes { get; set; }
        public string? MechanicNotes { get; set; }
    }

    public class ServiceTaskResponse
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal EstimatedLaborHours { get; set; }
        public decimal ActualLaborHours { get; set; }
        public decimal PartsCost { get; set; }
        public decimal LaborRate { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public class PartUsedResponse
    {
        public int Id { get; set; }
        public string PartName { get; set; } = string.Empty;
        public string? PartNumber { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Supplier { get; set; }
    }
}