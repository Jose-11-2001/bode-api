public class DiagnosticCode {
    public int Id { get; set; }
    public int ServiceOrderId { get; set; }
    public ServiceOrder ServiceOrder { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string System { get; set; }
    public bool IsActive { get; set; }
    public DateTime DetectedAt { get; set; }
    public DateTime? ClearedAt { get; set; }
}