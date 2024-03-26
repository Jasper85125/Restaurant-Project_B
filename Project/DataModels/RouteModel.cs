using System.Text.Json.Serialization;


public class RouteModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    public RouteModel(int id, int duration)
    {
        Id = id;
        Duration = duration;
    }

}