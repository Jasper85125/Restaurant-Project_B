using System.Text.Json.Serialization;


class PriceModel
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("passenger")]
    public string Passenger { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }

    public PriceModel(int id, string passenger, double price)
    {
        ID = id;
        Passenger = passenger;
        Price = price;
    }

}


