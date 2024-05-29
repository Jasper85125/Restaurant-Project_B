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
    [JsonConverter(typeof(ValueTupleKeyConverter<(int, int), SeatModel>))]
    public Dictionary<(int Row, int Col), SeatModel> SeatingMap { get; set; } = new();

    public BusModel(int id, int seats, string licensePlate, bool isActive = false)
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

    public void AddSeatingMap(Dictionary<(int Row, int Col), SeatModel> seatingMap)
    {
        SeatingMap = seatingMap;
    }
}
