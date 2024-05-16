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
        int selectedOption = 1; // Default selected option
        
        // Display options
        DisplayOptions(selectedOption); 

        while (true)
        {
            // Wait for key press
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            // Check arrow key presses
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    // Move to the previous option
                    selectedOption = Math.Max(1, selectedOption - 1);
                    break;
                case ConsoleKey.DownArrow:
                    // Move to the next option
                    selectedOption = Math.Min(2, selectedOption + 1);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    // Perform action based on selected option (e.g., execute corresponding function)
                    switch (selectedOption)
                    {
                        case 1:
                            PrintedOverview();
                            //MoreInformation();
                            Start();
                            break;
                        case 2:
                            CustomerStartMenu.Start();
                            break;
                    }
                    break;
            }
            Console.Clear();
            DisplayOptions(selectedOption);
        }
    }

    static void DisplayOptions(int selectedOption)
    {
        Console.WriteLine("\nWelkom bij het overzicht van het route menu.\n");
        Console.WriteLine("Wat wilt u doen?");

        // Display option 1
        Console.ForegroundColor = selectedOption == 1 ? ConsoleColor.Green: ConsoleColor.White;
        Console.Write(selectedOption == 1 ? ">> " : "   ");
        Console.WriteLine("[1] Een overzicht van alle routes.");

        // Display option 2
        Console.ForegroundColor = selectedOption == 2 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 2 ? ">> " : "   ");
        Console.WriteLine("[2] Ga terug naar het vorige menu.");

        // Reset text color
        Console.ResetColor();
    }
    

    public static List<RouteModel> Overview()
    {
        List<RouteModel> overview = routeLogic.GetAll();
        return overview;
    }

    public static void PrintedOverview()
    { 
        List<string> Header = new() {"Routenummer", "Naam", "Tijdsduur", "Stops", "Begintijd", "Eindtijd"};
        List<RouteModel> routeModels = routeLogic.GetAll();
        List<StopModel> StopsList = new() {};
        if (routeModels == null || routeModels.Count == 0)
        {
            Console.WriteLine("Lege data.");
        }
        else
        {
            while(true){
                (List<string> SelectedRow, int SelectedRowIndex)? TableInfo= tableRoutes.PrintTable(Header, routeModels, GenerateRow);
                if(TableInfo != null){
                    int selectedRowIndex = TableInfo.Value.SelectedRowIndex;
                    while(true)
                    {
                        // (string SelectedItem, int SelectedIndex)? result = tableRoutes.PrintSelectedRow(TableInfo.Value.SelectedRow, Header);
                        // Console.WriteLine($"Selected Item: {result.Value.SelectedItem}, Selected Index: {result.Value.SelectedIndex}"); //#test om PrintSelectedRow functie te testen.
                        // if (result == null)
                        // {
                        //     Console.WriteLine("U keert terug naar het prijsmenu overzicht.");
                        //     break;
                        // }
                        // TableInfo.Value.SelectedRow.ForEach(i => Console.WriteLine(i));
                        RouteModel selectedRouteModel = routeLogic.GetById(Convert.ToInt32(TableInfo.Value.SelectedRow[0]));
                        // Console.WriteLine($"Route: {selectedRouteModel.Name}");
                        // foreach (var halte in selectedRouteModel.Stops)
                        // {
                        //     Console.WriteLine($"{halte.Name}");
                        // }
                        // Console.WriteLine("Druk op een toets om door te gaan...");
                        // Console.ReadKey(true);
                        

                        
                    
                        // List<string> Header = new() {"Haltenummer", "Naam", "Tijd"};
                        // List<StopModel> stopModels = stopLogic.GetAll();

                        //Hier komt het toevoegen van haltes door middel van kiezen in de tabel.
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
                                if (stops.Contains(stops[i]))
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.WriteLine(stops[i].Name);
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.WriteLine(stops[i].Name);
                                    Console.ResetColor();
                                }
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
                                // case ConsoleKey.Spacebar:
                                //     StopModel selectedStop = stops[selectedIndex];
                                //     if (!stops.Contains(selectedStop))
                                //     {
                                //         stops.Add(selectedStop);
                                //         duplicateStopMessage = "";
                                //     }
                                //     else
                                //     {
                                //         duplicateStopMessage = $"\n{selectedStop.Name} is al toegevoegd, selecteer een andere halte.";
                                //     }
                                //     break;
                                // case ConsoleKey.Backspace:
                                //     if (stops.Count > 0 && selectedIndex >= 0)
                                //     {
                                //         StopModel removeStop = stops[selectedIndex];
                                //         stops.Remove(removeStop);
                                //     }
                                //     break;
                                case ConsoleKey.Enter:
                                    Console.Clear();
                                    
                                    if (stops.Count == 1)
                                    {
                                        Console.WriteLine("Wilt u hier instappen: ");
                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        // for (int i = 0; i < stops.Count; i++)
                                        // {
                                            //Console.Write(stops[i].Name);
                                            
                                        //     if (i < stops.Count - 1 && stops.Count > 1)
                                        //     {
                                        //         Console.Write(", ");
                                        //     }
                                        // }
                                        Console.Write(stops[0].Name);
                                        
                                        Console.ResetColor();
                                        Console.WriteLine($"\naan route: {selectedRouteModel.Name}?");
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.Write("\nBackspace ");
                                        Console.ResetColor();
                                        Console.Write("om te annuleren.");
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.Write(" Enter ");
                                        Console.ResetColor();
                                        Console.Write("om een halte te selecteren.");
                                        ConsoleKeyInfo ConfirmStops = Console.ReadKey(true);
                                        switch (ConfirmStops.Key)
                                        {
                                            case ConsoleKey.Enter:
                                                Console.Clear();
                                                checkStopName = false;
                                                break;
                                            case ConsoleKey.Backspace:
                                                break;
                                        }
                                    }

                                    break;
                                case ConsoleKey.Escape:
                                    checkStopName = false;
                                    break;
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