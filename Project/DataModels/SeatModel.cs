using System.Text.Json.Serialization;

public class SeatModel
{
    [JsonPropertyName("isOccupied")]
    public bool IsOccupied { get; set; }

    public SeatModel(bool isOccupied = false)
    {   
        this.IsOccupied = isOccupied;
    }
}