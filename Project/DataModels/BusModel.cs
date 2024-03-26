using System.Text.Json.Serialization;

class BusModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("seats")]
    public int Seats { get; set; }

    public BusModel(int id, int seats)
    {
        Id = id;
        Seats = seats;
    }

}