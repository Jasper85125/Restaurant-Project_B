using System.Text.Json;

static class BussesAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/busses.json"));


    public static List<BusModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<BusModel>>(json);
    }


    public static void WriteAll(List<BusModel> busses)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(busses, options);
        File.WriteAllText(path, json);
    }

}
