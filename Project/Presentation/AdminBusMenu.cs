using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

public static class AdminBusMenu
{
    private static TableLogic<BusModel> tableBus = new();
    private static BusLogic busLogic = new();
    private static RouteLogic routeLogic = new();
    private static TableLogic<BusModel> tableRoutes = new();
    private static CustomerTableLogic<RouteModel> tableRoutesKlant = new();

    private static SeatLogic seatLogic = new();
    
    


    public static void Start()
    {

        ShowAllBusInformation();
    }

    public static void ShowAllBusInformation ()
    {
        string title = "Busmenu";
        BusLogic busLogic = new();
        List<BusModel> listAllBusses = busLogic.GetAll();
        List<string> header = new() { "Busnummer", "Kenteken", "Busmodel", "Route(s)", "Activiteit"};
        List<RouteModel> RoutesList = new() {};
        string kind = "bus";

        if (listAllBusses == null || listAllBusses.Count == 0)
        {
            MakeBusFormation(true);
        }
     
        while(true){
            (List<string> SelectedRow, int SelectedRowIndex)? TableInfo= tableBus.PrintTable(header, listAllBusses, GenerateRow, title, Listupdater, kind);
            if(TableInfo == null)
            {
                AdminStartMenu.Start(); //exit menu door escape
                return;
            }
            else
            {
                int selectedRowIndex = TableInfo.Value.SelectedRowIndex;
                List<string> SelectedRow = TableInfo.Value.SelectedRow;
                if(selectedRowIndex == listAllBusses.Count())
                {
                    Console.Clear();
                    MakeBusFormation();
                    listAllBusses = busLogic.GetAll();
                    selectedRowIndex = listAllBusses.Count() - 1;
                }
                while(true){
                    SelectedRow = GenerateRow(listAllBusses[selectedRowIndex]);
                    (string SelectedItem, int SelectedIndex)? result = tableBus.PrintSelectedRow(SelectedRow, header);
                    if (result == null){
                        break; //exit loop door escape
                    }
                    else{
                        string selectedItem = result.Value.SelectedItem;
                        int selectedIndex = result.Value.SelectedIndex;
                        if (selectedIndex == 0){
                            Console.WriteLine($"U kan {header[selectedIndex]} niet aanpassen.");
                            Thread.Sleep(3000);
                        }
                        else if(selectedIndex == 1)
                        {
                            while (true)
                            {
                                Console.Write("Voer een kenteken in");
                                ColorPrint.PrintCyan(" (X-999-XX):");
                                string Input = Helper.StringHelper();
                                if (Input == "Escape/GoBack.") ShowAllBusInformation();
                                Console.WriteLine();

                                while (!Helper.IsValidString(Input))
                                {
                                    ColorPrint.PrintRed("Uw opgegeven kenteken kan niet leeg zijn.");
                                    Console.Write("Voer een kenteken in");
                                    ColorPrint.PrintCyan(" (X-999-XX):");
                                    Input = Helper.StringHelper();
                                    if (Input == "Escape/GoBack.") ShowAllBusInformation();
                                    Console.WriteLine();
                                }

                                Input = Input.ToUpper();

                                bool isValidLicensePlate = Regex.IsMatch(Input, @"^[A-Z]{1}-[0-9]{3}-[A-Z]{2}$");

                                if (listAllBusses.Any(bus => bus.LicensePlate == Input))
                                {
                                    ColorPrint.PrintRed("Kenteken bestaat al, geef een andere op.");  
                                }
                                else if (!isValidLicensePlate)
                                {
                                    ColorPrint.PrintRed("Kenteken is ongeldig. Gebruik het formaat X-999-XX.");
                                }
                                else
                                {
                                    // If the input is valid and does not exist in the list, break the loop
                                    listAllBusses[selectedRowIndex].LicensePlate = Input;
                                    busLogic.UpdateList(listAllBusses[selectedRowIndex]);
                                    break;
                                }
                            }
                        }

                        else if(selectedIndex == 2){
                            while (true){
                            Console.WriteLine($"Voer het {header[2]} in om het huidige {header[2]} te veranderen:");
                            string Input = Console.ReadLine();
                            bool containsOnlyNumbers = Input.All(char.IsDigit);
                            if (containsOnlyNumbers){
                                listAllBusses[selectedRowIndex].Seats = Input;
                                busLogic.UpdateList(listAllBusses[selectedRowIndex]);
                                break;
                                }
                            }
                        }

                    
                        else if (selectedIndex == 3)
                        {
                            Console.Clear();
                            RoutesList = listAllBusses[selectedRowIndex].Route;
                            ConsoleKeyInfo keyInfo;
                            do
                            {
                                Console.Clear();
                                Console.Write("Dit zijn de toegevoegde Route(s):" );
                                int LastRouteIndex = 0;
                                foreach (RouteModel Route in RoutesList)
                                {
                                    Console.Write($"[{Route.Name}] ");
                                    LastRouteIndex++;
                                }
                                
                                Console.WriteLine();
                                Console.Write("Als u nog een route wil toevoegen klik op");
                                ColorPrint.PrintWriteBlue(" Spatie.");
                                Console.Write("\nAls u de laatste route wil verwijderen klik op");
                                ColorPrint.PrintWriteRed(" Backspace.");
                                Console.Write("\nAls u tevreden bent met de routelijst, voeg de lijst toe met");
                                ColorPrint.PrintWriteGreen(" Enter.");

                                keyInfo = Console.ReadKey(true);
                                
                                switch (keyInfo.Key)
                                {
                                    case ConsoleKey.Spacebar:
                                        Console.WriteLine("\nU heeft op Spatie geklikt. Voeg een nieuwe route toe.");
                                        RouteModel Input = AdminRouteMenu.SelectRoute();
                                        if (Input != null){
                                            RoutesList.Add(Input);
                                            Console.WriteLine($"{Input.Name} is toegevoegd");
                                            listAllBusses[selectedRowIndex].Route = RoutesList;
                                            busLogic.UpdateList(listAllBusses[selectedRowIndex]);
                                            Thread.Sleep(3000);
                                            break;
                                            }
                                        else{
                                            Console.WriteLine("U keert terug");
                                            Thread.Sleep(3000);
                                            break;
                                        }
                                    case ConsoleKey.Backspace:
                                        if (LastRouteIndex >= 1)
                                        {
                                            Console.WriteLine("\nU heeft op Backspace geklikt. De laatste route is verwijderd");
                                            RoutesList.Remove(RoutesList[LastRouteIndex-1]);
                                            Thread.Sleep(3000);
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nGeen routes om te verwijderen.");
                                            Thread.Sleep(3000);
                                        }
                                        break;
                                    case ConsoleKey.Enter:
                                        Console.WriteLine("\nU heeft op Enter geklikt. De routelijst is toegevoegd");
                                        Thread.Sleep(3000);
                                        listAllBusses[selectedRowIndex].Route = RoutesList;
                                        busLogic.UpdateList(listAllBusses[selectedRowIndex]);

                                        break;
                                    default:
                                        Console.WriteLine("\nOngeldige invoer.");
                                        Thread.Sleep(1000);
                                        break;
                                }
                            } while (keyInfo.Key != ConsoleKey.Enter);
                        }
                        else if (selectedIndex == 4)
                            {
                                dynamic item = listAllBusses[selectedRowIndex].IsActive;
                                if (listAllBusses[selectedRowIndex].IsActive)
                                {
                                    listAllBusses[selectedRowIndex].IsActive = false;
                                }
                                else{
                                    listAllBusses[selectedRowIndex].IsActive = true;
                                }
                                busLogic.UpdateList(listAllBusses[selectedRowIndex]);
                            }                   
                    }
                }
            }
        }

    }


