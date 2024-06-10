using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Formats.Asn1;
using Microsoft.VisualBasic;

public static class CustomerReservation
{
    private static BusLogic busLogic = new();
    private static SeatLogic seatLogic = new();
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
        List<string> header = new() {"Halte", "Route", "Zitplaats", "Tijd"};
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
            (int?, string) SelectedRowIndex = tableReservations.PrintTable(header, Reservations, GenerateRow, title);
            if(SelectedRowIndex.Item1 == null) // escape to go back into Startmenu
            {
                CustomerStartMenu.Start();
                return;
            }
            else if (SelectedRowIndex.Item2 == "backspace") // backspace to delete reservation
            {
                CancelReservation(SelectedRowIndex.Item1);
                ShowAllPricesInformation();
            }
            else
            {
                List<string> selectedRow = GenerateRow(currentAccount.Reservations[Convert.ToInt32(SelectedRowIndex.Item1)]);
                while(true)
                {
                    selectedRow = GenerateRow(currentAccount.Reservations[Convert.ToInt32(SelectedRowIndex.Item1)]);
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
                            foreach (string stoel in SeatString(currentAccount.Reservations[Convert.ToInt32(SelectedRowIndex.Item1)]))
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

    public static void CancelReservation(int? selectedRowIndex)
    {
        Console.Clear();
        Console.WriteLine("Weet u zeker dat u deze reservering wilt annuleren.");
        bool answer = JaNee();
        if (answer)
        {
            ReservationModel toCancel = UserLogin.loggedInAccount.Reservations[Convert.ToInt32(selectedRowIndex)];
            BusModel busToUpdate = busLogic.GetById(toCancel.BusId);
            List<(int, int)> seatCoords = new List<(int, int)>();
            for (int i = 0; i < toCancel.SeatCol.Count; i++)
            {
                (int, int) coords = (toCancel.SeatRow[i],toCancel.SeatCol[i]);
                seatCoords.Add(coords);
            }
            SeatingMapMenu.MakeAvailable(busToUpdate, seatCoords);

            UserLogin.loggedInAccount.Reservations.Remove(toCancel);
            accountsLogic.UpdateList(UserLogin.loggedInAccount);
            Console.WriteLine("Uw reservering is geannuleerd.");
            ShowAllPricesInformation();
        }
        else
        {
            ShowAllPricesInformation();
        }
    }

    public static List<string> GenerateRow(ReservationModel Reservations)
    {
        var checkInStop = Reservations.Stop;
        var routeName = Reservations.RouteName;
        List<int> seatRow = Reservations.SeatRow;
        List<int> seatCol = Reservations.SeatCol;
        List<string> seats = new List<string>();
        RouteModel selectedRouteModel = routesInBusses[SelectedRowIndex.Value];
        List<StopModel> stops = selectedRouteModel.Stops;


        for (int i = 0; i < seatRow.Count; i++)
        {
            var time = stops[i].Time;
        }

        for (int i = 0; i < seatRow.Count; i++)
        {
            seats.Add($"Rij: {seatRow[i]} Stoel: {seatCol[i]}");
        }
        string seatsString = string.Join(", ", seats);
        return new List<string> { $"{checkInStop}", $"{routeName}", $"{seatsString}", $"{time}" };
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

    public static bool JaNee()
    {
        int selectedOption = 1;

        DisplayOptionsJaNee(selectedOption);

        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedOption = Math.Max(1, selectedOption - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedOption = Math.Min(2, selectedOption + 1);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    switch (selectedOption)
                    {
                        case 1:
                            return true;
                        case 2:
                            return false;
                    }
                    break;
            }
            Console.Clear();
            DisplayOptionsJaNee(selectedOption);
        }

    }
    public static void DisplayOptionsJaNee(int selectedOption)
    {
        Console.WriteLine("Selecteer een optie:");

        // Display option 1
        Console.ForegroundColor = selectedOption == 1 ? ConsoleColor.Green: ConsoleColor.White;
        Console.Write(selectedOption == 1 ? ">> " : "   ");
        Console.WriteLine("Ja.");

        // Display option 2
        Console.ForegroundColor = selectedOption == 2 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 2 ? ">> " : "   ");
        Console.WriteLine("Nee.");

        Console.ResetColor();
    }
}
