using System.Text.Json.Serialization;

public class SeatModel //: IActivatable
{
    // [JsonPropertyName("id")]
    // public int Id { get; set; }

    [JsonPropertyName("isOccupied")]
    public bool IsOccupied { get; set; }

    // [JsonPropertyName("isActive")]
    // public bool IsActive { get; set; }

    public SeatModel(/*int id,*/ bool isOccupied = false/*, bool isActive = true*/)
    {   
        // this.Id = id;
        this.IsOccupied = isOccupied;
        // this.IsActive = isActive;
    }
}

