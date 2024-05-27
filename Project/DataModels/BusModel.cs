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
    public bool IsActive { get; set; }

    [JsonPropertyName("seatingMap")]
    public SeatModel[,] SeatingMap { get; set; }

    public BusModel(int id, int seats, string licensePlate, bool isActive = true)// SeatModel[,] seatingMap = null)
    {
        Id = id;
        Seats = seats;
        LicensePlate = licensePlate;
        Route = new List<RouteModel>{};
        IsActive = isActive;
        //SeatingMap = seatingMap;
    }

    public void AddRoute(RouteModel route)
    {
        Route.Add(route);
    }
}
