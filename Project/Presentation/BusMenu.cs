public static class BusMenu
{

    public static void Start()
    {
        Console.WriteLine("\nWelkom bij het overzicht voor de bussen.");
        Console.WriteLine("Wat wilt u doen?");
        Console.WriteLine("[1] Een bus toevoegen.");
        Console.WriteLine("[2] Een bus updaten.");
        Console.WriteLine("[3] Tijden toevoegen aan haltes en routes.");
        Console.WriteLine("[4] Een overzicht van alle bussen.");
        Console.WriteLine("[5] Ga terug naar het vorige menu.");
        string? input = Console.ReadLine();
        if (input != null)
        {
            switch (input)
            {
                case "1":
                    Console.Clear();
                    AddBus();
                    Start();
                    break;
                case "2":
                    Console.Clear();
                    UpdateBus();
                    Start();
                    break;
                case "3":
                    Console.Clear();
                    AddTime();
                    Start();
                    break;
                case "4":
                    Console.Clear();
                    BusLogic busLogic = new();
                    List<BusModel> ListAllBusses = busLogic.GetAll();
                    ShowAllBusforamtion(ListAllBusses);
                    Start();
                    break;
                case "5":
                    Console.Clear();
                    Menu.Start();
                    break;
                default:
                    Console.WriteLine("Verkeerde input!");
                    Start();
                    break;
            }
        }

        else
        {
            Console.WriteLine("Verkeerde input!");
            Start();
        }
    

    }

    public static List<BusModel> Overview()
    {
        BusLogic LogicInstance = new BusLogic();
        List<BusModel> overview = LogicInstance.GetAll();
        return overview;
    }

    public static void AddBus()
    {
        List<BusModel> list_of_busses = Overview();
            Console.WriteLine("Hoeveel zitplaatsen heeft deze bus?");
            string? new_seats = Console.ReadLine();
            Console.WriteLine("Wat is de kenteken van de bus?");
            string? new_licensePlate = Console.ReadLine();
            try
            {
                BusLogic new_logic = new BusLogic();
                BusModel new_bus = new BusModel(new_logic.GenerateNewId(), Convert.ToInt32(new_seats), new_licensePlate);
                new_logic.UpdateList(new_bus);
                Console.WriteLine("Bus is succesvol toegevoegd!");
                Thread.Sleep(3000);
                Console.Clear();
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input! Please try again.");
                Thread.Sleep(3000);
                Console.Clear();
            }
    }

    public static void UpdateBus()
    {
        BusLogic loading = new BusLogic();
        Console.Clear();
        Console.WriteLine("Which of these IDs would you like to update.");
        foreach (BusModel Bus in loading.GetAll())
        {
            Console.WriteLine($"ID: {Bus.Id},\nLicense plate: {Bus.LicensePlate},\nNumber of seats: {Bus.Seats}.");
        }
        string? id_to_be_updated = Console.ReadLine();
        try
        {
            BusModel RouteObject = loading.GetById(Convert.ToInt32(id_to_be_updated));
            if (RouteObject == null)
            {
                Console.WriteLine("That's not a valid id!");
                Thread.Sleep(3000);
                Console.Clear();
            }
            else
            {   
                Console.Clear();
                Console.WriteLine("What do you want to update?\n1. License Plate\n2. Number of Seats");
                string? option = Console.ReadLine();
                
                if (option == "1")
                {
                    Console.WriteLine("What is the new license plate of the bus?");
                    string? new_license_plate = Console.ReadLine();
                    RouteObject.LicensePlate = new_license_plate; // Update the license plate
                    Console.WriteLine("Bus license plate has been updated");
                    Thread.Sleep(3000);
                    Console.Clear();
                }
                else if (option == "2")
                {
                    Console.WriteLine("What is the new number of seats for the bus?");
                    string? new_seats = Console.ReadLine();
                    RouteObject.Seats = Convert.ToInt32(new_seats); // Update the number of seats
                    Console.WriteLine("Bus seats have been updated");
                    Thread.Sleep(3000);
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Invalid option selected.");
                    Thread.Sleep(3000);
                    Console.Clear();
                    UpdateBus();
                }
                
                loading.UpdateList(RouteObject);
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Not a valid input");
            Thread.Sleep(3000);
            Console.Clear();
        }
    }
    public static void ShowAllBusforamtion (List<BusModel> ListAllBusses)
    {
        foreach (BusModel bus in ListAllBusses)
        {
            Console.WriteLine($"Bus ID: {bus.Id}");
            Console.WriteLine($"Kenteken: {bus.LicensePlate}");
            Console.WriteLine($"Beschikbare plekken: {bus.Seats}");
            Console.WriteLine($"Heeft een route: {HasRoute(bus)}\n");
        }

    } 

    /// Voor deze functies moet later nog een string format checker worden geschreven.
    public static void AddTime()
    {
        ShowAllBusforamtion(Overview());
        Console.WriteLine("Aan welke bus met route wilt u een tijd geven?");
        string? busID = Console.ReadLine();
        try
        {
            BusLogic LogicInstance = new BusLogic ();
            int intInputBus = Convert.ToInt32(busID);
            BusModel bus = LogicInstance.GetById(intInputBus);
            if (HasRoute(bus))
            {
                foreach (RouteModel Route in bus.Route)
                {
                    Console.WriteLine("Wat is de begin tijd voor de route?");
                    string? beginTimeRoute = Console.ReadLine();
                    while (beginTimeRoute == null)
                    {
                        Console.WriteLine("Vul een correcte waarde in");
                    }
                    Route.beginTime = beginTimeRoute;

                    foreach (StopModel Stop in Route.Stops)
                    {
                        Console.WriteLine($"Wat is de tijd voor halte {Stop.Name}");
                        string? newTime = Console.ReadLine();
                        while (newTime == null)
                        {
                            Console.WriteLine("Vul een correcte waarde in");
                        }
                        Stop.Time = newTime;
                    }

                    Console.WriteLine("Wat is de eindtijd voor de route?");
                    string? endTimeRoute = Console.ReadLine();
                    while (endTimeRoute == null)
                    {
                        Console.WriteLine("Vul een correcte waarde in");
                    }
                    Route.endTime = endTimeRoute;
                }
                LogicInstance.UpdateList(bus);
            }
            else
            {
                Console.WriteLine("Deze bus heeft nog geen route aangewezen gekregen.");
                Start();
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Verkeerde input ");
        }

    }

    public static bool HasRoute(BusModel bus)
    {
        if (bus.Route.Count() == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}