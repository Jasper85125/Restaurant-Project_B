using System.Text.Json.Serialization;
public class ReservationModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("checkInStop")]
    public string Stop { get; set; } // moet DateTime worden!

    [JsonPropertyName("routeName")]
    public string RouteName { get; set; }

    [JsonPropertyName("busLicensePlate")]
    public string BusLicensePlate { get; set; }

    [JsonPropertyName("seatRow")]
    public List<int> SeatRow { get; set; }

    [JsonPropertyName("seatCol")]
    public List<int> SeatCol { get; set; }
    
    public ReservationModel(int id, string stop, string routeName, string busLicensePlate)
    {
        Id = id;
        Stop = stop;
        RouteName = routeName;
        BusLicensePlate = busLicensePlate;
        SeatRow = new ();
        SeatCol = new ();
    }

    public void AddSeatRow (int row)
    {
        SeatRow.Add(row);
    }

    public void AddSeatCol (int col)
    {
        SeatCol.Add(col);
    }
}