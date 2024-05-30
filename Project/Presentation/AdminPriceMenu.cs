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
    
    public static void ShowAllPricesInformation()
    {
        string title = "Het prijscategorie menu";
        List<string> header = new() {"Id", "doelgroep", "prijs", "activiteit"};
        List<PriceModel> priceModels = pricesLogic.GetAll();
        string kind = "prijscategorie";
        if (priceModels == null || priceModels.Count == 0)
        {
            PriceModel newPriceModel = new(pricesLogic.GenerateNewId(),"",0,false);
                    pricesLogic.UpdateList(newPriceModel);
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
                         else if (selectedIndex == 3)
                            {
                                dynamic item = priceModels[selectedRowIndex].IsActive;
                                if (priceModels[selectedRowIndex].IsActive)
                                {
                                    priceModels[selectedRowIndex].IsActive = false;
                                }
                                else{
                                    priceModels[selectedRowIndex].IsActive = true;
                                }
                                pricesLogic.UpdateList(priceModels[selectedRowIndex]);
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
        var active = priceModel.IsActive;
        string activity = "";
        if (active)
        {
            activity = "Actief";
        }
        else
        {
            activity = "Non-actief";
        }
        return new List<string> { $"{id}", $"{passenger}", $"{price}",$"{activity}" };
    }

    public static void BackToStartMenu()
    {
        Console.WriteLine("U keert terug naar het Startmenu.\n");
        Thread.Sleep(3000);
        Menu.Start();
    }


    // public static bool ConfirmValue(PriceModel priceModel, string UpdatedValue = null, bool IsUpdate = false, bool delete = false)
    // {
    //     if (IsUpdate && string.IsNullOrEmpty(UpdatedValue) && !delete || !IsUpdate && (priceModel == null) && !delete)
    //     {
    //         ColorPrint.PrintRed(IsUpdate ? "Ongeldige invoer." : "Fout: Nieuwe prijsgevens ontbreken!");
    //         Thread.Sleep(3000);
    //         Console.Clear();
    //         return false;
    //     }

    //     if (delete)
    //     {
    //         Console.WriteLine($"U staat op het punt de prijscategorie te verwijderen met de volgende info");
    //         OldShowPriceInformation(priceModel);
    //     }
    //     else if (!IsUpdate)
    //     {
    //         Console.WriteLine($"U staat op het punt een nieuwe prijscategorie toe te voegen met de volgende info");
    //         OldShowPriceInformation(priceModel);
    //     }
    //     else if (IsUpdate)
    //     {
    //         Console.WriteLine($"U staat op het punt oude data te veranderen met de volgende info");
    //         OldShowPriceInformation(priceModel);
    //     }

    //     do
    //     {
    //         ConsoleKeyInfo keyInfo;
    //         Console.Write("Druk op ");
    //         Console.ForegroundColor = ConsoleColor.Green;
    //         Console.Write("Enter");
    //         Console.ResetColor();
    //         Console.Write(" om door te gaan of druk op ");
    //         Console.ForegroundColor = ConsoleColor.Red;
    //         Console.Write("Backspace");
    //         Console.ResetColor();
    //         Console.WriteLine(" om te annuleren.");

    //         keyInfo = Console.ReadKey(true);
    //         if (keyInfo.Key == ConsoleKey.Backspace)
    //         {
    //             ColorPrint.PrintRed(!delete ? "Toevoegen geannuleerd." : "Verwijderen geannuleerd");
    //             Thread.Sleep(3000);
    //             Console.Clear();
    //             return false;
    //         }
    //         else if (keyInfo.Key == ConsoleKey.Enter)
    //         {
    //             ColorPrint.PrintGreen(!delete ? "Data is toegevoegd!" : "De verwijdering is voltooid");
    //             Thread.Sleep(3000);
    //             Console.Clear();
    //             return true;
    //         }
    //         else
    //         {
    //             ColorPrint.PrintRed("Ongeldige invoer!");
    //             Thread.Sleep(3000);
    //             // return false;
    //         }
    //     }while(true);
    // }

    // public static void OldShowPriceInformation(PriceModel priceModel)
    // {
    //     List<PriceModel> priceModels = new() {priceModel};
    //     List<string> header = new() {"Id", "Doelgroep", "Prijs"};
    //     if (priceModels == null || priceModels.Count == 0)
    //     {
    //         Console.WriteLine("Lege data.");
    //     }
    //     else
    //     {
    //         basictableLogic.PrintTable(header, priceModels, GenerateRow);
    //     }
    // }
}
