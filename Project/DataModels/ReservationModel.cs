using System.Text.Json.Serialization;
public class ReservationModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("time")]
    public string Time { get; set; } // moet DateTime worden!

    [JsonPropertyName("halte")]
    public StopModel Stop { get; set; }
    
    public ReservationModel(int id, string time, StopModel stop = null)
    {
        Id = id;
        Time = time;
        Stop = stop;
    }
}