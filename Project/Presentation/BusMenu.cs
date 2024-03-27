static class BusMenu
{

    public static void Start()
    {
        Console.WriteLine("\nWelkom bij het overzicht voor de bussen.");
        Console.WriteLine("Wat wilt u doen?");
        Console.WriteLine("[1] Een bus toevoegen.");
        Console.WriteLine("[2] Een bus updaten.");
        Console.WriteLine("[3] Een overzicht van alle bussen.");
        Console.WriteLine("[4] Ga terug naar het vorige menu.");
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
                    BusLogic busLogic = new();
                    List<BusModel> ListAllBusses = busLogic.GetAllBusses();
                    ShowAllBusforamtion(ListAllBusses);
                    Start();
                    break;
                case "4":
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
        List<BusModel> overview = LogicInstance.GetAllBusses();
        return overview;
    }

    public static void AddBus()
    {
        List<BusModel> list_of_busses = Overview();
            int int_id = list_of_busses.Count();
            int new_id = int_id + 1;
            Console.WriteLine("Hoeveel zitplaatsen heef deze bus?");
            string? new_seats = Console.ReadLine();
            Console.WriteLine("Wat is de kenteken van de bus?");
            string? new_licensePlate = Console.ReadLine();
            try
            {
                BusModel new_bus = new BusModel(Convert.ToInt32(new_id), Convert.ToInt32(new_seats), Convert.ToString(new_licensePlate));
                BusLogic new_logic = new BusLogic();
                new_logic.UpdateList(new_bus);
                Console.WriteLine("Bus is succesvol toegevoegd!")
                ;System.Threading.Thread.Sleep(3000);
                Console.Clear();
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input! Please try again.");
                ;System.Threading.Thread.Sleep(3000);
                Console.Clear();
            }
    }
    public static void UpdateBus()
    {
        BusLogic loading = new BusLogic();
        Console.Clear();
        Console.WriteLine("Which of these IDs would you like to update.");
        foreach (BusModel Bus in loading.GetAllBusses())
        {
            Console.WriteLine($"ID: {Bus.Id},\nLicense plate: {Bus.LicensePlate},\nNumber of seats: {Bus.Seats}\n");
        }
        string? id_to_be_updated = Console.ReadLine();
        try
        {
            BusModel RouteObject = loading.GetById(Convert.ToInt32(id_to_be_updated));
            if (RouteObject == null)
            {
                Console.WriteLine("That's not a valid id!");
                ;System.Threading.Thread.Sleep(3000);
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
                    Console.WriteLine("Bus license plate has been updated")
                    ;System.Threading.Thread.Sleep(3000);
                    Console.Clear();
                }
                else if (option == "2")
                {
                    Console.WriteLine("What is the new number of seats for the bus?");
                    string? new_seats = Console.ReadLine();
                    RouteObject.Seats = Convert.ToInt32(new_seats); // Update the number of seats
                    Console.WriteLine("Bus seats have been updated");
                    ;System.Threading.Thread.Sleep(3000);
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Invalid option selected.");
                    ;System.Threading.Thread.Sleep(3000);
                    Console.Clear();
                }
                
                loading.UpdateList(RouteObject);
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Not a valid input");
            ;System.Threading.Thread.Sleep(3000);
            Console.Clear();
        }
    }
    public static void ShowAllBusforamtion (List<BusModel> ListAllBusses)
    {
        foreach (BusModel bus in ListAllBusses)
        {
            Console.WriteLine($"Bus number: {bus.Id}");
            Console.WriteLine($"License plate: {bus.LicensePlate}");
            Console.WriteLine($"Total seats: {bus.Seats}\n");
        }

    } 

    public static BusModel SearchByID()
    {
        BusLogic busLogic = new();
        Console.WriteLine("Enter an ID to search: ");
        int ID = Convert.ToInt32(Console.ReadLine());
        BusModel bus = busLogic.GetById(ID);
        return bus;
    }

}