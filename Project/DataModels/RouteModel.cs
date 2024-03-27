using System.Text.Json.Serialization;


public class RouteModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("stops")]
    public List<int> Stops { get; set; }

    public RouteModel(int id, int duration)
    {
        Id = id;
        Duration = duration;
        Stops = new List<int>{};
    }
}