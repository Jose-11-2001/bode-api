public class ServiceOrder {
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
    public string OrderNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DiagnosedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public ServiceStatus Status { get; set; }
    public string CustomerNotes { get; set; }
    public string MechanicNotes { get; set; }
    public int? AssignedTo { get; set; }
    public decimal TotalLaborCost { get; set; }
    public decimal TotalPartsCost { get; set; }
    public decimal Tax { get; set; }
    public decimal GrandTotal { get; set; }
    public ICollection<ServiceTask> Tasks { get; set; }
    public ICollection<DiagnosticCode> DiagnosticCodes { get; set; }
    public ICollection<PartUsed> PartsUsed { get; set; }
}