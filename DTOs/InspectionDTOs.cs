// ========== REQUESTS ==========

public class InspectionRequest {
    [Required]
    public int VehicleId { get; set; }
    public bool IncludeQuotation { get; set; } = true;
}

// ========== RESPONSES ==========

public class InspectionReportResponse {
    public int VehicleId { get; set; }
    public string VehicleInfo { get; set; }
    public DateTime InspectedAt { get; set; }
    public int TotalItemsInspected { get; set; }
    public int CriticalIssues { get; set; }
    public int UrgentIssues { get; set; }
    public int RoutineIssues { get; set; }
    public List<InspectionItemResponse> Items { get; set; }
    public QuotationSummary Quotation { get; set; }
}

public class InspectionItemResponse {
    public string Component { get; set; }
    public string Condition { get; set; } // Good, Worn, Damaged, Failed
    public string Details { get; set; }
    public string Recommendation { get; set; }
    public Priority Priority { get; set; }
    public decimal EstimatedCost { get; set; }
}

public class QuotationSummary {
    public decimal TotalParts { get; set; }
    public decimal TotalLabor { get; set; }
    public decimal Tax { get; set; }
    public decimal GrandTotal { get; set; }
    public List<QuotationItem> Items { get; set; }
}

public class QuotationItem {
    public string Description { get; set; }
    public decimal PartsCost { get; set; }
    public decimal LaborCost { get; set; }
    public decimal Total { get; set; }
}