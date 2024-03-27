using System.Text.Json.Serialization;


public class StopModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    public StopModel(int id, string name)
    {
        Id = id;
        Name = name;
    }
}