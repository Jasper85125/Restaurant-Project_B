using System.Text.Json.Serialization;

class BusModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("seats")]
    public int Seats { get; set; }

    [JsonPropertyName("licensePlate")]
    public string LicensePlate { get; set; }

    [JsonPropertyName("route")]
    public List<int> Route { get; set; }

    public BusModel(int id, int seats, string licensePlate)
    {
        Id = id;
        Seats = seats;
        LicensePlate = licensePlate;
        Route = new List<int>{};
    }

    public void AddRoute(int id)
    {
        Route.Add(id);
    }
}