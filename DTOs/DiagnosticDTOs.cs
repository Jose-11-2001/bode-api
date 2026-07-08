using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  // Add this

namespace BODE.API.DTOs
{
    // ========== REQUESTS ==========

    public class AddDiagnosticCodeRequest
    {
        [Required]
        [MaxLength(10)]
        public string Code { get; set; } = string.Empty;
        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
        public string? System { get; set; }
    }

    public class ScanOBDRequest
    {
        public int VehicleId { get; set; }
    }

    // ========== RESPONSES ==========

    public class DiagnosticCodeResponse
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? System { get; set; }
        public bool IsActive { get; set; }
        public DateTime DetectedAt { get; set; }
        public DateTime? ClearedAt { get; set; }
    }

    public class OBDScanResponse
    {
        public int VehicleId { get; set; }
        public List<DiagnosticCodeResponse> Codes { get; set; } = new();
        public int CodeCount { get; set; }
        public string? ScanSummary { get; set; }
    }

    public class CodeInterpretationResponse
    {
        public string Code { get; set; } = string.Empty;
        public string Interpretation { get; set; } = string.Empty;
        public string System { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public List<string> RecommendedActions { get; set; } = new();
    }
}