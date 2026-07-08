public class Vehicle {
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string LicensePlate { get; set; }
    public string VIN { get; set; }
    public string EngineType { get; set; }
    public int Mileage { get; set; }
    public ICollection<ServiceOrder> ServiceOrders { get; set; }
}