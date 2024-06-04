using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Formats.Asn1;
using Microsoft.VisualBasic;

public static class CustomerReservation
{
    private static PriceLogic pricesLogic = new();
    private static BasicTableLogic<ReservationModel> tableReservations = new();
    private static BasicTableLogic<PriceModel> basictableLogic = new();
    static private AccountsLogic accountsLogic = new AccountsLogic();
    


    public static void Start()
    {
        ShowAllPricesInformation();
    }
    
    public static void ShowAllPricesInformation()
    {
        string title = "Uw reserveringen";
        List<string> header = new() {"Halte", "Route", "Zitplaats"};
        AccountModel currentAccount = UserLogin.loggedInAccount;
        List<ReservationModel> Reservations = currentAccount.Reservations;
        string kind = "reserveringen";
        if (Reservations == null || Reservations.Count == 0)
        {
            PriceModel newPriceModel = new(pricesLogic.GenerateNewId(),"",0,false);
                    pricesLogic.UpdateList(newPriceModel);
        }
        while(true)
        {
            int? SelectedRowIndex = tableReservations.PrintTable(header, Reservations, GenerateRow, title);
            if(SelectedRowIndex == null){
                CustomerStartMenu.Start();
                return;
            }
            else
            {
                List<string> selectedRow = GenerateRow(currentAccount.Reservations[SelectedRowIndex.Value]);
                if(SelectedRowIndex == Reservations.Count())
                {
                    PriceModel newPriceModel = new(pricesLogic.GenerateNewId(),"",0,false);
                    pricesLogic.UpdateList(newPriceModel);
                    continue;
                }
                while(true)
                {
                    selectedRow = GenerateRow(currentAccount.Reservations[SelectedRowIndex.Value]);
                    (string SelectedItem, int SelectedIndex)? result = tableReservations.PrintSelectedRow(selectedRow, header);
                    if (result == null){
                        break; //exit loop door escape
                    }
                    else
                    {
                        string selectedItem = result.Value.SelectedItem;
                        int selectedIndex = result.Value.SelectedIndex;

                        if (selectedIndex == 0)
                        {
                            ColorPrint.PrintRed($"U kan de {header[selectedIndex]} niet aanpassen.");
                            Thread.Sleep(3000);
                        }
                        
                    }
                }
            }
        }
        
    }


    public static List<string> GenerateRow(ReservationModel Reservations)
    {
        var checkInStop = Reservations.Stop;
        var routeName = Reservations.RouteName;
        List<int> seatRow = Reservations.SeatRow;
        List<int> seatCol = Reservations.SeatCol;
        List<(int row, int col)> seats = new List<(int row, int col)>();
        for (int i = 0; i < seatRow.Count; i++)
        {
            seats.Add((seatRow[i], seatCol[i]));
        }
        string seatsString = string.Join(",", seats);
        return new List<string> { $"{checkInStop}", $"{routeName}", $"{seatsString}" };
    }

    public static void BackToStartMenu()
    {
        Console.WriteLine("U keert terug naar het Startmenu.\n");
        Thread.Sleep(3000);
        CustomerStartMenu.Start();
    }
}
