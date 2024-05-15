using System.Text.Json.Serialization;

public class BusModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("seats")]
    public int Seats { get; set; }

    [JsonPropertyName("licensePlate")]
    public string LicensePlate { get; set; }

    [JsonPropertyName("route")]
    public List<RouteModel> Route { get; set; }

    public BusModel(int id, int seats, string licensePlate)
    {
        Id = id;
        Seats = seats;
        LicensePlate = licensePlate;
        Route = new List<RouteModel>{};
    }

    public void AddRoute(RouteModel route)
    {
        Route.Add(route);
    }
}