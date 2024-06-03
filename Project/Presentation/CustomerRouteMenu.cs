using System.Drawing;

public static class CustomerRouteMenu
{
    private static SeatLogic seatLogic = new();
    private static RouteLogic routeLogic = new();
    private static BusLogic busLogic = new();
    private static StopLogic stopLogic = new();
    private static CustomerTableLogic<RouteModel> tableRoutes = new();
    private static CustomerTableLogic<StopModel> tableStops = new();

    static public void Start()
    {
        Console.Clear();
        PrintedOverview();
        
    }

   public static void PrintedOverview()
{ 
    List<string> Header = new() { "Naam", "Tijdsduur(uur)", "Halte(s)", "Begintijd", "Eindtijd" };
    string Title = "Beschikbare routes:\n";

    while (true)
    {
        List<BusModel> busModels = busLogic.GetAll();
        List<BusModel> busWithRoute = new List<BusModel>();
        foreach (BusModel bus in busModels)
        {
            if (bus.Route.Any())
            {
                busWithRoute.Add(bus);
            }
        }
        if (busWithRoute == null || busWithRoute.Count == 0)
        {
            Console.WriteLine("Op dit moment zijn er geen beschikbare routes.");
            Thread.Sleep(1000);
            CustomerStartMenu.Start();
            return;
        }
        // List<RouteModel> routeModels = routeLogic.GetAll();
        // if (routeModels == null || routeModels.Count == 0)
        // {
        //     Console.WriteLine("Op dit moment zijn er geen beschikbare routes.");
        //     Thread.Sleep(1000);
        //     CustomerStartMenu.Start();
        //     return;
        // }
        else
        {
            List<BusModel> busList = new List<BusModel>();
            List<RouteModel> routesInBusses = new List<RouteModel>();
            foreach (BusModel bus in busWithRoute)
            {
                foreach(RouteModel route in bus.Route)
                {
                    routesInBusses.Add(route);
                    busList.Add(bus);
                }  
            }
            var SelectedRowIndex = tableRoutes.PrintTable(Header, routesInBusses, GenerateRow, Title);
            if (SelectedRowIndex  != null)
            {

                while (true)
                {
                    RouteModel selectedRouteModel = routesInBusses[SelectedRowIndex.Value];
                    BusModel selectedBusModel = busList[SelectedRowIndex.Value];

                    bool checkStopName = true;
                    List<StopModel> stops = selectedRouteModel.Stops;

                    int selectedIndex = 0;
                    int currentPage = 1;
                    int pageSize = 10;
                    int totalPages = (int)Math.Ceiling((double)stops.Count / pageSize);

                    while (checkStopName)
                    {
                        Console.Clear();

                        Console.WriteLine($"Naam: {selectedRouteModel.Name}, tijdsduur: {selectedRouteModel.Duration}");
                        Console.WriteLine($"Naam: {selectedBusModel.LicensePlate}\n");
                        Console.WriteLine($"Selecteer een halte (Pagina {currentPage}/{totalPages}):");

                        int startIndex = (currentPage - 1) * pageSize;
                        int endIndex = Math.Min(startIndex + pageSize, stops.Count);

                        for (int i = startIndex; i < endIndex; i++)
                        {
                            if (i == selectedIndex)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(">> ");
                            }
                            else
                            {
                                Console.Write("   ");
                            }
                            Console.WriteLine(stops[i].Name);
                            Console.ResetColor();
                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("Enter ");
                        Console.ResetColor();
                        Console.WriteLine("om een halte te selecteren.");
                        Console.Write("Om een stap terug te gaan druk op");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" Escape");
                        Console.ResetColor();
                        Console.Write(".\n");
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.UpArrow:
                                if (selectedIndex > 0)
                                {
                                    selectedIndex--;
                                }
                                break;
                            case ConsoleKey.DownArrow:
                                if (selectedIndex < endIndex - 1)
                                {
                                    selectedIndex++;
                                }
                                break;
                            case ConsoleKey.LeftArrow:
                                if (currentPage > 1)
                                {
                                    currentPage--;
                                    selectedIndex -= pageSize;
                                    if (selectedIndex < 0)
                                    {
                                        selectedIndex = 0;
                                    }
                                }
                                break;
                            case ConsoleKey.RightArrow:
                                if (currentPage < totalPages)
                                {
                                    currentPage++;
                                    selectedIndex += pageSize;
                                    if (selectedIndex >= stops.Count)
                                    {
                                        selectedIndex = stops.Count - 1;
                                    }
                                }
                                break;

                            case ConsoleKey.Enter:
                                StopModel selectedStop = stops[selectedIndex];

                                Console.Clear();
                                Console.Write("Wilt u hier instappen: ");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Write(selectedStop.Name);
                                Console.ResetColor();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("\nBackspace ");
                                Console.ResetColor();
                                Console.Write("om te annuleren.");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(" Enter ");
                                Console.ResetColor();
                                Console.Write("om een halte te selecteren.");

                                ConsoleKeyInfo confirmInput = Console.ReadKey(true);

                                bool stop = false;
                                while (stop == false)
                                {
                                    switch (confirmInput.Key)
                                    {
                                        case ConsoleKey.Enter:
                                            // BusModel bus = new(busLogic.GenerateNewId(),0,"",false);

                                            // SeatModel[,] seatModels = new SeatModel[6,8];
                                            // seatLogic.CreateSeats(seatModels);
                                            // seatLogic.PrintArr(seatModels);
                                            // Dictionary<(int Row, int Col), SeatModel> seatingMap = seatLogic.ConvertToDict(seatModels);
                                            // bus.SeatingMap = seatingMap;
                                            // busLogic.UpdateList(bus);

                                            Dictionary<(int Row, int Col), SeatModel> seatingMap = selectedBusModel.SeatingMap;
                                            SeatModel[,] seatModels = seatLogic.ConvertTo2DArr(seatingMap);

                                            SeatingMapMenu.Start(seatModels, selectedBusModel,selectedRouteModel, selectedStop);
                                            Dictionary<(int Row, int Col), SeatModel> updatedSeatingMap = seatLogic.ConvertToDict(seatModels);
                                            selectedBusModel.SeatingMap = updatedSeatingMap;

                                            busLogic.UpdateList(selectedBusModel);

                                            /*
                                            SeatModel[,] seatModels = new SeatModel[6,8];
                                            seatLogic.CreateSeats(seatModels);
                                            */

                                            stop = true;
                                            break;
                                        case ConsoleKey.Escape:
                                            stop = true;
                                            break;
                                        default:
                                            confirmInput = Console.ReadKey(true);
                                            break;
                                    }
                                }
                                break;
                            case ConsoleKey.Escape:
                                Start();
                                return;
                            default:
                                Console.WriteLine("Ongeldige invoer. Probeer het opnieuw.");
                                break;
                        }
                    }
                }
            }
            else
            {
                break;
            }
        }
    }
}
    
    public static List<string> GenerateRow(RouteModel routeModel)
    {
        List<StopModel> allStops = new() {};
        var id = routeModel.Id;
        var duration = routeModel.Duration;
        var name = routeModel.Name;
        var stops = routeModel.Stops;
        var beginTime = routeModel.beginTime;
        var endTime = routeModel.endTime;
        foreach(StopModel stop in stops){
            allStops.Add(stop);
        }
        var stopsString = string.Join(", ", stops.Select(stop => stop.Name));
        return new List<string> {$"{name}", $"{duration}", stopsString, $"{beginTime}", $"{endTime}" };
    }


    public static bool ConfirmValue(RouteModel newRoute, string UpdatedValue = null, bool IsUpdate = false)
    {
        if (IsUpdate && string.IsNullOrEmpty(UpdatedValue) || !IsUpdate && (newRoute == null || string.IsNullOrEmpty(newRoute.Name)))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(IsUpdate ? "Ongeldige invoer." : "Fout: Nieuwe busgegevens ontbreken!");
            Console.ResetColor();
            Thread.Sleep(3000);
            Console.Clear();
            return false;
        }
        List<StopModel> stops = newRoute.Stops;
        var stopsString = string.Join(", ", stops.Select(stop => stop.Name));

        do
        {
            ConsoleKeyInfo keyInfo;
            Console.WriteLine(!IsUpdate ? $"U staat op het punt een nieuwe route toe te voegen met de volgende info:\nNaam: {newRoute.Name}, Tijdsduur: {newRoute.Duration}, Haltes: {stopsString}" : $"U staat op het punt oude data te veranderen: {UpdatedValue}");
            Console.Write("Druk op ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Enter");
            Console.ResetColor();
            Console.Write(" om door te gaan of druk op ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Backspace");
            Console.ResetColor();
            Console.WriteLine(" om te annuleren.");
            keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Toevoegen geannuleerd.");
                Console.ResetColor();
                Thread.Sleep(3000);
                Console.Clear();
                return false;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Data is toegevoegd!");
                Console.ResetColor();
                Thread.Sleep(3000);
                Console.Clear();
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ongeldige invoer!");
                Console.ResetColor();
                Thread.Sleep(3000);
                Console.Clear();
            }
        }while(true);
    }
}