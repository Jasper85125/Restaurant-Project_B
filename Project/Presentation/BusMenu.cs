static class BusMenu
{

    public static void Start()
    {
        Console.WriteLine("\nWelkom bij het overzicht voor prijzen.");
        Console.WriteLine("Wat wilt u doen?");
        Console.WriteLine("[1] Een bus toevoegen.");
        Console.WriteLine("[2] Een bus updaten.");
        Console.WriteLine("[3] Een overzicht van alle bussen.");
        string? input = Console.ReadLine();
        if (input != null)
        {
            switch (input)
            {
                case "1":
                    AddBus();
                    break;
                case "2":
                    // UpdatePrice();
                    break;
                case "3":
                    try{
                    BusLogic busLogic = new();
                    List<BusModel> ListAllBusses = busLogic.GetAllBusses();
                    ShowAllBusforamtion(ListAllBusses);
                    break;
                    }catch(Exception e){
                        Console.WriteLine($"Error: {e.Message}");
                    }
                    break;
                default:
                    Console.WriteLine("Invalid input");
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

    public static void AddBus()
    {

    }
    public static void UpdatePrice(BusModel price)
    {

    }
    public static void ShowPriceInformation(BusModel bus)
    {
        Console.WriteLine($"ID: {bus.Id}");
        Console.WriteLine($"Passenger: {bus.Seats}");
    }

    public static void ShowAllBusforamtion (List<BusModel> ListAllBusses)
    {
        foreach (BusModel bus in ListAllBusses)
        {
            Console.WriteLine($"ID: {bus.Id}");
            Console.WriteLine($"Seats: {bus.Seats}");
        }

    } 

    public static BusModel SearchByID()
    {
        BusLogic busLogic = new();
        Console.WriteLine("Enter an ID to search: ");
        int ID = Convert.ToInt32(Console.ReadLine());
        BusModel price = busLogic.GetById(ID);
        return price;
    }

}