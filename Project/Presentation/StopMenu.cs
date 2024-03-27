public class StopMenu
{
    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("\nWelkom bij het overzicht voor tussenstops.");
        Console.WriteLine("Wat wilt u doen?");
        Console.WriteLine("[1] Een tussenstop toevoegen.");
        Console.WriteLine("[2] Een tussenstop aan een route koppelen.");
        Console.WriteLine("[3] Een overzicht van alle tussenstops.");
        Console.WriteLine("[4] Ga terug naar het vorige menu.");

        string? input = Console.ReadLine();
        if (input == null)
        {
            Console.WriteLine("Verkeerde input!");
            StopMenu.Start();
        }
        else
        {
            StopMenu.Choice(input);
        }
    }

    static public void Choice (string input)
    {
        switch (input)
        {
            case "1":
                Console.Clear();
                StopMenu.AddStop();
                Start();
                break;
        
            case "2":
                Console.Clear();
                StopMenu.AddToRoute();
                Start();
                break;
            case "3":
                Console.Clear();
                PrintedOverview();
                Start();
                break;
            case "4":
                Console.Clear();
                RouteMenu.Welcome();
                break;
            default:
                Console.WriteLine($"\nDat is geen geldige opties. Let goed op u keuze.");
                StopMenu.Start();
                break;
        }
    }

    public static List<StopModel> Overview()
    {
        StopLogic LogicInstance = new StopLogic();
        List<StopModel> overview = LogicInstance.GetAllStops();
        return overview;
    }

    public static void PrintedOverview()
    {
        List<StopModel> overview = Overview();
            foreach (StopModel stop in overview)
            {
                Console.WriteLine($"Het Stop ID is {stop.Id} en de naam van de stop is {stop.Name}.");
            }
    }

    public static void AddStop()
    {
        List<StopModel> listOfStops = Overview();
        int intID = listOfStops.Count();
        int newID = intID + 1;
        Console.WriteLine("Wat is de naam van de tussenstop?");
        string? newName = Console.ReadLine();
        try
        {
            StopModel newStop = new StopModel(Convert.ToInt32(newID), Convert.ToString(newName));
            StopLogic newLogic = new StopLogic();
            newLogic.UpdateList(newStop);
        }
        catch (FormatException)
        {
            Console.WriteLine("Verkeerde input. Probeer het nog een keer.");
            AddStop();
        }
    }

    public static void AddToRoute()
    {
        RouteMenu.PrintedOverview();
        Console.WriteLine("\nAan welke van deze routes wilt u een stop toevoegen?");
        string? inputRoute = Console.ReadLine();
        try
        {
            RouteLogic LogicInstance = new RouteLogic ();
            int intInputRoute = Convert.ToInt32(inputRoute);
            RouteModel route = LogicInstance.GetById(intInputRoute);
            PrintedOverview();
            Console.WriteLine($"Welke stop wilt u toevoegen aan de route met ID: {route.Id}?");
            string? inputStop = Console.ReadLine();
            try
            {
                int intInputStop = Convert.ToInt32(inputStop);
                StopLogic stopLogicInstance = new StopLogic ();
                StopModel stopModel = stopLogicInstance.GetById(intInputStop);
                if (stopModel == null)
                {
                    Console.WriteLine("Verkeerde input. Er bestaat geen stop met dat ID.");
                    AddToRoute();
                }
                else
                {

                    route.AddStop(intInputStop);
                    LogicInstance.UpdateList(route);
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
            AddToRoute();
        }
    }
}