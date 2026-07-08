public class PagedResult<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public IEnumerable<T> Items { get; set; }
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}

public class QuotationResponse
{
    public int ServiceOrderId { get; set; }
    public string OrderNumber { get; set; }
    public CustomerSummary Customer { get; set; }
    public VehicleSummary Vehicle { get; set; }
    public List<QuotationItem> Items { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal GrandTotal { get; set; }
    public DateTime GeneratedAt { get; set; }
}

public class CustomerSummary
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}

public class VehicleSummary
{
    public int Id { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string LicensePlate { get; set; }
}

public class StatusHistoryResponse
{
    public int Id { get; set; }
    public ServiceStatus FromStatus { get; set; }
    public ServiceStatus ToStatus { get; set; }
    public DateTime ChangedAt { get; set; }
    public string ChangedBy { get; set; }
    public string Notes { get; set; }
}

public class DiagnosticCodeReference
{
    public string Code { get; set; }
    public string Description { get; set; }
    public string System { get; set; }
    public string Severity { get; set; }
    public string CommonCause { get; set; }
}

public class DiagnosticStatsResponse
{
    public int TotalCodesFound { get; set; }
    public int ActiveCodes { get; set; }
    public int ResolvedCodes { get; set; }
    public Dictionary<string, int> CodesBySystem { get; set; }
    public List<string> MostCommonCodes { get; set; }
}

public class MaintenanceScheduleResponse
{
    public int VehicleId { get; set; }
    public int CurrentMileage { get; set; }
    public List<MaintenanceItem> UpcomingMaintenance { get; set; }
    public List<MaintenanceItem> OverdueMaintenance { get; set; }
}

public class MaintenanceItem
{
    public string Service { get; set; }
    public int IntervalMiles { get; set; }
    public int NextDueAt { get; set; }
    public int? OverdueBy { get; set; }
    public string Priority { get; set; }
}

public class CustomerStatsResponse
{
    public int TotalVehicles { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal AverageOrderValue { get; set; }
    public DateTime? LastVisit { get; set; }
    public string MostCommonService { get; set; }
}