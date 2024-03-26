using System.Text.Json;

static class PricesAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/prices.json"));


    public static List<PriceModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<PriceModel>>(json);
    }


    public static void WriteAll(List<PriceModel> prices)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(prices, options);
        File.WriteAllText(path, json);
    }

}
