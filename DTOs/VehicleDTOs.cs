public class VehicleRequest {
    [Required]
    public int CustomerId { get; set; }
    [Required, MaxLength(50)]
    public string Make { get; set; }
    [Required, MaxLength(50)]
    public string Model { get; set; }
    [Required]
    public int Year { get; set; }
    [Required, MaxLength(20)]
    public string LicensePlate { get; set; }
    public string VIN { get; set; }
    public string EngineType { get; set; }
    public int Mileage { get; set; }
}

public class VehicleResponse {
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string LicensePlate { get; set; }
    public string VIN { get; set; }
    public string EngineType { get; set; }
    public int Mileage { get; set; }
    public string FullName => $"{Year} {Make} {Model}";
    public int ActiveOrders { get; set; }
}