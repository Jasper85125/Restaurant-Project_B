using System.Text.Json.Serialization;


class PriceModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("minage")]
    public int MinAge { get; set; }

    [JsonPropertyName("maxage")]
    public int MaxAge { get; set; }

    [JsonPropertyName("price")]
    public int Price { get; set; }

    public PriceModel(int id, int minAge, int maxAge, int price)
    {
        Id = id;
        MinAge = minAge;
        MaxAge = maxAge;
        Price = price;
    }

}




