using System.Text.Json;

static class RoutesAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/Routes.json"));


    public static List<RoutesModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<RoutesModel>>(json);
    }


    public static void WriteAll(List<RoutesModel> routes)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(routes, options);
        File.WriteAllText(path, json);
    }



}