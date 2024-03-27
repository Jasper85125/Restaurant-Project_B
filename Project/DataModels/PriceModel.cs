using System.Text.Json.Serialization;


class PriceModel
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("passenger")]
    public string Passenger { get; set; }

    [JsonPropertyName("price")]
    public int Price { get; set; }

    public PriceModel(int id, string passenger, int price)
    {
        ID = id;
        Passenger = passenger;
        Price = price;
    }

}


