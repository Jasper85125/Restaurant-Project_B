using System.Text.Json.Serialization;


public class RouteModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("stops")]
    public List<StopModel> Stops { get; set; }

    [JsonPropertyName("begin")]
    public string beginTime { get; set; }

    [JsonPropertyName("end")]
    public string endTime { get; set; }

    public RouteModel(int id, int duration, string name)
    {
        Id = id;
        Duration = duration;
        Name = name;
        Stops = new List<StopModel>{};
        beginTime = null;
        endTime = null;
    }

    public void AddStop(StopModel stop)
    {
        Stops.Add(stop);
    }
}