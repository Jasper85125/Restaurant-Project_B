using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Formats.Asn1;
using Microsoft.VisualBasic;

public static class AdminPriceMenu
{
    private static PriceLogic pricesLogic = new();
    private static TableLogic<PriceModel> tablePrices = new();
    private static BasicTableLogic<PriceModel> basictableLogic = new();


    public static void Start()
    {
        ShowAllPricesInformation();
    }

    public static void DeletePriceCategory()
    {
        Console.WriteLine("Een overzicht van alle prijscategorieën");
        OldShowAllPricesInformation();
        Console.WriteLine("Voer het nummer van de ID in dat u wilt verwijderen: ");
        PriceModel priceModel = SearchByID();

        if (priceModel != null)
        {
            Console.Clear();
            OldShowPriceInformation(priceModel);
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
            ColorPrint.PrintRed("Geen prijscategorie gevonden");
            Thread.Sleep(3000);

        }
        BackToStartMenu();

    }
    
    public static void ShowAllPricesInformation()
    {
        string title = "Het prijscategorie menu";
        List<string> header = new() {"Id", "doelgroep", "prijs", "activiteit"};
        List<PriceModel> priceModels = pricesLogic.GetAll();
        string kind = "prijscategorie";
        if (priceModels == null || priceModels.Count == 0)
        {
            ColorPrint.PrintRed("Lege data.");
            Console.WriteLine("U keert terug naar het admin hoofd menu.\n");
            Thread.Sleep(3000);
            AdminStartMenu.Start();
        }
        while(true)
        {
            (List<string> SelectedRow, int SelectedRowIndex)? TableInfo = tablePrices.PrintTable(header, priceModels, GenerateRow, title, Listupdater, kind);
            if(TableInfo == null){
                AdminStartMenu.Start();
                return;
            }
            else
            {
                int selectedRowIndex = TableInfo.Value.SelectedRowIndex;
                List<string> selectedRow = TableInfo.Value.SelectedRow;
                if(selectedRowIndex == priceModels.Count())
                {
                    PriceModel newPriceModel = new(pricesLogic.GenerateNewId(),"",0,false);
                    pricesLogic.UpdateList(newPriceModel);
                    continue;
                }
                while(true)
                {
                    selectedRow = GenerateRow(priceModels[selectedRowIndex]);
                    (string SelectedItem, int SelectedIndex)? result = tablePrices.PrintSelectedRow(selectedRow, header);
                    if (result == null){
                        break; //exit loop door escape
                    }
                    else
                    {
                        string selectedItem = result.Value.SelectedItem;
                        int selectedIndex = result.Value.SelectedIndex;

                        if (selectedIndex == 0)
                        {
                            ColorPrint.PrintRed($"U kan de {header[selectedIndex]} niet aanpassen.");
                            Thread.Sleep(3000);
                        }
                        else if(selectedIndex == 1)
                        {
                            while(true)
                            {
                                Console.WriteLine($"Voer een nieuwe {header[selectedIndex]} in om");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write($"'{selectedItem}'");
                                Console.ResetColor();
                                Console.Write(" te vervangen:\n");
                                string Input = Console.ReadLine();
                                while (!Helper.IsValidString(Input))
                                {
                                    ColorPrint.PrintRed($"'{Input}' is geen geldige optie.");
                                    Console.WriteLine($"Wat is de naam van de nieuwe {header[selectedIndex]}?");
                                    Input = Console.ReadLine();
                                }
                                //variable to check Passenger
                                bool PassengerExists = false;

                                // Check if the input Passenger already exists
                                foreach(var priceIndex in priceModels) {
                                    if(Input == priceIndex.Passenger) {
                                        PassengerExists = true;
                                        break;
                                    }
                                }

                                if(PassengerExists) {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Naam bestaat al, geef een andere op.");
                                    Console.ResetColor();
                                    Thread.Sleep(3000);
                                } else {
                                    //if Passenger does not exists, it gets added to the list
                                    priceModels[selectedRowIndex].Passenger = Input;
                                    pricesLogic.UpdateList(priceModels[selectedRowIndex]);
                                    break;
                                }
                            }
                        
                        }
                        else if(selectedIndex == 2)
                        {
                            while (true)
                            {
                                Console.WriteLine($"Voer een nieuwe {header[selectedIndex]} in om");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write($"'{selectedItem}'");
                                Console.ResetColor();
                                Console.Write(" te vervangen:\n");
                                string Input = Console.ReadLine();
                                while (!Helper.IsValidDouble(Input))
                                {
                                    ColorPrint.PrintRed($"'{Input}' is geen geldige optie.");
                                    Console.WriteLine("De prijs moet in hele getallen gegeven worden.");
                                    Console.WriteLine("Wat is de nieuwe prijs?");
                                    Input = Console.ReadLine();
                                }
                                priceModels[selectedRowIndex].Price = Convert.ToDouble(Input);
                                pricesLogic.UpdateList(priceModels[selectedRowIndex]);
                                break;      
                            }
                        }
                    }
                }
            }
        }
        
    }

