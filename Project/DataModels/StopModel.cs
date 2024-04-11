using System.Text.Json.Serialization;


public class StopModel
{
    public string Name { get; set; }

    public string Time { get; set; }

    public StopModel(string name)
    {
        Name = name;
        Time = null;
    }
}