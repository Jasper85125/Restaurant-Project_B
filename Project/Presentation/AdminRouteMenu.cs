using System.Data.Common;
using System.Formats.Asn1;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;

public static class AdminRouteMenu
{
    private static RouteLogic routeLogic = new();
    private static BusLogic busLogic = new();
    private static StopLogic stopLogic = new();
    private static TableLogic<RouteModel> tableRoutes = new();
    private static CustomerTableLogic<RouteModel> tableRoutesKlant = new();
    private static TableLogic<StopModel> tableStops = new();

    static public void Start()
    {
        Console.Clear();
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
                    // Perform action based on selected option
                    switch (selectedOption)
                    {
                        case 1:
                            PrintedOverview();
                            //MoreInformation();
                            Start();
                            break;
                        case 2:
                            MakeStop();
                            break;
                    }
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    BackToAdminMenu();
                    AdminStartMenu.Start();
                    break;
            }
            Console.Clear();
            DisplayOptions(selectedOption);
        }
    }

    static void DisplayOptions(int selectedOption)
    {
        Console.WriteLine("\nWelkom bij het overzicht van het routemenu.");
        Console.WriteLine("Selecteer een optie:\n");

        // Display option 4
        Console.ForegroundColor = selectedOption == 1 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 1 ? ">> " : "   ");
        Console.WriteLine("Een overzicht van alle routes.");

        // Display option 5
        Console.ForegroundColor = selectedOption == 2 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 2 ? ">> " : "   ");
        Console.WriteLine("Een halte toevoegen.");

        // Reset text color
        Console.ResetColor();

        Console.Write("\nAls je de juiste hebt klik op ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Enter");
        Console.ResetColor();
        Console.Write(".");
        Console.Write("\nOm naar het adminhoofdmenu te gaan klik op");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(" Escape");
        Console.ResetColor();
        Console.WriteLine(".\n");
    }
    

    public static List<RouteModel> Overview()
    {
        List<RouteModel> overview = routeLogic.GetAll();
        return overview;
    }

    public static void AddRoute()
    {
        List<RouteModel> overview = routeLogic.GetAll();
        // check if Name is string
        Console.WriteLine("Wat is de naam van de nieuwe route?");
        string? newName = Console.ReadLine();
        while (!Helper.IsOnlyLetterSpaceDash(newName))
        {
            ColorPrint.PrintRed($"'{newName}' is geen geldige optie.");
            Console.WriteLine("De naam van de route kan alleen bestaan uit letters.");
            Console.WriteLine("Wat is de naam van de nieuwe route?");
            newName = Console.ReadLine();
        }

        //checks if Duration is Int input
        Console.WriteLine("Hoelang duurt de route in uur?");
        string? newDuration = Console.ReadLine();
        while(!Helper.IsValidInteger(newDuration)) 
        {
            ColorPrint.PrintRed($"'{newDuration}' is geen geldige optie.");
            Console.WriteLine("De duur van de route moet in hele getallen gegeven worden.");
            Console.WriteLine("Hoelang duurt de route in uren?");
            newDuration = Console.ReadLine();
        }

        // Makes the route
        RouteModel newRoute = new RouteModel(routeLogic.GenerateNewId(), Convert.ToInt32(newDuration), newName, false);
        Console.WriteLine("Nu gaat U haltes toevoegen aan de route");
        AddStopToRoute(newRoute);

    }
    public static void Listupdater(RouteModel model){
        routeLogic.UpdateList(model);
    }

    public static void AddStopToRoute(RouteModel route, List<StopModel> selectedStops = null)
    {
        List<StopModel> selectedStopsCopy = new List<StopModel> ();
        if (selectedStops != null)
        {
            selectedStopsCopy = selectedStops.ToList();
        }
        if (selectedStops == null) selectedStops = new List<StopModel> (); 
        if (stopLogic.GetAll() == null || stopLogic.GetAll().Count == 0)
        {
            Console.WriteLine("Er zijn geen haltes gevonden in de database.");
            Console.WriteLine("Voeg eerst een halte toe");
            MakeStop();
            AddStopToRoute(route);
        }
        else
        {
            List<string> header = new() {"Haltenummer", "Naam", "Tijd"};
            List<StopModel> stopModels = stopLogic.GetAll();

            //Hier komt het toevoegen van haltes door middel van kiezen in de tabel.
            bool checkStopName = true;
            List<StopModel> stops = stopLogic.GetAll().OrderBy(stop => stop.Name).ToList();
                  
            int selectedIndex = 0;
            int currentPage = 1;
            int pageSize = 10;
            int totalPages = (int)Math.Ceiling((double)stops.Count / pageSize);

            string duplicateStopMessage = "";

            while (checkStopName)
            {
                Console.Clear();

                Console.WriteLine($"Naam: {route.Name}, tijdsduur: {route.Duration}.\n");
                if (selectedStops.Count() != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    for (int i = 0; i < selectedStops.Count; i++)
                    {
                        Console.Write(selectedStops[i].Name);
                        
                        if (i < selectedStops.Count - 1 && selectedStops.Count > 1)
                        {
                            Console.Write(", ");
                        }
                    }

                    Console.ResetColor();
                }
                if (!string.IsNullOrEmpty(duplicateStopMessage))
                {
                    ColorPrint.PrintRed(duplicateStopMessage);
                }

                Console.WriteLine($"\n\nSelecteer een halte (Pagina {currentPage}/{totalPages}):");

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
                    if (selectedStops.Contains(stops[i]))
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

                Console.WriteLine("\nDruk op:");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Spatie ");
                Console.ResetColor();
                Console.WriteLine("om te selecteren.");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Backspace ");
                Console.ResetColor();
                Console.WriteLine("om te deselecteren.");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Enter ");
                Console.ResetColor();
                Console.WriteLine("om de lijst op te slaan.");

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
                    case ConsoleKey.Spacebar:
                        StopModel selectedStop = stops[selectedIndex];
                        if (!selectedStops.Contains(selectedStop))
                        {
                            selectedStops.Add(selectedStop);
                            duplicateStopMessage = "";
                        }
                        else
                        {
                            duplicateStopMessage = $"\n{selectedStop.Name} is al toegevoegd, selecteer een andere halte.";
                        }
                        break;
                    case ConsoleKey.Backspace:
                        if (selectedStops.Count > 0 && selectedIndex >= 0)
                        {
                            StopModel removeStop = stops[selectedIndex];
                            selectedStops.Remove(removeStop);
                        }
                        break;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        
                        if (selectedStops.Count() != 0)
                        {
                            Console.WriteLine("Wilt u deze haltes toevoegen?: ");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            for (int i = 0; i < selectedStops.Count; i++)
                            {
                                Console.Write(selectedStops[i].Name);
                                
                                if (i < selectedStops.Count - 1 && selectedStops.Count > 1)
                                {
                                    Console.Write(", ");
                                }
                            }
                            Console.ResetColor();
                            Console.WriteLine($"\naan route: {route.Name}?");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("\nBackspace ");
                            Console.ResetColor();
                            Console.Write("om te annuleren.");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(" Enter ");
                            Console.ResetColor();
                            Console.Write("om de lijst op te slaan.");
                            ConsoleKeyInfo ConfirmStops = Console.ReadKey(true);
                            switch (ConfirmStops.Key)
                            {
                                case ConsoleKey.Enter:
                                    Console.Clear();
                                    foreach (StopModel halte in selectedStops)
                                    {
                                        if (selectedStopsCopy.Contains(halte))
                                        {

                                        }
                                        else
                                        {
                                            RouteLogic.AddToRoute(halte, route);
                                        }
                                    }
                                    foreach (StopModel halte in selectedStopsCopy)
                                    {
                                        if (!selectedStops.Contains(halte))
                                        {
                                            RouteLogic.RemoveFromRoute(halte, route);
                                        }
                                    }
                                    if (ConfirmValue(route))
                                    {
                                        routeLogic.UpdateList(route);
                                    }
                                    else
                                    {

                                    }
                                    //Console.WriteLine("\ntoegevoegd");
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


    public static void PrintedOverview()
    { 
        List<string> header = new() {"routenummer", "naam", "tijdsduur(uur)", "halte(s)", "begintijd", "eindtijd", "actieviteit"};
        string title = "Routes overzicht";
        
        List<StopModel> stopsList = new() {};
        string kind = "route";
        while(true){
            List<RouteModel> routeModels = routeLogic.GetAll();
        
            if (routeModels == null || routeModels.Count == 0)
            {
                RouteModel newRouteModel = new(routeLogic.GenerateNewId(),0,"", false);
                routeLogic.UpdateList(newRouteModel);
            }
            else
            {
                while(true){
                    (List<string> SelectedRow, int SelectedRowIndex)? TableInfo= tableRoutes.PrintTable(header, routeModels, GenerateRow, title, Listupdater, kind);
                    if(TableInfo == null){
                        Start();
                        return; //exit loop door escape

                    }
                    else{
                        int selectedRowIndex = TableInfo.Value.SelectedRowIndex;

                        if(selectedRowIndex == routeModels.Count())
                        {
                            RouteModel newRouteModel = new(routeLogic.GenerateNewId(),0,"", false);
                            routeLogic.UpdateList(newRouteModel);
                            break;
                        }
                        while(true){
                            List<string> selectedRow = GenerateRow(routeModels[selectedRowIndex]);
                            (string SelectedItem, int SelectedIndex)? result = tableRoutes.PrintSelectedRow(selectedRow, header);
                            if (result == null){
                                break; //exit loop door escape
                            }
                            else{
                                string selectedItem = result.Value.SelectedItem;
                                int selectedIndex = result.Value.SelectedIndex;
                                if (selectedIndex == 0){
                                    Console.WriteLine($"U kan {header[selectedIndex]} niet aanpassen.");
                                    Thread.Sleep(3000);
                                }
                                else if(selectedIndex == 1){
                                    Console.WriteLine($"Voer iets in om de {header[selectedIndex]} van de route te veranderen:");
                                    string Input = Console.ReadLine();

                                while(!Helper.IsOnlyLetterSpaceDash(Input))
                                {
                                    ColorPrint.PrintRed($"'{Input}' is geen geldige optie.");
                                    Console.WriteLine("De naam kan alleen bestaan uit letters.");
                                    Console.WriteLine("Wat is de naam van de nieuwe route?");
                                    Input = Console.ReadLine();
                                }

                                    //variable to check Name
                                    bool NameExists = false;

                                    // Check if the input Name already exists
                                    foreach(var nameIndex in routeModels) {
                                        if(Input == nameIndex.Name) {
                                            NameExists = true;
                                            break;
                                        }
                                    }

                                    if(NameExists) {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Naam bestaat al, geef een andere op.");
                                        Console.ResetColor();
                                        Thread.Sleep(3000);
                                    } else {
                                        //if Name does not exists, it gets added to the list
                                        routeModels[selectedRowIndex].Name = Input;
                                        routeLogic.UpdateList(routeModels[selectedRowIndex]);
                                        break;
                                    }
                            }
                            else if(selectedIndex == 2){
                                Console.WriteLine("Voer een nummer in het item te veranderen:");
                                string Input = Console.ReadLine();
                                while (!Helper.IsValidInteger(Input))
                                {
                                    ColorPrint.PrintRed($"'{Input}' is geen geldige optie.");
                                    Console.WriteLine("De duur van de route moet in hele getallen gegeven worden.");
                                    Console.WriteLine("Hoelang duurt de route in uren?");
                                    Input = Console.ReadLine();
                                }
                                routeModels[selectedRowIndex].Duration = Convert.ToInt32(Input);
                                routeLogic.UpdateList(routeModels[selectedRowIndex]);
                                break;

                                }
                                else if(selectedIndex == 3){
                                    Console.Clear();
                                    List<RouteModel>ListAllRoutes = routeLogic.GetAll();
                                    stopsList = ListAllRoutes[selectedRowIndex].Stops.ToList();
                                    AddStopToRoute(routeModels[selectedRowIndex], stopsList);
                                }
                                else if (selectedIndex == 3){
                                    Console.Clear();
                                    dynamic item = routeModels[selectedRowIndex];
                                    if (item.IsActive)
                                    {
                                        item.IsActive = false;
                                    }
                                    else{
                                    item.IsActive = true;
                                    }
                                    Listupdater(item);
                                }
                            }
                        }
                    }         
                }
            }
        }
    
    }



    public static void AddToBus()
    {
        List<BusModel> ListAllBusses = busLogic.GetAll();
        AdminBusMenu.ShowAllBusInformation();
    }

    public static void MakeStop()
    {
        bool checkStopName = true;
        while (checkStopName)
        {
            Console.WriteLine("Wat is de naam van de halte?");
            Console.Write("\nOm terug te keren klik op");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" Backspace");
            Console.ResetColor();
            Console.WriteLine(".\n");
    
             // Wait for key press
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            // Check arrow key presses
            switch (keyInfo.Key)
            {
                case ConsoleKey.Backspace:
                    // Move to the previous option
                    return;
            }
            string? newName = Console.ReadLine();
            
            if (Helper.IsOnlyLetterSpaceDash(newName))
            {
                foreach (StopModel stop in stopLogic.GetAll())
                {
                    if (stop.Name == newName)
                    {
                        Console.WriteLine("Halte bestaat al");
                        MakeStop();
                    }
                }
                StopModel newStop = new StopModel(stopLogic.GenerateNewId() ,Convert.ToString(newName));
                stopLogic.UpdateList(newStop);
                checkStopName = false;
                bool switchMainer = true;
                while (switchMainer)
                {
                    Console.WriteLine("Wilt U nog een halte toevoegen. Ja of Nee");
                    string? answer = Console.ReadLine();
                    switch (answer.ToLower())
                    {
                        case "ja":
                            MakeStop();
                            break;
                        case "nee":
                            return;
                        default:
                            ColorPrint.PrintRed($"{answer} is geen geldige input. Probeer het opnieuw.");
                            break;
                    }
                }
            }
            else
            {
                ColorPrint.PrintRed($"'{newName}' is geen geldige optie.");
                Console.WriteLine("De naam van een halte kan alleen letters zijn.");
                Console.WriteLine("Probeer het nog een keer.\n");
            }
        }
        Console.Clear();
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
        var active = routeModel.IsActive;
        foreach(StopModel stop in stops){
            allStops.Add(stop);
        }
        var stopsString = string.Join(", ", stops.Select(stop => stop.Name));
        string activity = "";
        if (active)
        {
            activity = "Actief";
        }
        else{
            activity = "Non-actief";
        }
        return new List<string> { $"{id}", $"{name}", $"{duration}", stopsString, $"{beginTime}", $"{endTime}",$"{activity}" };
    }

    // public static List<string> GenerateRow(StopModel stopModel)
    // {
    //     var id = stopModel.Id;
    //     var name = stopModel.Name;
    //     var time = stopModel.Time;
    //     return new List<string> {$"{id}", $"{name}", $"{time}"};
    // }
    public static List<string> GenerateRowForSelectRoute(RouteModel routeModel)
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
        return new List<string> {$"{name}", $"{duration}", stopsString, $"{beginTime}", $"{endTime}"};
    }

    public static RouteModel SelectRoute()
    {
        List<string> header = new() {"Naam", "Tijdsduur", "Stops", "Begintijd", "Eindtijd"};
        List<RouteModel> routeModels = routeLogic.GetAll();
        string title = "Selecteer een route voor de bus.";
        if (routeModels == null || routeModels.Count == 0)
        {
            Console.WriteLine("Lege data.");
            return null;
        }
        else
        {
            var SelectedRowIndex = tableRoutesKlant.PrintTable(header, routeModels, GenerateRowForSelectRoute, title);
            if(SelectedRowIndex != null){
                return routeModels[SelectedRowIndex.Value];
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
            ColorPrint.PrintRed(IsUpdate ? "Ongeldige invoer." : "Fout: Nieuwe busgegevens ontbreken!");
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
                ColorPrint.PrintRed("Toevoegen geannuleerd.");
                Thread.Sleep(3000);
                Console.Clear();
                return false;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                ColorPrint.PrintGreen("Data is toegevoegd!");
                Thread.Sleep(3000);
                Console.Clear();
                return true;
            }
            else
            {
                ColorPrint.PrintRed("Ongeldige invoer!");
                Thread.Sleep(3000);
                Console.Clear();
            }
        }while(true);
    }

    public static void BackToAdminMenu()
    {
        ColorPrint.PrintYellow("U keert terug naar het adminhoofdmenu.");
        Thread.Sleep(3000);
        AdminStartMenu.Start();
    }
}