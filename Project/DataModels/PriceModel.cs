using System.Text.Json.Serialization;


public class PriceModel : IActivatable
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("passenger")]
    public string Passenger { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; } = false;

    public PriceModel(int id, string passenger, double price, bool isActive)
    {
        this.Id = id;
        this.Passenger = passenger;
        this.Price = price;
        this.IsActive = isActive;
    }
}


