using System.Text.Json;

static class DataAccess<T>
{
    static string path = System.IO.Path.GetFullPath
    (System.IO.Path.Combine(Environment.CurrentDirectory, $@"DataSources/{RemoveSuffix(typeof(T).Name)}.json"));

    public static List<T> LoadAll()
    {
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

    public static void WriteAll(List<T> items)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(items, options);
            File.WriteAllText(path, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static string RemoveSuffix(string classModel, string suffix="Model")
    {
        return classModel.Replace(suffix, "");
    }
}