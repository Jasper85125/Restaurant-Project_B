using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public static class RouteMenu
{
    static public void Welcome()
    {
        Console.WriteLine("\nWelkom bij het overzicht voor routes.\n");
        Console.WriteLine("Wat wilt u doen?");
        Console.WriteLine("[1] Een route toevoegen.");
        Console.WriteLine("[2] Een route updaten.");
        Console.WriteLine("[3] Een route aan bus toevoegen");
        Console.WriteLine("[4] Een overzicht van alle routes.");
        Console.WriteLine("[5] ga naar menu voor tussenstops.");
        Console.WriteLine("[6] Ga terug naar het vorige menu.");
        
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
                StopMenu.Start();
                break;
            case "6":
                Console.Clear();
                Menu.Start();
                break;
            default:
                Console.WriteLine($"\nDat is geen geldige opties. Let goed op u keuze.");
                RouteMenu.Welcome();
                break;
        }
    }

    public static List<RouteModel> Overview()
    {
        RouteLogic LogicInstance = new RouteLogic();
        List<RouteModel> overview = LogicInstance.GetAllRoutes();
        return overview;
    }

    public static void AddRoute()
    {
        List<RouteModel> list_of_routes = Overview();
        int int_id = list_of_routes.Count();
        int new_id = int_id + 1;
        Console.WriteLine("Wat is de naam van de nieuwe route?");
        string? newName = Console.ReadLine();
        while (newName == null)
        {
            Console.WriteLine("Wat is de naam van de nieuwe route?");
            newName = Console.ReadLine();
        }
        Console.WriteLine("Hoelang duurt de route in uur?");
        string? new_duration = Console.ReadLine();
        try
        {
            RouteModel newRoute = new RouteModel(Convert.ToInt32(new_id), Convert.ToInt32(new_duration), newName);
            RouteLogic newLogic = new RouteLogic();
            newLogic.UpdateList(newRoute);
        }
        catch (FormatException)
        {
            Console.WriteLine("Verkeerde input. Probeer het nog eens.");
        }
    }

    public static void UpdateRoute()
    {
        RouteLogic loading = new RouteLogic();
            Console.WriteLine("Welk van deze IDs zou u willen updaten.");
            foreach (RouteModel Route in loading.GetAllRoutes())
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
        List<RouteModel> overview = RouteMenu.Overview();
            foreach (RouteModel Route in overview)
            {
                Console.WriteLine($"\nHet route ID is {Route.Id}. De naam van de route is {Route.Name} en de duur van de rit is {Route.Duration} uur.");
                Console.WriteLine($"De route heeft {Route.Stops.Count()} tussenstops");
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
        BusLogic busLogic = new();
        List<BusModel> ListAllBusses = busLogic.GetAllBusses();
        BusMenu.ShowAllBusforamtion(ListAllBusses);
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
}