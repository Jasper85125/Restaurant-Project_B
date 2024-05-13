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
                    selectedOption = Math.Min(2, selectedOption + 1);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    // Perform action based on selected option (e.g., execute corresponding function)
                    switch (selectedOption)
                    {
                        case 1:
                            PrintedOverview();
                            MoreInformation();
                            Welcome();
                            break;
                        case 2:
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

        // Display option 4
        Console.ForegroundColor = selectedOption == 1 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 1 ? ">> " : "   ");
        Console.WriteLine("[1] Een overzicht van alle routes.");

        // Display option 6
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

    public static bool IsString(string parameter)
    {
        if (Helper.IsString(parameter))
        {
            return true;
        }
        else
        {
            return false;
        } 
    }

    public static bool IsInt(string parameter)
    {
        if (Helper.IsInteger(parameter))
        {
            return true;
        }
        else
        {
            return false;
        } 
    }

    public static bool IsDouble(string parameter)
    {
        if (Helper.IsDouble(parameter))
        {
            return true;
        }
        else
        {
            return false;
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