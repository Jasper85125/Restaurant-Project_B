using System.Text.Json;

static class RouteAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/routes.json"));


    public static List<RouteModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<RouteModel>>(json);
    }


    public static void WriteAll(List<RouteModel> routes)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(routes, options);
        File.WriteAllText(path, json);
    }

}