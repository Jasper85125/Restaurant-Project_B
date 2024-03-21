using System.Text.Json.Serialization;


class BusModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("seats")]
    public string Seats { get; set; }

    public BusModel(int id, string seats)
    {
        Id = id;
        Seats = seats;
    }

}




