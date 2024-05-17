using System.Data.Common;
using System.Formats.Asn1;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;

public static class RouteMenuklant
{
    private static RouteLogic routeLogic = new();
    private static BusLogic busLogic = new();
    private static StopLogic stopLogic = new();
    private static TableLogicklant<RouteModel> tableRoutes = new();
    private static TableLogicklant<StopModel> tableStops = new();

    static public void Start()
    {
        Console.Clear();
        PrintedOverview();
        
    }



    public static void PrintedOverview()
    { 
        List<string> Header = new() {"Routenummer", "Naam", "Tijdsduur", "Stops", "Begintijd", "Eindtijd"};
        List<RouteModel> routeModels = routeLogic.GetAll();
        List<StopModel> StopsList = new() {};
        string Title = "Beschikbare routes:\n";
        if (routeModels == null || routeModels.Count == 0)
        {
            Console.WriteLine("Lege data.");
        }
        else
        {
            while(true){
                (List<string> SelectedRow, int SelectedRowIndex)? TableInfo= tableRoutes.PrintTable(Header, routeModels, GenerateRow, Title);
                if(TableInfo != null){
                    int selectedRowIndex = TableInfo.Value.SelectedRowIndex;
                    while(true)
                    {

                        RouteModel selectedRouteModel = routeLogic.GetById(Convert.ToInt32(TableInfo.Value.SelectedRow[0]));

                        bool checkStopName = true;
                        List<StopModel> stops = selectedRouteModel.Stops.ToList();
                            
                        int selectedIndex = 0;
                        int currentPage = 1;
                        int pageSize = 10;
                        int totalPages = (int)Math.Ceiling((double)stops.Count / pageSize);

                        string duplicateStopMessage = "";

                        while (checkStopName)
                        {
                            Console.Clear();

                            Console.WriteLine($"Naam: {selectedRouteModel.Name}, tijdsduur: {selectedRouteModel.Duration}\n");
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

                            // Console.WriteLine("\nDruk op:");
                            // Console.ForegroundColor = ConsoleColor.Cyan;
                            // Console.Write("Spatie ");
                            // Console.ResetColor();
                            // Console.WriteLine("om te selecteren.");
                            // Console.ForegroundColor = ConsoleColor.Red;
                            // Console.Write("Backspace ");
                            // Console.ResetColor();
                            // Console.WriteLine("om te deselecteren.");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Enter ");
                            Console.ResetColor();
                            Console.WriteLine("om een halte te selecteren.");

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
                                    // Retrieve the selected stop based on selectedIndex
                                    StopModel selectedStop = stops[selectedIndex];

                                    Console.Clear();
                                    Console.WriteLine("Wilt u hier instappen: ");
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
                                    switch (confirmInput.Key)
                                    {
                                        case ConsoleKey.Enter:
                                            //hier verder
                                            break;
                                        case ConsoleKey.Backspace:
                                            break;
                                        default:
                                            Console.WriteLine("Ongeldige invoer. Probeer het opnieuw.");
                                            break;
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
                else{
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
        return new List<string> { $"{id}", $"{name}", $"{duration}", stopsString, $"{beginTime}", $"{endTime}" };
    }

    public static List<string> GenerateRow(StopModel stopModel)
    {
        var id = stopModel.Id;
        var name = stopModel.Name;
        var time = stopModel.Time;
        return new List<string> {$"{id}", $"{name}", $"{time}"};
    }

    public static RouteModel SelectRoute(){
        List<string> Header = new() {"Routenummer", "Naam", "Tijdsduur", "Stops", "Begintijd", "Eindtijd"};
        List<RouteModel> routeModels = routeLogic.GetAll();
        string Title = "Selecteer een route";
        if (routeModels == null || routeModels.Count == 0)
        {
            Console.WriteLine("Lege data.");
            return null;
        }
        else
        {
            (List<string> SelectedRow, int SelectedRowIndex)? TableInfo= tableRoutes.PrintTable(Header, routeModels, GenerateRow, Title);
            if(TableInfo != null){
                int selectedRowIndex = TableInfo.Value.SelectedRowIndex;
                return routeModels[selectedRowIndex];
            }
            else{
                return null;
            }
            
        }
    }   

    public static bool ConfirmValue(RouteModel newRoute, string UpdatedValue = null, bool IsUpdate = false)
    {
        if (IsUpdate && string.IsNullOrEmpty(UpdatedValue) || !IsUpdate && (newRoute == null || string.IsNullOrEmpty(newRoute.Name)))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(IsUpdate ? "Ongeldige invoer." : "Fout: Nieuwe busgegevens ontbreken!");
            Console.ResetColor();
            Thread.Sleep(2000);
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
                Thread.Sleep(2000);
                Console.Clear();
                return false;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Data is toegevoegd!");
                Console.ResetColor();
                Thread.Sleep(2000);
                Console.Clear();
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ongeldige invoer!");
                Console.ResetColor();
                Thread.Sleep(2000);
                Console.Clear();
            }
        }while(true);
    }
}