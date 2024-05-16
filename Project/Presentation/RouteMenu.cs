using System.Data.Common;
using System.Formats.Asn1;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;

public static class RouteMenu
{
    private static RouteLogic routeLogic = new();
    private static BusLogic busLogic = new();
    private static StopLogic stopLogic = new();
    private static TableLogic<RouteModel> tableRoutes = new();
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
                    // Perform action based on selected option (e.g., execute corresponding function)
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
                case ConsoleKey.Backspace:
                    Console.Clear();
                    Console.WriteLine("U keert terug naar het admin hoofdmenu.");
                    Thread.Sleep(2000);
                    AdminStartMenu.Start();
                    break;
            }
            Console.Clear();
            DisplayOptions(selectedOption);
        }
    }

    static void DisplayOptions(int selectedOption)
    {
        Console.WriteLine("\nWelkom bij het overzicht van het route menu.\n");

        // Display option 4
        Console.ForegroundColor = selectedOption == 1 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 1 ? ">> " : "   ");
        Console.WriteLine("[1] Een overzicht van alle routes.");

        // Display option 5
        Console.ForegroundColor = selectedOption == 2 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 2 ? ">> " : "   ");
        Console.WriteLine("[2] Een halte toevoegen.");

        // Reset text color
        Console.ResetColor();

        Console.WriteLine("\nSelecteer een optie met de pijltjes.");
        Console.Write("Als je de juiste hebt klik op ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Enter");
        Console.ResetColor();
        Console.Write(".");
        Console.Write("\nOm naar het admin hoofdmenu te gaan klik op");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(" Backspace");
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
        // check if Name is string
        Console.WriteLine("Wat is de naam van de nieuwe route?");
        string? newName = Console.ReadLine();
        while (!Helper.IsOnlyLetter(newName))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"'{newName}' is geen geldige optie.");
            Console.ResetColor();
            Console.WriteLine("De naam van de route kan alleen bestaan uit letters.");
            Console.WriteLine("Wat is de naam van de nieuwe route?");
            newName = Console.ReadLine();
        }

        //checks if Duration is Int input
        Console.WriteLine("Hoelang duurt de route in uur?");
        string? newDuration = Console.ReadLine();
        while(!Helper.IsValidInteger(newDuration)) 
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"'{newDuration}' is geen geldige optie.");
            Console.ResetColor();
            Console.WriteLine("De duur van de route moet in hele getallen gegeven worden.");
            Console.WriteLine("Hoelang duurt de route in uren?");
            newDuration = Console.ReadLine();
        }

        // Makes the route
        RouteModel newRoute = new RouteModel(routeLogic.GenerateNewId(), Convert.ToInt32(newDuration), newName);
        Console.WriteLine("Nu gaat U haltes toevoegen aan de route");
        AddStopToRoute(newRoute);

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
            List<string> Header = new() {"Haltenummer", "Naam", "Tijd"};
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(duplicateStopMessage);
                    Console.ResetColor();
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
        List<string> Header = new() {"Routenummer", "Naam", "Tijdsduur(uur)", "Stops", "Begintijd", "Eindtijd"};
        List<RouteModel> routeModels = routeLogic.GetAll();
        List<StopModel> StopsList = new() {};
        if (routeModels == null || routeModels.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Lege data.");
            Console.ResetColor();
            Console.WriteLine("U keert terug naar het admin hoofd menu.\n");
            Thread.Sleep(3000);
            AdminStartMenu.Start();
        }
        else
        {
            while(true){
                (List<string> SelectedRow, int SelectedRowIndex)? TableInfo= tableRoutes.PrintTable(Header, routeModels, GenerateRow);
                if(TableInfo != null){
                    int selectedRowIndex = TableInfo.Value.SelectedRowIndex;

                    if(selectedRowIndex == routeModels.Count())
                    {
                        RouteModel newRouteModel = new(routeLogic.GenerateNewId(),0,"");
                        routeLogic.UpdateList(newRouteModel);
                        continue;
                    }
                    while(true){
                        (string SelectedItem, int SelectedIndex)? result = tableRoutes.PrintSelectedRow(TableInfo.Value.SelectedRow, Header);
                        //Console.WriteLine($"Selected Item: {result.Value.SelectedItem}, Selected Index: {result.Value.SelectedIndex}"); //#test om PrintSelectedRow functie te testen.
                        if (result != null)                        {
                            string selectedItem = result.Value.SelectedItem;
                            int selectedIndex = result.Value.SelectedIndex;
                            if (selectedIndex == 0){
                                Console.WriteLine($"U kan {Header[selectedIndex]} niet aanpassen.");
                                Thread.Sleep(3000);
                            }
                            else if(selectedIndex == 1){
                                Console.WriteLine("Voer iets in om de naam van de route te veranderen:");
                                string Input = Console.ReadLine();

                                while(!Helper.IsOnlyLetter(Input))
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"'{Input}' is geen geldige optie.");
                                    Console.ResetColor();
                                    Console.WriteLine("De naam kan alleen bestaan uit letters.");
                                    Console.WriteLine("Wat is de naam van de nieuwe route?");
                                    Input = Console.ReadLine();
                                }
        
                                routeModels[selectedRowIndex].Name = Input;
                                routeLogic.UpdateList(routeModels[selectedRowIndex]);
                                break;
                            }
                            else if(selectedIndex == 2){
                                Console.WriteLine("Voer een nummer in het item te veranderen:");
                                string Input = Console.ReadLine();
                                while (!Helper.IsValidInteger(Input))
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"'{Input}' is geen geldige optie.");
                                    Console.ResetColor();
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
                                StopsList = ListAllRoutes[selectedRowIndex].Stops.ToList();
                                AddStopToRoute(routeModels[selectedRowIndex], StopsList);
                                // while (true){
                                // vergeet niet de Helper class !!!!!!!!!!
                                // // string Input = Console.ReadLine();
                                // // bool containsOnlyNumbers = Input.All(char.IsDigit);
                                // // if (containsOnlyNumbers){
                                // //     routeModels[selectedRowIndex].Duration = Convert.ToInt32(Input);
                                // //     routeLogic.UpdateList(routeModels[selectedRowIndex]);
                                //     //break;
                                //     //}
                                 // }
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("U keert terug naar het routemenu overzicht.");
                            break;
                        }
                    }
                }
                else{
                    break;
                }
            }
        }
    
    }


    public static void AddToBus()
    {
        List<BusModel> ListAllBusses = busLogic.GetAll();
        BusMenu.ShowAllBusInformation();
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
            
            if (Helper.IsOnlyLetter(newName))
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
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{answer} is geen geldige input. Probeer het opnieuw.");
                            Console.ResetColor();
                            break;
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"'{newName}' is geen geldige optie.");
                Console.ResetColor();
                Console.WriteLine("De naam van een halte kan alleen letters zijn.");
                Console.WriteLine("Probeer het nog een keer.\n");
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

    public static RouteModel SelectRoute()
    {
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