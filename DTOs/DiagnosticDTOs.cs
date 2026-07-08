// ========== REQUESTS ==========

public class AddDiagnosticCodeRequest {
    [Required, MaxLength(10)]
    public string Code { get; set; }
    [Required, MaxLength(200)]
    public string Description { get; set; }
    public string System { get; set; }
}

public class ScanOBDRequest {
    public int VehicleId { get; set; }
}

// ========== RESPONSES ==========

public class DiagnosticCodeResponse {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string System { get; set; }
    public bool IsActive { get; set; }
    public DateTime DetectedAt { get; set; }
    public DateTime? ClearedAt { get; set; }
}

public class OBDScanResponse {
    public int VehicleId { get; set; }
    public List<DiagnosticCodeResponse> Codes { get; set; }
    public int CodeCount { get; set; }
    public string ScanSummary { get; set; }
}

public class CodeInterpretationResponse {
    public string Code { get; set; }
    public string Interpretation { get; set; }
    public string System { get; set; }
    public string Severity { get; set; } // Critical, Major, Minor
    public List<string> RecommendedActions { get; set; }
}