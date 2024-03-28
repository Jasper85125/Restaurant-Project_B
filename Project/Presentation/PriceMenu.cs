using System.Formats.Asn1;

static class PriceMenu
{
    static private PriceLogic pricesLogic = new();
    public static void Start()
    {
        Console.WriteLine("\nWelkom bij het overzicht voor prijzen.");
        Console.WriteLine("Wat wilt u doen?");
        Console.WriteLine("[1] Een prijs categorie toevoegen.");
        Console.WriteLine("[2] Een prijs categorie editen.");
        Console.WriteLine("[3] Een prijs categorie verwijderen.");
        Console.WriteLine("[4] Een overzicht van alle prijscategorieën.");
        Console.WriteLine("[5] Ga terug naar het vorige menu.");
        string? input = Console.ReadLine();
        if (input != null)
        {
            switch (input)
            {
                case "1":
                    Console.Clear();
                    AddPriceCategory();
                    break;
                case "2":
                    Console.Clear();
                    EditPriceCategory();
                    break;
                case "3":
                    Console.Clear();
                    DeletePriceCategory();
                    break;
                case "4":
                    Console.Clear();
                    ShowAllPricesInformation();
                    AfterShowingInformation();
                    break;
                case "5":
                    Console.Clear();
                    BackToStartMenu();
                    Menu.Start();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Verkeerde input!");
                    ;System.Threading.Thread.Sleep(3000);
                    Start();
                    break;
            }
        }

        else
        {
            Console.WriteLine("Verkeerde input!");
            ;System.Threading.Thread.Sleep(3000);
            Start();
        }
    }

    public static void DeletePriceCategory()
    {
        Console.WriteLine("Een overzicht van alle prijscategorieën");
        ShowAllPricesInformation();
        Console.WriteLine("Voer het nummer van de ID in dat u wilt verwijderen: ");
        PriceModel priceModel = SearchByID();

        if (priceModel != null)
        {
            ShowPriceInformation(priceModel);
            Console.WriteLine("Wilt u deze prijscategorie verwijderen? Y/N: ");
            string? answer = Console.ReadLine();
            if (answer!= null && answer.ToLower() == "n")
            {
                Console.WriteLine("Uw antwoord is nee.");
                BackToStartMenu();
            }
            else if (answer!= null && answer.ToLower() != "y")
            {
                Console.WriteLine("Verkeerde input!");
                ;System.Threading.Thread.Sleep(3000);
                DeletePriceCategory();
            }

            pricesLogic.DeletePriceCategory(priceModel.ID);
            Console.WriteLine("De verwijdering is voltooid");
        }
        else
        {
            Console.WriteLine("Geen prijscategorie gevonden");
            ;System.Threading.Thread.Sleep(3000);
        }
        BackToStartMenu();

    }
    public static void AddPriceCategory()
    {
        Console.WriteLine("Hieronder kunt u de huidige prijscategorieën zien");
        ShowAllPricesInformation();
        Console.WriteLine("Wilt u nog een prijs categorie toevoegen? Y/N: ");
        string? answer = Console.ReadLine();

        if (answer!= null && answer.ToLower() == "n")
        {
            Console.WriteLine("Uw antwoord is nee.");
            BackToStartMenu();
        }
        else if (answer!= null && answer.ToLower() != "y")
        {
            Console.WriteLine("Verkeerde input!");
            ;System.Threading.Thread.Sleep(3000);
            AddPriceCategory();
        }

        Console.WriteLine("Voer de naam van het nieuwe passenger type in: ");
        string? NewPassenger = Console.ReadLine();
        Console.WriteLine("Bepaal de prijs voor de nieuwe passenger: ");
        double NewPrice = Convert.ToDouble(Console.ReadLine());

        PriceModel NewPriceCategory = new (pricesLogic.GenerateNewId(), NewPassenger, NewPrice);
        pricesLogic.UpdateList(NewPriceCategory);
        Console.WriteLine($"De prijs categorie '{NewPassenger}' is toevoegd");
        ;System.Threading.Thread.Sleep(2000);
        BackToStartMenu();
    }
    public static void EditPriceCategory()
    {
        Console.WriteLine("Een overzicht van alle prijscategorieën");
        ShowAllPricesInformation();
        Console.WriteLine("Voer het nummer van de ID in dat u wilt bewerken: ");
        PriceModel priceModel = SearchByID();
        if (priceModel != null)
        {
            ShowPriceInformation(priceModel);
            Console.WriteLine("Wilt u deze prijscategorie bewerken? Y/N: ");
            string? answer = Console.ReadLine();
            if (answer!= null && answer.ToLower() == "n")
            {
                Console.WriteLine("Uw antwoord is nee.");
                BackToStartMenu();
            }
            else if (answer!= null && answer.ToLower() != "y")
            {
                Console.WriteLine("Verkeerde input!");
                ;System.Threading.Thread.Sleep(3000);
                EditPriceCategory();
            }
            UpdatePriceCategory(priceModel);
        }
        else
        {
            Console.WriteLine("Geen prijscategorie gevonden");
        }
        BackToStartMenu();
    
    }
    public static void UpdatePriceCategory(PriceModel priceModel)
    {
        Console.WriteLine("Voer de nieuwe ID: ");
        int NewID = Convert.ToInt32(Console.ReadLine());

        // Check if the new ID already exists in the list
        if (NewID != priceModel.ID && pricesLogic.GetPrices.Any(p => p.ID == NewID)) // pricesLogic.GetPrices is een list
        {
            Console.WriteLine("Dit ID bestaat al. Kies een ander ID.");
            UpdatePriceCategory(priceModel);
            return;
        }
        priceModel.ID = NewID;
    
        Console.WriteLine("Voer de naam van het nieuwe passenger type in: ");
        string? NewPassenger = Console.ReadLine();
        priceModel.Passenger = NewPassenger;

        Console.WriteLine("Bepaal de nieuwe prijs: ");
        double NewPrice = Convert.ToDouble(Console.ReadLine());
        priceModel.Price = NewPrice;

        pricesLogic.UpdateList(priceModel);
        Console.WriteLine("De bewerking is voltooid, hier is het resultaat: ");
        ShowPriceInformation(priceModel);
        BackToStartMenu();
    }
    public static void ShowPriceInformation(PriceModel priceModel)
    {
        Console.WriteLine($"ID: {priceModel.ID}");
        Console.WriteLine($"Passenger: {priceModel.Passenger}");
        Console.WriteLine($"Price: {priceModel.Price}");
        Console.WriteLine("---------------");
    }

    public static void ShowAllPricesInformation()
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
        int ID = Convert.ToInt32(Console.ReadLine());
        PriceModel price = pricesLogic.GetById(ID);
        return price;
    }

    public static void BackToStartMenu()
    {
        Console.WriteLine("U keert terug naar het Startmenu.\n");
        ;System.Threading.Thread.Sleep(3000);
        Menu.Start();
    }

    public static void AfterShowingInformation()
    {
        string answer = "";
        while (answer.ToLower() !="y")
        {       
            Console.WriteLine("Om terug te gaan naar het Startmenu voer y in.");
            answer = Console.ReadLine();
        }
        BackToStartMenu();
    }
}