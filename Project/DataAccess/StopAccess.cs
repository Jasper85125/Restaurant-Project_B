using System.Text.Json;

static class StopAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/stops.json"));


    public static List<StopModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<StopModel>>(json);
    }


    public static void WriteAll(List<StopModel> stops)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(stops, options);
        File.WriteAllText(path, json);
    }
}