static class PriceMenu
{
    static private PriceLogic pricesLogic = new();

    public static void Start()
    {
        Console.WriteLine("\nWelkom bij het overzicht voor prijzen.");
        Console.WriteLine("Wat wilt u doen?");
        Console.WriteLine("[1] Een prijs categorie toevoegen.");
        Console.WriteLine("[2] Een prijs categorie updaten.");
        Console.WriteLine("[3] Een overzicht van alle prijscategorieën.");
        string? input = Console.ReadLine();
        if (input != null)
        {
            switch (input)
            {
                case "1":
                    AddPrice();
                    break;
                case "2":
                    // UpdatePrice();
                    break;
                case "3":
                    List<PriceModel> ListAllPrices = pricesLogic.Prices;
                    ShowAllPriceInforamtion(ListAllPrices);
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
    
        // AccountModel acc = accountsLogic.CheckLogin(email, password);
        // if (acc != null)
        // {
        //     Console.WriteLine("Welcome back " + acc.FullName);
        //     Console.WriteLine("Your email number is " + acc.EmailAddress);

        //     //Write some code to go back to the menu
        //     //Menu.Start();
        // }
        // else
        // {
        //     Console.WriteLine("No account found with that email and password");
        // }

    }

    public static void AddPrice()
    {

    }
    public static void UpdatePrice(PriceModel price)
    {

    }
    public static void ShowPriceInformation(PriceModel price)
    {
        Console.WriteLine($"ID: {price.ID}");
        Console.WriteLine($"Passenger: {price.Passenger}");
        Console.WriteLine($"Price: {price.Price}");
    }

    public static void ShowAllPriceInforamtion (List<PriceModel> listAllPrices)
    {
        foreach (PriceModel price in listAllPrices)
        {
            Console.WriteLine($"ID: {price.ID}");
            Console.WriteLine($"Passenger: {price.Passenger}");
            Console.WriteLine($"Price: {price.Price}");
        }

    } 

    public static PriceModel SearchByID()
    {
        Console.WriteLine("Enter an ID to search: ");
        int ID = Convert.ToInt32(Console.ReadLine());
        PriceModel price = pricesLogic.GetById(ID);
        return price;
    }

}