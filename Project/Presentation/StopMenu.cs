public class StopMenu
{
    public static void Start()
    {
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
                StopMenu.AddStop();
                Start();
                break;
        
            case "2":
                //StopMenu.UpdateStops();
                Start();
                break;
            case "3":
                List<StopModel> overview = Overview();
                foreach (StopModel stop in overview)
                {
                    Console.WriteLine($"The Stop ID is {stop.Id} and the name of the stop is {stop.Name}.");
                }
                Start();
                break;
            case "4":
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
            Console.WriteLine("Invalid input! Please try again.");
        }
    }
}