using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public static class RouteMenu
{
    private static RouteLogic routesLogic = new();
    private static TableLogic<RouteModel> tableRoutes = new();

    static public void Welcome()
    {
        Console.WriteLine("\nWelkom bij het overzicht voor routes.\n");
        Console.WriteLine("Wat wilt u doen?");
        Console.WriteLine("[1] Een route toevoegen.");
        Console.WriteLine("[2] Een route updaten.");
        Console.WriteLine("[3] Een route aan bus toevoegen");
        Console.WriteLine("[4] Een overzicht van alle routes.");
        Console.WriteLine("[5] Ga terug naar het vorige menu.");
        
        string? input = Console.ReadLine();
        if (input == null)
        {
            Console.WriteLine("Verkeerde input!");
            RouteMenu.Welcome();
        }
        else
        {
            RouteMenu.Choice(input);
        }
    }
    
    public static void Choice (string input)
    {
        switch (input)
        {
            case "1":
                Console.Clear();
                AddRoute();
                Welcome();
                break;
            case "2":
                Console.Clear();
                UpdateRoute();
                Welcome();
                break;
            case "3":
                Console.Clear();
                AddToBus();
                Welcome();
                break;
            case "4":
                Console.Clear();
                PrintedOverview();
                MoreInformation();
                Welcome();
                break;
            case "5":
                Console.Clear();
                Menu.Start();
                break;
            default:
                Console.WriteLine($"\n{input} is geen geldige opties. Let goed op u keuze.");
                RouteMenu.Welcome();
                break;
        }
    }

    public static List<RouteModel> Overview()
    {
        RouteLogic LogicInstance = new RouteLogic();
        List<RouteModel> overview = LogicInstance.GetAll();
        return overview;
    }

    public static void AddRoute()
    {
        List<RouteModel> list_of_routes = Overview();

        // check if Name is string
        Console.WriteLine("Wat is de naam van de nieuwe route?");
        string? newName = Console.ReadLine();
        while (newName == null || newName.All(char.IsLetter) == false)
        {
            Console.WriteLine("De naam van de route kan alleen bestaan uit letters.");
            Console.WriteLine("Wat is de naam van de nieuwe route?");
            newName = Console.ReadLine();
        }
        
        // checks if Duration is Int input
        Console.WriteLine("Hoelang duurt de route in uur?");
        string? newDuration = Console.ReadLine();
        while(newDuration == null || newDuration.All(char.IsDigit) == false)
        {
            Console.WriteLine("De duur van de route moet in hele getallen gegeven worden.");
            Console.WriteLine("Hoelang duurt de route in uur?");
            newDuration = Console.ReadLine();
        }
        
        // Makes the route
        RouteLogic newLogic = new RouteLogic();
        RouteModel newRoute = new RouteModel(newLogic.GenerateNewId(), Convert.ToInt32(newDuration), newName);
        Console.WriteLine("Nu gaat u haltes toevoegen aan de route");
        bool addStopQuestion = true;
        while (addStopQuestion)
        {
            StopModel newStop = StopMenu.MakeStop();
            bool checkDuplicateStop = false;
            foreach (StopModel stop in newRoute.Stops)
            {
                if (stop.Name.ToLower() == newStop.Name.ToLower())
                {
                    Console.WriteLine("Deze halte bestaat al en kan daardoor niet worden toegevoegd.");
                    checkDuplicateStop = true;
                }
            }
            if (checkDuplicateStop == false)
            {
                newRoute = StopMenu.AddToRoute(newStop, newRoute);
                Console.WriteLine("Wilt U nog een halte toevoegen? Ja of Nee");
                string? answer = Console.ReadLine();

                // Checks if answer is Ja of Nee
                bool answerCheck = false;
                while (answerCheck == false)
                {
                    if (answer.ToLower() == "ja")
                    {
                        answerCheck = true;
                    }
                    else if (answer.ToLower() == "nee")
                    {
                        addStopQuestion = false;
                        answerCheck = true;
                    }
                    else
                    {
                        Console.WriteLine($"{answer} is geen geldige optie. Gebruik Ja of Nee\n");
                        Console.WriteLine("Wilt U nog een halte toevoegen? Ja of Nee");
                        answer = Console.ReadLine();
                    }
                }
            }              
        }
        newLogic.UpdateList(newRoute);
        Console.WriteLine($"\nDit is uw nieuwe toegevoegde route");
        List<RouteModel> routeModel = new() {newRoute};
        List<string> Header = new() {"Id", "Duration", "Name", "Stops", "Begintijd", "Eindtijd"};
        if (routeModel == null || routeModel.Count == 0)
        {
            Console.WriteLine("Lege data.");
        }
        else
        {
            tableRoutes.PrintTable(Header, routeModel, GenerateRow);
        }
    }

    public static void UpdateRoute()
    {
        RouteLogic loading = new RouteLogic();
            Console.WriteLine("Welk van deze IDs zou u willen updaten.");
            foreach (RouteModel Route in loading.GetAll())
            {
                Console.WriteLine($"Het Route ID is {Route.Id} en de duur van de rit is {Route.Duration} uur.");
            }
            string? id_to_be_updated = Console.ReadLine();
            try
            {
                RouteModel RouteObject = loading.GetById(Convert.ToInt32(id_to_be_updated));
                if (RouteObject == null)
                {
                    Console.WriteLine("Dat is geen geldig ID!");
                }
                else
                {
                    Console.WriteLine("Wat is de duur van de route?");
                    string? new_duration = Console.ReadLine();
                    RouteObject.Duration = Convert.ToInt32(new_duration);
                    loading.UpdateList(RouteObject);
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
        List<string> Header = new() {"Id", "Duration", "Name", "Stops", "BeginTijd", "EindTijd"};
        List<RouteModel> routeModels = routesLogic.GetAll();
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
        RouteLogic loading = new RouteLogic();
        Console.WriteLine("Wilt u meer informatie over een route Y/N");
        string? answer = Console.ReadLine();
        if (answer.ToLower() == "y" || answer.ToLower() == "n")
        {
            if (answer.ToLower() == "y")
            {
                Console.WriteLine("Over welke route wilt u meer informatie?");
                string? idMoreInfo = Console.ReadLine();
                try
                {
                    RouteModel RouteObject = loading.GetById(Convert.ToInt32(idMoreInfo));
                    if (RouteObject == null)
                    {
                        Console.WriteLine("Dat is geen geldig ID!");
                        MoreInformation();
                    }
                    else
                    {
                        Console.WriteLine($"Route ID {RouteObject.Id} bevat de volgende haltes.");
                        foreach (StopModel Stop in RouteObject.Stops)
                        {
                            Console.WriteLine($"{Stop.Name}");
                        }
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
        BusMenu.ShowAllBusInformation();
        Console.WriteLine("\nAan welke van deze bussen wilt u een route toevoegen?");
        string? inputBus = Console.ReadLine();
        try
        {
            BusLogic LogicInstance = new BusLogic ();
            int intInputBus = Convert.ToInt32(inputBus);
            BusModel bus = LogicInstance.GetById(intInputBus);
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
                RouteLogic routeLogicInstance = new RouteLogic ();
                RouteModel routeModel = routeLogicInstance.GetById(intInputRoute);
                if (routeModel == null)
                {
                    Console.WriteLine("Verkeerde input. Er bestaat geen  met dat ID.");
                    AddToBus();
                }
                else
                {

                    bus.AddRoute(routeModel);
                    LogicInstance.UpdateList(bus);
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
        return new List<string> { $"{id}", $"{duration}", $"{name}", stopsString, $"{beginTime}", $"{endTime}" };
    }
}