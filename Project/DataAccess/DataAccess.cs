using System.Text.Json;

static class DataAccess<T>
{
    static string basePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources"));

    public static List<T> LoadAll(string fileName)
    {
        string path = System.IO.Path.Combine(basePath, $"{fileName}.json");
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<T>>(json);
    }

    public static void WriteAll(List<T> items, string fileName)
    {
        string path = System.IO.Path.Combine(basePath, $"{fileName}.json");
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(items, options);
        File.WriteAllText(path, json);
    }
}