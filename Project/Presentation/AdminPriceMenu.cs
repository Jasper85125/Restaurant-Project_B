using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
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
        List<string> header = new() {"Id", "doelgroep", "prijs in euro", "activiteit"};
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
                    if (result == null)
                    {
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
                                ColorPrint.PrintWriteRed($"'{selectedItem}'");
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
                                foreach(var priceIndex in priceModels) 
                                {
                                    if(Input == priceIndex.Passenger)
                                    {
                                        PassengerExists = true;
                                        break;
                                    }
                                }

                                if(PassengerExists) 
                                {
                                    ColorPrint.PrintRed("Naam bestaat al, geef een andere op.");
                                    Thread.Sleep(3000);
                                } 
                                else 
                                {
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
                                ColorPrint.PrintWriteRed($"'{selectedItem}'");
                                Console.Write(" te vervangen:\n");
                                string Input = Console.ReadLine();
                                while (!Helper.IsValidDouble(Input))
                                {
                                    ColorPrint.PrintRed($"'{Input}' is geen geldige optie.");
                                    Console.WriteLine("De prijs moet als een geheel getal of een decimaal getal worden ingevoerd.");
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

    public static void Listupdater(PriceModel model)
    {
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
        return new List<string> { $"{id}", $"{passenger}", $"{price:F2}",$"{activity}" };
    }

    public static void BackToStartMenu()
    {
        Console.WriteLine("U keert terug naar het Startmenu.\n");
        Thread.Sleep(3000);
        Menu.Start();
    }
}
