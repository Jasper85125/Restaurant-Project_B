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

    static public void Welcome()
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
                    selectedOption = Math.Min(6, selectedOption + 1);
                    break;
                case ConsoleKey.Enter:
                    // Perform action based on selected option (e.g., execute corresponding function)
                    switch (selectedOption)
                    {
                        case 1:
                            Console.Clear();
                            AddRoute();
                            Welcome();
                            break;
                        case 2:
                            Console.Clear();
                            UpdateRoute();
                            Welcome();
                            break;
                        case 3:
                            Console.Clear();
                            AddToBus();
                            Welcome();
                            break;
                        case 4:
                            Console.Clear();
                            PrintedOverview();
                            MoreInformation();
                            Welcome();
                            break;
                        case 5:
                            Console.Clear();
                            MakeStop();
                            break;
                        case 6:
                            Console.Clear();
                            Menu.Start();
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
        Console.WriteLine("[1] Een route toevoegen.");

        // Display option 2
        Console.ForegroundColor = selectedOption == 2 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 2 ? ">> " : "   ");
        Console.WriteLine("[2] Een route updaten.");

        // Display option 3
        Console.ForegroundColor = selectedOption == 3 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 3 ? ">> " : "   ");
        Console.WriteLine("[3] Een route aan bus toevoegen");

        // Display option 4
        Console.ForegroundColor = selectedOption == 4 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 4 ? ">> " : "   ");
        Console.WriteLine("[4] Een overzicht van alle routes.");

        // Display option 5
        Console.ForegroundColor = selectedOption == 5 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 5 ? ">> " : "   ");
        Console.WriteLine("[5] Een halte toevoegen.");

        // Display option 6
        Console.ForegroundColor = selectedOption == 6 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 6 ? ">> " : "   ");
        Console.WriteLine("[6] Ga terug naar het vorige menu.");

        // Reset text color
        Console.ResetColor();
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
        while (newName == null || newName.All(char.IsLetter) == false)
        {
            Console.WriteLine($"{newName} is geen geldige optie.");
            Console.WriteLine("De naam van de route kan alleen bestaan uit letters.");
            Console.WriteLine("Wat is de naam van de nieuwe route?");
            newName = Console.ReadLine();
        }

        //checks if Duration is Int input
        Console.WriteLine("Hoelang duurt de route in uur?");
        string? newDuration = Console.ReadLine();
        while(newDuration == null || newDuration.All(char.IsDigit) == false)
        {
            Console.WriteLine($"{newDuration} is geen geldige optie. Gebruik Ja of Nee\n");
            Console.WriteLine("De duur van de route moet in hele getallen gegeven worden.");
            Console.WriteLine("Hoelang duurt de route in uren?");
            newDuration = Console.ReadLine();
        }

        // Makes the route
        RouteModel newRoute = new RouteModel(routeLogic.GenerateNewId(), Convert.ToInt32(newDuration), newName);
        Console.WriteLine("Nu gaat U haltes toevoegen aan de route");
        AddToRoute(newRoute);

    }

    public static void AddToRoute(RouteModel route)
    {
        if (stopLogic.GetAll() == null || stopLogic.GetAll().Count == 0)
        {
            Console.WriteLine("Er zijn geen haltes gevonden in de database.");
            Console.WriteLine("Voeg eerst een halte toe");
            MakeStop();
            AddToRoute(route);
        }
        else
        {
            List<string> Header = new() {"Haltenummer", "Naam", "Tijd"};
            List<StopModel> stopModels = stopLogic.GetAll();
            tableStops.PrintTable(Header, stopModels, GenerateRow);

            //Hier komt het toevoegen van haltes door middel van kiezen in de tabel.
            bool checkStopName = true;
            List<StopModel> stops = stopLogic.GetAll().OrderBy(stop => stop.Name).ToList();
            int selectedIndex = 0;
            int currentPage = 1;
            int pageSize = 10;
            int totalPages = (int)Math.Ceiling((double)stops.Count / pageSize);

            while (checkStopName)
            {
                Console.Clear();

                Console.WriteLine($"Naam: {route.Name}, tijdsduur: {route.Duration}.\n");

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
                Console.Write("Druk op ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Enter");
                Console.ResetColor();
                Console.Write("  om te selecteren.");

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
                        Console.WriteLine($"Geselecteerde halte: {selectedStop.Name}");
                        checkStopName = false;
                        break;
                    default:
                        Console.WriteLine("Ongeldige invoer. Probeer het opnieuw.");
                        break;
                }
            }




        }

    }

    //     bool addStopQuestion = true;
    //     while (addStopQuestion)
    //     {
    //         bool checkDuplicateStop = false;
    //         foreach (StopModel stop in newRoute.Stops)
    //         {
    //             if (stop.Name.ToLower() == newStop.Name.ToLower())
    //             {
    //                 Console.WriteLine("Deze halte bestaat al en kan daardoor niet worden toegevoegd.");
    //                 checkDuplicateStop = true;
    //             }
    //         }
    //         if (checkDuplicateStop == false)
    //         {
    //             newRoute = RouteLogic.AddToRoute(newStop, newRoute);
    //             Console.WriteLine("Wilt U nog een halte toevoegen? Ja of Nee");
    //             string? answer = Console.ReadLine();

    //             // Checks if answer is Ja of Nee
    //             bool answerCheck = false;
    //             while (answerCheck == false)
    //             {
    //                 if (answer.ToLower() == "ja")
    //                 {
    //                     answerCheck = true;
    //                 }
    //                 else if (answer.ToLower() == "nee")
    //                 {
    //                     addStopQuestion = false;
    //                     answerCheck = true;
    //                 }
    //                 else
    //                 {
    //                     Console.WriteLine($"{answer} is geen geldige optie. Gebruik Ja of Nee\n");
    //                     Console.WriteLine("Wilt U nog een halte toevoegen? Ja of Nee");
    //                     answer = Console.ReadLine();
    //                 }
    //             }
    //         }              
    //     }
    //     routeLogic.UpdateList(newRoute);
    //     Console.WriteLine($"\nDit is uw nieuwe toegevoegde route");
    //     List<RouteModel> routeModel = new() {newRoute};
    //     List<string> Header = new() {"Routenummer", "Naam","Tijdsduur", "Stops", "Begintijd", "Eindtijd"};
    //     if (routeModel == null || routeModel.Count == 0)
    //     {
    //         Console.WriteLine("Lege data.");
    //     }
    //     else
    //     {
    //         tableRoutes.PrintTable(Header, routeModel, GenerateRow);
    //     }
    // }

    public static void UpdateRoute()
    {
            Console.WriteLine("Welk van deze IDs zou u willen updaten.");
            foreach (RouteModel Route in routeLogic.GetAll())
            {
                Console.WriteLine($"Het Route ID is {Route.Id} en de duur van de rit is {Route.Duration} uur.");
            }
            string? id_to_be_updated = Console.ReadLine();
            try
            {
                RouteModel RouteObject = routeLogic.GetById(Convert.ToInt32(id_to_be_updated));
                if (RouteObject == null)
                {
                    Console.WriteLine("Dat is geen geldig ID!");
                }
                else
                {
                    Console.WriteLine("Wat is de duur van de route?");
                    string? new_duration = Console.ReadLine();
                    RouteObject.Duration = Convert.ToInt32(new_duration);
                    routeLogic.UpdateList(RouteObject);
                    Console.WriteLine("Route is geüpdatet");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Geen gelidg input");
            }
    }

    public static void PrintedOverview()
    { 
        List<string> Header = new() {"Routenummer", "Naam", "Tijdsduur", "Stops", "Begintijd", "Eindtijd"};
        List<RouteModel> routeModels = routeLogic.GetAll();
        if (routeModels == null || routeModels.Count == 0)
        {
            Console.WriteLine("Lege data.");
        }
        else
        {
            tableRoutes.PrintTable(Header, routeModels, GenerateRow);
        }
    
    }

    public static void MoreInformation()
    {
        Console.WriteLine("Wilt u meer informatie over een route J/N");
        string? answer = Console.ReadLine();
        if (answer.ToLower() == "j" || answer.ToLower() == "n")
        {
            if (answer.ToLower() == "j")
            {
                Console.WriteLine("Over welke route wilt u meer informatie?");
                string? idMoreInfo = Console.ReadLine();
                try
                {
                    RouteModel RouteObject = routeLogic.GetById(Convert.ToInt32(idMoreInfo));
                    if (RouteObject == null)
                    {
                        Console.WriteLine("Dat is geen geldig ID!");
                        MoreInformation();
                    }
                    else
                    {
                        Console.WriteLine($"Route ID {RouteObject.Id} bevat de volgende haltes:");
                        foreach (StopModel Stop in RouteObject.Stops)
                        {
                            Console.WriteLine($"[{Stop.Name}]");
                        }
                        Thread.Sleep(3000);
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Geen gelidg input");
                }
            }
            else
            {
                Console.WriteLine("U wordt terug gezet naar het vorige menu.");
            }
        }
        else
        {
            Console.WriteLine("Verkeerde input. type y of n.");
            MoreInformation();
        }
    }

    public static void AddToBus()
    {
        List<BusModel> ListAllBusses = busLogic.GetAll();
        BusMenu.ShowAllBusInformation(ListAllBusses);
        Console.WriteLine("\nAan welke van deze bussen wilt u een route toevoegen?");
        string? inputBus = Console.ReadLine();
        try
        {
            int intInputBus = Convert.ToInt32(inputBus);
            BusModel bus = busLogic.GetById(intInputBus);
            if (BusMenu.HasRoute(bus))
            {
                Console.WriteLine("Deze bus heeft al een aangewezen route.");
                Welcome();
            }
            PrintedOverview();
            Console.WriteLine($"Welke route wilt u toevoegen aan de bus met ID: {bus.Id}?");
            string? inputRoute = Console.ReadLine();
            try
            {
                int intInputRoute = Convert.ToInt32(inputRoute);
                RouteModel routeModel = routeLogic.GetById(intInputRoute);
                if (routeModel == null)
                {
                    Console.WriteLine("Verkeerde input. Er bestaat geen  met dat ID.");
                    AddToBus();
                }
                else
                {

                    bus.AddRoute(routeModel);
                    busLogic.UpdateList(bus);
                }
            }
            catch
            {
                Console.WriteLine("Verkeerde input. Probeer het nog een keer.");
            }
        }
        catch (Exception)
        {
            Console.WriteLine($"Verkeerde input. Probeer het nog een keer.");
            AddToBus();
        }
    }

    public static void MakeStop()
    {
        bool checkStopName = true;
        while (checkStopName)
        {
            Console.WriteLine("Wat is de naam van de halte?");
            string? newName = Console.ReadLine();
            if (newName != null && newName.All(char.IsLetter))
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
                            break;
                        default:
                            Console.WriteLine($"{answer} is geen geldige input. Probeer het opnieuw.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("De naam van een halte kan alleen letters zijn.");
                Console.WriteLine("Probeer het nog een keer.");
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
            int selectedRowIndex = TableInfo.Value.SelectedRowIndex;
            return routeModels[selectedRowIndex];
        }
    }
}