    public static void Listupdater(PriceModel model){
        pricesLogic.UpdateList(model);
    }


    public static List<string> GenerateRow(PriceModel priceModel)
    {
        var id = priceModel.Id;
        var passenger = priceModel.Passenger;
        var price = priceModel.Price;
        var Actief = priceModel.IsActive;
        string Activiteit = "";
        if (Actief)
        {
            Activiteit = "Actief";
        }
        else{
            Activiteit = "Non-actief";
        }
        return new List<string> { $"{id}", $"{passenger}", $"{price}",$"{Activiteit}" };
    }

    public static bool ConfirmValue(PriceModel priceModel, string UpdatedValue = null, bool IsUpdate = false, bool delete = false)
    {
        if (IsUpdate && string.IsNullOrEmpty(UpdatedValue) && !delete || !IsUpdate && (priceModel == null) && !delete)
        {
            ColorPrint.PrintRed(IsUpdate ? "Ongeldige invoer." : "Fout: Nieuwe prijsgevens ontbreken!");
            Thread.Sleep(2000);
            Console.Clear();
            return false;
        }

        if (delete)
        {
            Console.WriteLine($"U staat op het punt de prijscategorie te verwijderen met de volgende info");
            OldShowPriceInformation(priceModel);
        }
        else if (!IsUpdate)
        {
            Console.WriteLine($"U staat op het punt een nieuwe prijscategorie toe te voegen met de volgende info");
            OldShowPriceInformation(priceModel);
        }
        else if (IsUpdate)
        {
            Console.WriteLine($"U staat op het punt oude data te veranderen met de volgende info");
            OldShowPriceInformation(priceModel);
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
                ColorPrint.PrintRed(!delete ? "Toevoegen geannuleerd." : "Verwijderen geannuleerd");
                Thread.Sleep(2000);
                Console.Clear();
                return false;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                ColorPrint.PrintGreen(!delete ? "Data is toegevoegd!" : "De verwijdering is voltooid");
                Thread.Sleep(2000);
                Console.Clear();
                return true;
            }
            else
            {
                ColorPrint.PrintRed("Ongeldige invoer!");
                Thread.Sleep(2000);
                // return false;
            }
        }while(true);
    }

    public static void BackToStartMenu()
    {
        Console.WriteLine("U keert terug naar het Startmenu.\n");
        Thread.Sleep(3000);
        Menu.Start();
    }

    public static PriceModel SearchByID()
    {
        int ID = Convert.ToInt32(Console.ReadLine());
        PriceModel price = pricesLogic.GetById(ID);
        return price;
    }
    public static void OldShowAllPricesInformation()
    {
        List<string> header = new() {"Id", "Doelgroep", "Prijs"};
        List<PriceModel> priceModels = pricesLogic.GetAll();
        if (priceModels == null || priceModels.Count == 0)
        {
            Console.WriteLine("Lege data.");
        }
        else
        {
            basictableLogic.PrintTable(header, priceModels, GenerateRow);
        }
    }

    public static void OldShowPriceInformation(PriceModel priceModel)
    {
        List<PriceModel> priceModels = new() {priceModel};
        List<string> header = new() {"Id", "Doelgroep", "Prijs"};
        if (priceModels == null || priceModels.Count == 0)
        {
            Console.WriteLine("Lege data.");
        }
        else
        {
            basictableLogic.PrintTable(header, priceModels, GenerateRow);
        }
    }
}
