using System.Text.Json.Serialization;


public class RouteModel : IActivatable
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
    public TimeSpan? beginTime { get; set; }

    [JsonPropertyName("end")]
    public TimeSpan? endTime { get; set; }

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
    public RouteModel(int id, int duration, string name, bool isActive = true)
    {
        Id = id;
        Duration = duration;
        Name = name;
        Stops = new List<StopModel>{};
        beginTime = Stops.Where(stop => stop.Time.HasValue).Min(stop => stop.Time);
        endTime = Stops.Where(stop => stop.Time.HasValue).Max(stop => stop.Time);
        IsActive = isActive;
    }

    public void AddStop(StopModel stop)
    {
        Stops.Add(stop);
    }
}