    /// Voor deze functies moet later nog een string format checker worden geschreven.
    // public static void AddTime()
    // {
    //     ShowAllBusInformation(Overview());
    //     Console.WriteLine("Aan welke bus met route wilt U een tijd geven?");
    //     string? busID = Console.ReadLine();
    //     try
    //     {
    //         BusLogic LogicInstance = new BusLogic ();
    //         int intInputBus = Convert.ToInt32(busID);
    //         BusModel bus = LogicInstance.GetById(intInputBus);
    //         if (HasRoute(bus))
    //         {
    //             foreach (RouteModel Route in bus.Route)
    //             {
    //                 Console.WriteLine("Wat is de begintijd voor de route?");
    //                 string? beginTimeRoute = Console.ReadLine();
    //                 while (beginTimeRoute == null)
    //                 {
    //                     Console.WriteLine("Vul een correcte waarde in");
    //                 }
    //                 Route.beginTime = beginTimeRoute;

    //                 foreach (StopModel Stop in Route.Stops)
    //                 {
    //                     Console.WriteLine($"Wat is de tijd voor halte {Stop.Name}");
    //                     string? newTime = Console.ReadLine();
    //                     while (newTime == null)
    //                     {
    //                         Console.WriteLine("Vul een correcte waarde in");
    //                     }
    //                     Stop.Time = newTime;
    //                 }

    //                 Console.WriteLine("Wat is de eindtijd voor de route?");
    //                 string? endTimeRoute = Console.ReadLine();
    //                 while (endTimeRoute == null)
    //                 {
    //                     Console.WriteLine("Vul een correcte waarde in");
    //                 }
    //                 Route.endTime = endTimeRoute;
    //             }
    //             LogicInstance.UpdateList(bus);
    //         }
    //         else
    //         {
    //             Console.WriteLine("Deze bus heeft nog geen route aangewezen gekregen.");
    //             Start();
    //         }
    //     }
    //     catch (Exception)
    //     {
    //         Console.WriteLine("Verkeerde input ");
    //     }

    // }

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

