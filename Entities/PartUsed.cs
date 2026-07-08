public class PartUsed {
    public int Id { get; set; }
    public int ServiceOrderId { get; set; }
    public ServiceOrder ServiceOrder { get; set; }
    public string PartName { get; set; }
    public string PartNumber { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string Supplier { get; set; }
}