using System.Text.Json;

static class DataAccess<T>
{
    static string basePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources"));

    public static List<T> LoadAll(string fileName)
    {
        Console.WriteLine(typeof(T).Name);
        string path = System.IO.Path.Combine(basePath, $"{fileName}.json");
        List<T> ListToReturn = null;
        try
        {
            string json = File.ReadAllText(path);
            ListToReturn = JsonSerializer.Deserialize<List<T>>(json);
            return ListToReturn;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return ListToReturn;
    }

    public static void WriteAll(List<T> items, string fileName)
    {
        try
        {
            string path = System.IO.Path.Combine(basePath, $"{fileName}.json");
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(items, options);
            File.WriteAllText(path, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}