    public static bool ConfirmValue(BusModel newBus, string UpdatedValue = null, bool IsUpdate = false)
    {
        if (IsUpdate && string.IsNullOrEmpty(UpdatedValue) || !IsUpdate && (newBus == null || string.IsNullOrEmpty(newBus.LicensePlate)))
        {
            ColorPrint.PrintRed(IsUpdate ? "Ongeldige invoer." : "Fout: Nieuwe busgegevens ontbreken!");
            Thread.Sleep(3000);
            Console.Clear();
            return false;
        }

        Console.WriteLine(!IsUpdate ? $"U staat op het punt een nieuwe bus toe te voegen met de volgende info: zitplaatsen: {newBus.Seats}, Kenteken: {newBus.LicensePlate}" : $"U staat op het punt oude data te veranderen: {UpdatedValue}");
        Console.Write("Druk op ");
        ColorPrint.PrintWriteGreen("Enter");
        Console.Write(" om door te gaan of druk op ");
        ColorPrint.PrintWriteRed("Backspace");
        Console.WriteLine(" om te annuleren.");

        ConsoleKeyInfo keyInfo;
        do
        {
            keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                ColorPrint.PrintRed("Toevoegen geannuleerd.");
                Thread.Sleep(3000);
                Console.Clear();
                return false;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                ColorPrint.PrintGreen("Data is toegevoegd!");
                Thread.Sleep(3000);
                Console.Clear();
                return true;
            }
            else
            {
                ColorPrint.PrintRed("Ongeldige invoer!");
                Thread.Sleep(3000);
                Console.Clear();
                return false;
            }
        }while(true);
    }



    public static List<string> GenerateRow(BusModel busModel)
    {
        var id = busModel.Id;
        var seats = busModel.Seats;
        var licensePlate = busModel.LicensePlate;
        var routeNames = string.Join(", ", busModel.Route.Select(r => r.Name));
        var active = busModel.IsActive;
        string activity = "";
        if (active)
        {
            activity = "Actief";
        }
        else{
            activity = "Non-actief";
        }
        
        return new List<string> { $"{id}", $"{licensePlate}", $"{seats}", $"{routeNames}",$"{activity}" };
    }

    public static void Listupdater(BusModel model){
        busLogic.UpdateList(model);
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

    public static void BackToStartMenu()
    {
        Console.WriteLine("U keert terug naar het Startmenu.\n");
        Thread.Sleep(3000);
        Menu.Start();
    }

    public static void MakeBusFormation(bool showMessage = false)
    {
        int selectedOption = 1; // Default selected option

        // Display options
        DisplayOptions(selectedOption, showMessage);

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
                    selectedOption = Math.Min(2, selectedOption + 1);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    // Perform action based on selected option (e.g., execute corresponding function)
                    switch (selectedOption)
                    {
                        case 1:
                            BusModel newBusBusiness = new(busLogic.GenerateNewId(),"Business","Nieuwe Bus",false);
                            SeatModel[,] seatModelsBusiness  = new SeatModel[7, 12];
                            seatLogic.CreateBusinessSeats(seatModelsBusiness);
                            Dictionary<(int Row, int Col), SeatModel> seatingMapBusiness = seatLogic.ConvertToDict(seatModelsBusiness); // seatModels wordt geconvert naar Dictionary
                            newBusBusiness.AddSeatingMap(seatingMapBusiness);
                            busLogic.UpdateList(newBusBusiness);
                            return;
                        case 2:
                            BusModel newBusModelPlebs = new(busLogic.GenerateNewId(),"Plebs","Nieuwe Bus",false);
                            SeatModel[,] seatModelsPlebs = new SeatModel[7, 14];
                            seatLogic.CreateSeats(seatModelsPlebs); // seatModels wordt gevuld met stoelen
                            Dictionary<(int Row, int Col), SeatModel> seatingMapPlebs = seatLogic.ConvertToDict(seatModelsPlebs); // seatModels wordt geconvert naar Dictionary
                            newBusModelPlebs.AddSeatingMap(seatingMapPlebs);
                            busLogic.UpdateList(newBusModelPlebs);
                            return;
                    }
                    break;
                case ConsoleKey.Escape:
                    ShowAllBusInformation();
                    return;
            }

            // Clear console and display options
            Console.Clear();
            DisplayOptions(selectedOption, showMessage);
        }

        static void DisplayOptions(int selectedOption, bool showMessage = false)
        {
            if (showMessage) 
            {
                ColorPrint.PrintYellow("Het systeem heeft nog geen bussen, voeg een bus toe.");
            }
            Console.WriteLine("Wat voor bus indeling wilt u?\n");

            // Display option 1
            Console.ForegroundColor = selectedOption == 1 ? ConsoleColor.Green: ConsoleColor.White;
            Console.Write(selectedOption == 1 ? ">> " : "   ");
            Console.WriteLine("Business/Party.");

            // Display option 2
            Console.ForegroundColor = selectedOption == 2 ? ConsoleColor.Green : ConsoleColor.White;
            Console.Write(selectedOption == 2 ? ">> " : "   ");
            Console.WriteLine("Normaal (Plebs).");

            // Reset text color
            Console.ResetColor();

            Console.Write("\nKlik");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" Enter ");
            Console.ResetColor();
            Console.WriteLine("om een optie te selecteren");

            Console.Write("Klik");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" Escape ");
            Console.ResetColor();
            Console.WriteLine("om naar het bussen overzicht te gaan.\n");
        }
    }
}   
