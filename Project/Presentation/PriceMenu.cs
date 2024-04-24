using System.Diagnostics.Contracts;
using System.Formats.Asn1;
using Microsoft.VisualBasic;

public static class PriceMenu
{
    private static PriceLogic pricesLogic = new();
    private static TableLogic<PriceModel> tablePrices = new();

    public static void Start()
    {
        int selectedOption = 1; // Default selected option

        DisplayOptions(selectedOption);

        while (true)
        {
            // Wait for key press
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            // Check arrow key presses
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    // Move to the previous option
                    selectedOption = Math.Max(1, selectedOption - 1);
                    break;
                case ConsoleKey.DownArrow:
                    // Move to the next option
                    selectedOption = Math.Min(5, selectedOption + 1);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    // Perform action based on selected option (e.g., execute corresponding function)
                    switch (selectedOption)
                    {
                        case 1:
                            AddPriceCategory();
                            break;
                        case 2:
                            EditPriceCategory();
                            break;
                        case 3:
                            DeletePriceCategory();
                            break;
                        case 4:
                            ShowAllPricesInformation();
                            AfterShowingInformation();
                            break;
                        case 5:
                            Menu.Start();
                            break;
                        default:
                            Console.WriteLine("Verkeerde input!");
                            Thread.Sleep(3000);
                            Start();
                            break;
                    }
                    break;
            }
            

            // Clear console and display options
            Console.Clear();
            DisplayOptions(selectedOption);
        }

    }

    public static void DisplayOptions(int selectedOption)
    {
        Console.WriteLine("Selecteer een optie:");

        // Display option 1
        Console.ForegroundColor = selectedOption == 1 ? ConsoleColor.Green: ConsoleColor.White;
        Console.Write(selectedOption == 1 ? ">> " : "   ");
        Console.WriteLine("[1] Een prijscategorie toevoegen.");

        // Display option 2
        Console.ForegroundColor = selectedOption == 2 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 2 ? ">> " : "   ");
        Console.WriteLine("[2] Een prijscategorie updaten.");

        // Display option 3
        Console.ForegroundColor = selectedOption == 3 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 3 ? ">> " : "   ");
        Console.WriteLine("[3] Een prijscategorie verwijderen.");

        // Display option 4
        Console.ForegroundColor = selectedOption == 4 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 4 ? ">> " : "   ");
        Console.WriteLine("[4] Een overzicht van alle prijscategorieën.");

        // Display option 5
        Console.ForegroundColor = selectedOption == 5 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 5 ? ">> " : "   ");
        Console.WriteLine("[5] Ga terug naar het vorige menu.");

        // Reset text color
        Console.ResetColor();
    }

    public static void DeletePriceCategory()
    {
        Console.WriteLine("Een overzicht van alle prijscategorieën");
        ShowAllPricesInformation();
        Console.WriteLine("Voer het nummer van de ID in dat u wilt verwijderen: ");
        PriceModel priceModel = SearchByID();

        if (priceModel != null)
        {
            Console.Clear();
            ShowPriceInformation(priceModel);
            Console.WriteLine("Wilt u deze prijscategorie verwijderen? J/N: ");
            string? answer = Console.ReadLine();
            // if (Helper.IsString(answer))
            // {
            //     System.Console.WriteLine("asdsasa");
            // }
            if (answer!= null && answer.ToLower() == "n")
            {
                Console.WriteLine("Uw antwoord is nee.");
                BackToStartMenu();
            }
            else if (answer!= null && answer.ToLower() != "j")
            {
                Console.WriteLine("Verkeerde input!");
                Thread.Sleep(3000);
                DeletePriceCategory();
            }
            
            if (ConfirmValue(priceModel, null, false, true))
            {
                pricesLogic.DeletePriceCategory(priceModel.Id);
            }
            else
            {
                BackToStartMenu();
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Geen prijscategorie gevonden");
            Thread.Sleep(3000);
            Console.ResetColor();

        }
        BackToStartMenu();

    }
    public static void AddPriceCategory()
    {
        Console.WriteLine("Hieronder kunt u de huidige prijscategorieën zien");
        ShowAllPricesInformation();
        while (true)
        {
            Console.WriteLine("Wilt u nog een prijscategorie toevoegen? J/N: ");
            string? answer = Console.ReadLine();
            if (answer!= null && answer.ToLower() == "n")
            {
                Console.WriteLine("Uw antwoord is nee.");
                BackToStartMenu();
                break;
            }
            else if (answer!= null && answer.ToLower() == "j" )
            {
                Thread.Sleep(1000);
                break;

            }
            else if (answer!= null && answer.ToLower() != "j")
            {
                Console.WriteLine("Verkeerde input!");
                Thread.Sleep(2000);
            }
        }
        
        Console.WriteLine("Voer de naam van het nieuwe passenger type in: ");
        string? newPassenger = Console.ReadLine();
        Console.WriteLine("Bepaal de prijs voor de nieuwe passenger: ");
        string newPrice = Console.ReadLine();
        PriceModel newPriceCategory = new (pricesLogic.GenerateNewId(), newPassenger, Convert.ToDouble(newPrice));
        if (ConfirmValue(newPriceCategory, newPrice, false))
        {
            pricesLogic.UpdateList(newPriceCategory);
            Console.WriteLine($"De prijscategorie '{newPassenger}' is toevoegd");
        }
        else
        {
            while (true)
            {
                Console.WriteLine("Wilt u nog een prijscategorie toevoegen? J/N: ");
                string? answer = Console.ReadLine();
                if (answer!= null && answer.ToLower() == "n")
                {
                    Console.WriteLine("Uw antwoord is nee.");
                    BackToStartMenu();
                    break;
                }
                else if (answer!= null && answer.ToLower() == "j" )
                {
                    Thread.Sleep(1000);
                    AddPriceCategory();
                    break;

                }
                else if (answer!= null && answer.ToLower() != "j")
                {
                    Console.WriteLine("Verkeerde input!");
                    Thread.Sleep(2000);
                }
            }
        }

        Thread.Sleep(2000);
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
            Console.WriteLine("Wilt u deze prijscategorie bewerken? J/N: ");
            string? answer = Console.ReadLine();
            if (answer!= null && answer.ToLower() == "n")
            {
                Console.WriteLine("Uw antwoord is nee.");
                BackToStartMenu();
            }
            else if (answer!= null && answer.ToLower() != "j")
            {
                Console.WriteLine("Verkeerde input!");
                Thread.Sleep(3000);
                EditPriceCategory();
            }
            UpdatePriceCategory(priceModel);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Geen prijscategorie gevonden!");
            Thread.Sleep(3000);
            Console.Clear();
            Console.ResetColor();
            EditPriceCategory();
        }
        BackToStartMenu();
    
    }
    public static void UpdatePriceCategory(PriceModel priceModel)
    {
        // Console.WriteLine("Voer de nieuwe ID: ");
        // int NewID = Convert.ToInt32(Console.ReadLine());

        // // Check if the new ID already exists in the list
        // if (NewID != priceModel.ID && pricesLogic.GetPrices.Any(p => p.ID == NewID)) // pricesLogic.GetPrices is een list
        // {
        //     Console.WriteLine("Dit ID bestaat al. Kies een ander ID.");
        //     UpdatePriceCategory(priceModel);
        //     return;
        // }
        // priceModel.ID = NewID;
    
        // Console.WriteLine("Voer de naam van het nieuwe passenger type in: ");
        // string? NewPassenger = Console.ReadLine();
        // priceModel.Passenger = NewPassenger;

        Console.WriteLine("Bepaal de nieuwe prijs: ");
        string NewPrice = Console.ReadLine();
        if (ConfirmValue(null, NewPrice, true))
        {
            priceModel.Price = Convert.ToDouble(NewPrice);
        }
        else
        {
            while (true)
            {
                Console.WriteLine("Wilt u verder met het toevoegen J/N");
                string answer = Console.ReadLine();
                if (answer!= null && answer.ToLower() == "n")
                {
                    Console.WriteLine("Uw antwoord is nee.");
                    BackToStartMenu();
                    break;
                }
                else if (answer!= null && answer.ToLower() == "j")
                {
                    Thread.Sleep(1000);
                    UpdatePriceCategory(priceModel);
                    break;
                }
                else if (answer!= null && answer.ToLower() != "j")
                {
                    Console.WriteLine("Verkeerde input!");
                    Thread.Sleep(2000);
                }
            }
        }

        pricesLogic.UpdateList(priceModel);
        Console.WriteLine("De bewerking is voltooid, hier is het resultaat: ");
        ShowPriceInformation(priceModel);
        BackToStartMenu();
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
        Thread.Sleep(3000);
        Menu.Start();
    }

    public static void AfterShowingInformation()
    {
        string answer = "";
        while (answer.ToLower() !="j")
        {       
            Console.WriteLine("Om terug te gaan naar het Startmenu voer J in.");
            answer = Console.ReadLine();
        }
        BackToStartMenu();
    }


    public static void ShowAllPricesInformation()
    {
        List<string> Header = new() {"Id", "Doelgroep", "Prijs"};
        List<PriceModel> priceModels = pricesLogic.GetAll();
        if (priceModels == null || priceModels.Count == 0)
        {
            Console.WriteLine("Lege data.");
        }
        else
        {
            while(true){
                (List<string> SelectedRow, int SelectedRowIndex)? TableInfo= tablePrices.PrintTable(Header, priceModels, GenerateRow);
                if(TableInfo != null){
                    int selectedRowIndex = TableInfo.Value.SelectedRowIndex;
                    while(true){
                        (string SelectedItem, int SelectedIndex)? result = tablePrices.PrintSelectedRow(TableInfo.Value.SelectedRow, Header);
                        //Console.WriteLine($"Selected Item: {result.Value.SelectedItem}, Selected Index: {result.Value.SelectedIndex}"); #test om PrintSelectedRow functie te testen.
                        if (result != null)                        {
                            string selectedItem = result.Value.SelectedItem;
                            int selectedIndex = result.Value.SelectedIndex;
                            if (selectedIndex == 0){
                                Console.WriteLine($"U kan {Header[selectedIndex]} niet aanpassen.");
                                Thread.Sleep(3000);
                            }
                            else if(selectedIndex == 1){
                                Console.WriteLine("Voer iets in om het item te veranderen:");
                                string Input = Console.ReadLine();
                                priceModels[selectedIndex].Passenger = Input;
                                pricesLogic.UpdateList(priceModels[selectedRowIndex]);
                                break;
                            }
                            else if(selectedIndex == 2){
                                while (true){
                                Console.WriteLine("Voer een nummer in het item te veranderen:");
                                string Input = Console.ReadLine();
                                bool containsOnlyNumbers = Input.All(char.IsDigit);
                                if (containsOnlyNumbers){
                                    priceModels[selectedIndex].Price = Convert.ToInt32(Input);
                                    pricesLogic.UpdateList(priceModels[selectedRowIndex]);
                                    break;
                                    }
                                }
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("U keert terug naar het prijsmenu overzicht.");
                            break;
                        }
                    }
                }
                else{
                    break;
                }
            }

        }
    }


    public static void ShowPriceInformation(PriceModel priceModel)
    {
        List<PriceModel> priceModels = new() {priceModel};
        List<string> Header = new() {"Id", "Doelgroep", "Prijs"};
        if (priceModels == null || priceModels.Count == 0)
        {
            Console.WriteLine("Lege data.");
        }
        else
        {
            tablePrices.PrintTable(Header, priceModels, GenerateRow);
        }
    }

    public static List<string> GenerateRow(PriceModel priceModel)
    {
        var id = priceModel.Id;
        var passenger = priceModel.Passenger;
        var price = priceModel.Price;
        return new List<string> { $"{id}", $"{passenger}", $"{price}" };
    }

    public static bool ConfirmValue(PriceModel priceModel, string UpdatedValue = null, bool IsUpdate = false, bool delete = false)
    {
        if (IsUpdate && string.IsNullOrEmpty(UpdatedValue) && !delete || !IsUpdate && (priceModel == null) && !delete)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(IsUpdate ? "Ongeldige invoer." : "Fout: Nieuwe prijsgevens ontbreken!");
            Console.ResetColor();
            Thread.Sleep(2000);
            Console.Clear();
            return false;
        }

        if (delete)
        {
            Console.WriteLine($"U staat op het punt de prijscategorie te verwijderen met de volgende info");
            ShowPriceInformation(priceModel);
        }
        else if (!IsUpdate)
        {
            Console.WriteLine($"U staat op het punt een nieuwe prijscategorie toe te voegen met de volgende info");
            ShowPriceInformation(priceModel);
        }
        else if (IsUpdate)
        {
            Console.WriteLine($"U staat op het punt oude data te veranderen: {UpdatedValue}");
        }

        do
        {
            ConsoleKeyInfo keyInfo;
            Console.Write("Druk op ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Enter");
            Console.ResetColor();
            Console.Write(" om door te gaan of druk op ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Backspace");
            Console.ResetColor();
            Console.WriteLine(" om te annuleren.");

            keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(!delete ? "Toevoegen geannuleerd." : "Verwijderen geannuleerd");
                Console.ResetColor();
                Thread.Sleep(2000);
                Console.Clear();
                return false;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(!delete ? "Data is toegevoegd!" : "De verwijdering is voltooid");
                Console.ResetColor();
                Thread.Sleep(2000);
                Console.Clear();
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ongeldige invoer!");
                Console.ResetColor();
                Thread.Sleep(2000);
                // return false;
            }
        }while(true);
    }
}
