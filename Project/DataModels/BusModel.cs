using System.Text.Json.Serialization;

public class BusModel : IActivatable
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("seats")]
    public int Seats { get; set; }

    [JsonPropertyName("licensePlate")]
    public string LicensePlate { get; set; }

    [JsonPropertyName("route")]
    public List<RouteModel> Route { get; set; }

    [JsonPropertyName("IsActive")]
    public bool IsActive { get; set; } = false;

    public BusModel(int id, int seats, string licensePlate, bool isActive = true)
    {
        Id = id;
        Seats = seats;
        LicensePlate = licensePlate;
        Route = new List<RouteModel>{};
        IsActive = isActive;
    }

    public void AddRoute(RouteModel route)
    {
        Route.Add(route);
    }
}
