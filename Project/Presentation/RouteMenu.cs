using System.Data.Common;

static public class RouteMenu
{
    static public void Welcome()
    {
        Console.WriteLine("\nWelkom bij het overzicht voor routes.");
        Console.WriteLine("Wat wilt u doen?");
        Console.WriteLine("[1] Een route toevoegen.");
        Console.WriteLine("[2] Een route aan een bus geven.");
        Console.WriteLine("[3] Een route updaten.");
        Console.WriteLine("[4] Een overzicht van alle routes.");
        
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
        if (input == "1")
        {
            List<RouteModel> list_of_routes = Overview();
            int int_id = list_of_routes.Count();
            int new_id = int_id++;
            Console.WriteLine("Hoelang duurt de route in uur?");
            string? new_duration = Console.ReadLine();
            RouteModel new_route = new RouteModel(Convert.ToInt32(new_id), Convert.ToInt32(new_duration));
            RouteLogic new_logic = new RouteLogic();
            new_logic.UpdateList(new_route);
            Welcome();
        }
        else if (input == "2")
        {
            Console.WriteLine("Waiting for bus code to be finished");
        }
        else if (input == "3")
        {
            RouteLogic loading = new RouteLogic();
            Console.WriteLine("Which of these IDs would you like to update.");
            foreach (RouteModel Route in loading.GetAllRoutes())
            {
                Console.WriteLine($"The Route ID is {Route.Id} and the duration of the trip is {Route.Duration} hours");
            }
            string? id_to_be_updated = Console.ReadLine();
            if (id_to_be_updated is int)
            {
                RouteModel RouteObject = loading.GetById(Convert.ToInt32(id_to_be_updated));
                if (RouteObject == null)
                {
                    Console.WriteLine("That's not a valid id!");
                    Welcome();
                }
                else
                {
                    Console.WriteLine("What is the new duration of the route?");
                    string? new_duration = Console.ReadLine();
                    RouteObject.Duration = Convert.ToInt32(new_duration);
                    loading.UpdateList(RouteObject);
                    Console.WriteLine("Route has been updated");
                    Welcome();
                }
            }
            else
            {
                Console.WriteLine("Not an valid input");
                Welcome();
            }
        }
        else if (input == "4")
        {
            List<RouteModel> overview = RouteMenu.Overview();
            foreach (RouteModel Route in overview)
            {
                Console.WriteLine($"The Route ID is {Route.Id} and the duration of the trip is {Route.Duration} hours");
            }
            Welcome();
        }
        else
        {
            Console.WriteLine($"\nDat is geen geldige opties. Let goed op u keuze.");
            RouteMenu.Welcome();
        }
    }

    public static List<RouteModel> Overview()
    {
        RouteLogic LogicInstance = new RouteLogic();
        List<RouteModel> overview = LogicInstance.GetAllRoutes();
        return overview;
    }
}