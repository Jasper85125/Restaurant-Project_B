using System.Data.Common;
using System.Runtime.InteropServices;

static public class RouteMenu
{
    static public void Welcome()
    {
        Console.WriteLine("\nWelkom bij het overzicht voor routes.");
        Console.WriteLine("Wat wilt u doen?");
        Console.WriteLine("[1] Een route toevoegen.");
        Console.WriteLine("[2] Een route updaten.");
        Console.WriteLine("[3] Een overzicht van alle routes.");
        Console.WriteLine("[4] ga naar menu voor tussenstops.");
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
    
    static public void Choice (string input)
    {
        switch (input)
        {
            case "1":
                RouteMenu.AddRoute();
                Welcome();
                break;
            case "2":
                RouteMenu.UpdateRoute();
                Welcome();
                break;
            case "3":
                RouteMenu.PrintedOverview();
                Welcome();
                break;
            case "4":
                StopMenu.Start();
                break;
            case "5":
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
            Console.WriteLine("Hoelang duurt de route in uur?");
            string? new_duration = Console.ReadLine();
            try
            {
                RouteModel new_route = new RouteModel(Convert.ToInt32(new_id), Convert.ToInt32(new_duration));
                RouteLogic new_logic = new RouteLogic();
                new_logic.UpdateList(new_route);
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
                Console.WriteLine($"Het route ID is {Route.Id} en de duur van de rit is {Route.Duration} uur.");
            }
    }
}