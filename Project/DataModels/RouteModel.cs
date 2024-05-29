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
    public   Dictionary<StopModel, DateTime> Stops { get; set; }

    [JsonPropertyName("begin")]
    public string beginTime { get; set; }

    [JsonPropertyName("end")]
    public string endTime { get; set; }

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
    public RouteModel(int id, int duration, string name, bool isActive = true)
    {
        Id = id;
        Duration = duration;
        Name = name;
        Stops = new Dictionary<StopModel, DateTime>();
        beginTime = null;
        endTime = null;
        IsActive = isActive;
    }

    public void AddHalte(StopModel halte, DateTime halteTime)
    {
        if (Stops.ContainsKey(halte))
        {
            Stops[halte] = halteTime;
        }
        else
        {
            Stops.Add(halte, halteTime);
        }
    }
}