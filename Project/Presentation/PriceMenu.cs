using System.Formats.Asn1;

static class PriceMenu
{
    static private PriceLogic pricesLogic = new();

    public static void Start()
    {
        Console.WriteLine("\nWelkom bij het overzicht voor prijzen.");
        Console.WriteLine("Wat wilt u doen?");
        Console.WriteLine("[1] Een prijs categorie toevoegen.");
        Console.WriteLine("[2] Een prijs categorie Editen.");
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
                    EditPrice();
                    break;
                case "3":
                    ShowAllPricesInforamtion();

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
        Console.WriteLine("Hieronder kunt u de huidige prijscategorieën zien");
        ShowAllPricesInforamtion();
        Console.WriteLine("Wilt u nog een prijs categorie toevoegen? Y/N: ");
        string? answer = Console.ReadLine();

        if (answer!= null && answer.ToLower() == "n")
        {
            Console.WriteLine("Uw antwoord is nee. \nU keert terug naar het startscherm.\n");
            Menu.Start();
        }
        else if (answer!= null && answer.ToLower() != "y")
        {
            Console.WriteLine("Verkeerde input!");
            AddPrice();
        }

        Console.WriteLine("Voer de naam van het nieuwe passenger type in: ");
        string? passenger = Console.ReadLine();
        Console.WriteLine("Bepaal de prijs voor de nieuwe passenger: ");
        double price = Convert.ToDouble(Console.ReadLine());

        PriceModel NewPriceCategory = new (pricesLogic.GenerateNewId(), passenger, price);
        pricesLogic.UpdateList(NewPriceCategory);
        Console.WriteLine($"De prijs categorie '{passenger}' is toevoegd");
        ShowPriceInformation(NewPriceCategory);
        Console.WriteLine($"\nU keert terug naar het startscherm.\n");
        Menu.Start(); 
    }

    public static void EditPrice()
    {
        Console.WriteLine("Een overzicht van alle prijscategorieën");
        ShowAllPricesInforamtion();
        PriceModel price = SearchByID();
        if (price != null)
        {
            ShowPriceInformation(price);
            Console.WriteLine("Wilt u deze prijscategorie bewerken ? Y/N: ");
            string? answer = Console.ReadLine();
            if (answer!= null && answer.ToLower() == "n")
            {
                Console.WriteLine("Uw antwoord is nee. \nU keert terug naar het startscherm.\n");
                Menu.Start();
            }
            else if (answer!= null && answer.ToLower() != "y")
            {
                Console.WriteLine("Verkeerde input!");
                EditPrice();
            }
            UpdatePrice(price);
        }
        else
        {
            Console.WriteLine("Geen prijscategorie gevonden");
        }
        Console.WriteLine("U keert terug naar het startscherm.\n");
        Menu.Start();
    
    }
    public static void UpdatePrice(PriceModel price)
    {

    }
    public static void ShowPriceInformation(PriceModel price)
    {
        Console.WriteLine($"ID: {price.ID}");
        Console.WriteLine($"Passenger: {price.Passenger}");
        Console.WriteLine($"Price: {price.Price}");
        Console.WriteLine("---------------");
    }

    public static void ShowAllPricesInforamtion()
    {
        List<PriceModel> ListAllPrices = pricesLogic.GetPrices;
        
        foreach (PriceModel price in ListAllPrices)
        {
            Console.WriteLine($"ID: {price.ID}");
            Console.WriteLine($"Passenger: {price.Passenger}");
            Console.WriteLine($"Price: {price.Price}");
            Console.WriteLine("---------------");
        }
    }

    public static PriceModel SearchByID()
    {
        Console.WriteLine("Voer een ID in om te zoeken: ");
        int ID = Convert.ToInt32(Console.ReadLine());
        PriceModel price = pricesLogic.GetById(ID);
        return price;
    }

}