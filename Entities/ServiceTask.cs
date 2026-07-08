public class ServiceTask {
    public int Id { get; set; }
    public int ServiceOrderId { get; set; }
    public ServiceOrder ServiceOrder { get; set; }
    public ServiceCategory Category { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal EstimatedLaborHours { get; set; }
    public decimal ActualLaborHours { get; set; }
    public decimal PartsCost { get; set; }
    public decimal LaborRate { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}