using System.Text.Json.Serialization;


public class RoutesModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    public RoutesModel(int id, int duration)
    {
        Id = id;
        Duration = duration;
    }

}