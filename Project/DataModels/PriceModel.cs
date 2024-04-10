using System.Text.Json.Serialization;


public class PriceModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("passenger")]
    public string Passenger { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }

    public PriceModel(int id, string passenger, double price)
    {
        this.Id = id;
        this.Passenger = passenger;
        this.Price = price;
    }
}


