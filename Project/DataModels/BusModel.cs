using System.Text.Json.Serialization;

public class BusModel : IActivatable, IEquatable<BusModel>
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("seats")]
    public string Seats { get; set; }

    [JsonPropertyName("licensePlate")]
    public string LicensePlate { get; set; }

    [JsonPropertyName("route")]
    public List<RouteModel> Route { get; set; }

    [JsonPropertyName("IsActive")]
    public bool IsActive { get; set; }

    [JsonPropertyName("seatingMap")]
    [JsonConverter(typeof(ValueTupleKeyConverter<(int, int), SeatModel>))]
    public Dictionary<(int Row, int Col), SeatModel> SeatingMap { get; set; } = new();

    public BusModel(int id, string seats, string licensePlate, bool isActive = false)
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

    public bool Equals(BusModel? busModel)
    {
        if (busModel == null) return false;
        return this.Id == busModel.Id && this.LicensePlate == busModel.LicensePlate && this.Route.Count == busModel.Route.Count && this.IsActive == busModel.IsActive && this.SeatingMap.Count == busModel.SeatingMap.Count;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj is BusModel busModel) return this.Equals(busModel);
        return false;
    }

    public static bool operator ==(BusModel b1, BusModel b2)
    {
        if (b1 is null) return b2 is null;
        return b1.Equals(b2);
    }

    public static bool operator !=(BusModel b1, BusModel b2) => !(b1 == b2);
}