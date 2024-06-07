using System.Text.Json.Serialization;


public class StopModel : IEquatable<StopModel>
{
    [JsonPropertyName("id")]
    public int Id {get; set;}

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("time")]
    public TimeSpan? Time { get; set; } //Moet Datetime worden!

    public StopModel(int id, string name, TimeSpan? time = null)
    {
        Id = id;
        Name = name;
        Time = time;
    }

    public bool Equals(StopModel? stopModel)
    {
        if (stopModel == null) return false;
        return this.Id == stopModel.Id && this.Name == stopModel.Name && this.Time == stopModel.Time;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj is StopModel stopModel) return this.Equals(stopModel);
        return false;
    }

    public static bool operator ==(StopModel s1, StopModel s2)
    {
        if (s1 is null) return s2 is null;
        return s1.Equals(s2);
    }

    public static bool operator !=(StopModel s1, StopModel s2) => !(s1 == s2);
}