public static class CustomerRouteMenu
{
    private static SeatLogic seatLogic = new();
    private static RouteLogic routeLogic = new();
    private static BusLogic busLogic = new();
    private static StopLogic stopLogic = new();
    private static CustomerTableLogic<RouteModel> tableRoutes = new();
    private static BasicTableLogic<StopModel> tableStops = new();

    static public void Start()
    {
        Console.Clear();
        PrintedOverview();
        
    }

   public static void PrintedOverview()
{ 
    List<string> Header = new() { "Naam", "Tijdsduur(uur)", "Halte(s)", "Type bus", "Begintijd", "Eindtijd" };
    string Title = "Kies een beschikbare route waar u op wilt reserveren:\n";

    while (true)
    {
        List<BusModel> busModels = busLogic.GetAll();
        List<BusModel> busWithRoute = new List<BusModel>();
        foreach (BusModel bus in busModels)
        {
            if (bus.Route.Any() && bus.IsActive == true)
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
                        Console.WriteLine($"Kenteken: {selectedBusModel.LicensePlate}\n");
                        Console.WriteLine($"Selecteer de halte waar u wilt opstappen (Pagina {currentPage}/{totalPages}):");

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
                            Console.WriteLine($"{stops[i].Name} | {stops[i].Time?.ToString(@"hh\:mm") ?? "N/A"}");
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
                                Console.Write($"{selectedStop.Name} | {selectedStop.Time?.ToString(@"hh\:mm") ?? "N/A"}");
                                Console.ResetColor();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("\nBackspace ");
                                Console.ResetColor();
                                Console.Write("om te annuleren.");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(" \nEnter ");
                                Console.ResetColor();
                                Console.Write("om een halte te selecteren.");

                                ConsoleKeyInfo confirmInput = Console.ReadKey(true);

                                bool stop = false;
                                while (stop == false)
                                {
                                    if (selectedBusModel.Seats == "Plebs")
                                    {
                                        Dictionary<(int Row, int Col), SeatModel> seatingMap = selectedBusModel.SeatingMap;
                                        SeatModel[,] seatModels = seatLogic.ConvertTo2DArr(seatingMap);
                                        SeatingMapMenu.Start(seatModels, selectedBusModel,selectedRouteModel, selectedStop);
                                        Dictionary<(int Row, int Col), SeatModel> updatedSeatingMap = seatLogic.ConvertToDict(seatModels);
                                        selectedBusModel.SeatingMap = updatedSeatingMap;
                                        busLogic.UpdateList(selectedBusModel);
                                    }
                                    else if (selectedBusModel.Seats == "Business")
                                    {
                                        Dictionary<(int Row, int Col), SeatModel> seatingMap = selectedBusModel.SeatingMap;
                                        SeatModel[,] seatModels = seatLogic.ConvertTo2DArr(seatingMap);
                                        SeatingMapMenu2.Start(seatModels, selectedBusModel,selectedRouteModel, selectedStop);
                                        Dictionary<(int Row, int Col), SeatModel> updatedSeatingMap = seatLogic.ConvertToDict(seatModels);
                                        selectedBusModel.SeatingMap = updatedSeatingMap;
                                        busLogic.UpdateList(selectedBusModel);
                                    }
                                    
                                }
                                break;
                            case ConsoleKey.Escape:
                                Console.Clear();
                                CustomerRouteMenu.Start();
                                return;
                            default:
                                break;
                        }
                    }
                }
            }
            else
            {
                Console.Clear();
                ColorPrint.PrintYellow("U gaat terug naar het startmenu");
                Thread.Sleep(3000);
                CustomerStartMenu.Start();
                break;
            }
        }
    }
}
    
    public static List<string> GenerateRow(RouteModel routeModel)
    {
        string KindSeat = "";
        List<BusModel> busModels = busLogic.GetAll();

        var busWithRoute = busModels.Where(bus => bus.Route.Any() && bus.IsActive).ToList();
        foreach( var bus in busWithRoute){
            foreach(var route in bus.Route){
                if (route.Id == routeModel.Id){
                    KindSeat = bus.Seats;
                }
            }
        }
        List<StopModel> allStops = new() {};
        var id = routeModel.Id;
        var duration = routeModel.Duration;
        var name = routeModel.Name;
        var stops = routeModel.Stops;
        var beginTime = routeModel.beginTime?.ToString(@"hh\:mm");
        var endTime = routeModel.endTime?.ToString(@"hh\:mm");
        foreach(StopModel stop in stops){
            allStops.Add(stop);
        }
        var stopsString = string.Join(", ", stops.Select(stop => stop.Name));
        return new List<string> {$"{name}", $"{duration}", stopsString, $"{KindSeat}", $"{beginTime}", $"{endTime}" };
    }
}