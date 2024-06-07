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
            ColorPrint.PrintYellow("U heeft nog geen reserveringen.");
            ColorPrint.PrintYellow("U gaat terug naar het Klantmenu.");
            Thread.Sleep(4000);
            Console.Clear();
            CustomerStartMenu.Start();
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
                        if (selectedIndex == 2)
                        {
                            foreach (string stoel in SeatString(currentAccount.Reservations[SelectedRowIndex.Value]))
                            {
                                Console.WriteLine(stoel);
                                Console.Write("Om een stap terug te gaan druk op");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(" Escape");
                                Console.ResetColor();
                                Console.Write(".\n");
                            }
                            while (true)
                            {
                                // Wait for key press
                                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                                // Check arrow key presses
                                switch (keyInfo.Key)
                                {
                                    case ConsoleKey.Escape:
                                        Console.Clear();
                                        ShowAllPricesInformation();
                                        break;
                                }
                                // Clear console and display options
                                Console.Clear();
                            }

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
        List<string> seats = new List<string>();
        for (int i = 0; i < seatRow.Count; i++)
        {
            seats.Add($"Rij: {seatRow[i]} Stoel: {seatCol[i]}");
        }
        string seatsString = string.Join(", ", seats);
        return new List<string> { $"{checkInStop}", $"{routeName}", $"{seatsString}" };
    }

    public static List<string> SeatString(ReservationModel Reservations)
    {
        List<int> seatRow = Reservations.SeatRow;
        List<int> seatCol = Reservations.SeatCol;
        List<string> seats = new List<string>();
        for (int i = 0; i < seatRow.Count; i++)
        {
            seats.Add($"|  Rij: {seatRow[i]} Stoel: {seatCol[i]}  |\n");
        }
        string seatsString = string.Join("", seats);
        return new List<string> {$"--------------------- \n{seatsString}---------------------"};
    }

    public static void BackToStartMenu()
    {
        Console.WriteLine("U keert terug naar het Startmenu.\n");
        Thread.Sleep(3000);
        CustomerStartMenu.Start();
    }
}
