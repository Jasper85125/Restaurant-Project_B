using System.Text.Json.Serialization;


public class StopModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("time")]
    public string Time { get; set; }

    public StopModel(string name)
    {
        Name = name;
        Time = null;
    }
}