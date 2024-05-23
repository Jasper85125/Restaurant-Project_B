using System.Runtime.CompilerServices;

public static class AdminBusMenu
{
    private static TableLogic<BusModel> tableBus = new();
    private static BusLogic busLogic = new();
    private static RouteLogic routeLogic = new();
    private static TableLogic<BusModel> tableRoutes = new();
     private static TableLogicklant<RouteModel> tableRoutesKlant = new();
    
    


    public static void Start()
    {

        ShowAllBusInformation();
    }

    public static void ShowAllBusInformation ()
    {
        string Title = "Bus menu";
        BusLogic busLogic = new();
        List<BusModel> ListAllBusses = busLogic.GetAll();
        List<string> Header = new() { "Busnummer", "Kenteken", "Zitplaatsen", "Route(s)", "Activiteit"};
        List<RouteModel> RoutesList = new() {};
        string kind = "bus";
        if (ListAllBusses == null || ListAllBusses.Count == 0)
        {
            ColorPrint.PrintRed("Lege data.");
            Console.WriteLine("U keert terug naar het admin hoofd menu.\n");
            Thread.Sleep(3000);
            AdminStartMenu.Start();
        }
        else
        {
            while(true){
                (List<string> SelectedRow, int SelectedRowIndex)? TableInfo= tableBus.PrintTable(Header, ListAllBusses, GenerateRow, Title, Listupdater, kind);
                if(TableInfo != null)
                {
                    int selectedRowIndex = TableInfo.Value.SelectedRowIndex;
                    List<string> SelectedRow = TableInfo.Value.SelectedRow;
                    if(selectedRowIndex ==  ListAllBusses.Count())
                    {
                        BusModel newBusModel = new(busLogic.GenerateNewId(),0,"", false);
                        busLogic.UpdateList(newBusModel);
                        continue;
                    }
                    while(true){
                        (string SelectedItem, int SelectedIndex)? result = tableBus.PrintSelectedRow(SelectedRow, Header);
                        if (result != null){
                            string selectedItem = result.Value.SelectedItem;
                            int selectedIndex = result.Value.SelectedIndex;
                            if (selectedIndex == 0){
                                Console.WriteLine($"U kan {Header[selectedIndex]} niet aanpassen.");
                                Thread.Sleep(3000);
                            }
                            else if(selectedIndex == 1){
                                Console.WriteLine("Voer iets in om het item te veranderen:");
                                string Input = Console.ReadLine();
                                
                                //variable to check licensePlate
                                bool licensePlateExists = false;

                                // Check if the input license plate already exists
                                foreach(var bus in ListAllBusses) {
                                    if(Input == bus.LicensePlate) {
                                        licensePlateExists = true;
                                        break;
                                    }
                                }

                                if(licensePlateExists) {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Kenteken bestaat al, geef een andere op.");
                                    Console.ResetColor();
                                    Thread.Sleep(3000);
                                } else {
                                    //if licensePlate does not exists, it gets added to the list
                                    ListAllBusses[selectedRowIndex].LicensePlate = Input;
                                    busLogic.UpdateList(ListAllBusses[selectedRowIndex]);
                                    break;
                                }
                            }
                            else if(selectedIndex == 2){
                                while (true){
                                Console.WriteLine("Voer een nummer in het item te veranderen:");
                                string Input = Console.ReadLine();
                                bool containsOnlyNumbers = Input.All(char.IsDigit);
                                if (containsOnlyNumbers){
                                    ListAllBusses[selectedRowIndex].Seats = Convert.ToInt32(Input);
                                    busLogic.UpdateList(ListAllBusses[selectedRowIndex]);
                                    break;
                                    }
                                }
                                break;
                            }
                            else if (selectedIndex == 3)
                            {
                                Console.Clear();
                                RoutesList = ListAllBusses[selectedRowIndex].Route;
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
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.Write(" Spatie.");
                                    Console.ResetColor();
                                    Console.Write("\nAls u de laatste route wil verwijderen klik op");
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write(" Backspace.");
                                    Console.ResetColor();
                                    Console.Write("\nAls u tevreden bent met de routelijst, voeg de lijst toe met");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write(" Enter.");
                                    Console.ResetColor();

                                    keyInfo = Console.ReadKey(true);
                                    
                                    switch (keyInfo.Key)
                                    {
                                        case ConsoleKey.Spacebar:
                                            Console.WriteLine("\nU heeft op Spatie geklikt. Voeg een nieuwe route toe.");
                                            RouteModel Input = AdminRouteMenu.SelectRoute();
                                            if (Input != null){
                                                RoutesList.Add(Input);
                                                Console.WriteLine($"{Input.Name} is toegevoegd");
                                                ListAllBusses[selectedRowIndex].Route = RoutesList;
                                                busLogic.UpdateList(ListAllBusses[selectedRowIndex]);
                                                Thread.Sleep(2000);
                                                break;
                                                }
                                            else{
                                                Console.WriteLine("U keert terug");
                                                Thread.Sleep(2000);
                                                break;
                                            }
                                        case ConsoleKey.Backspace:
                                            if (LastRouteIndex >= 1)
                                            {
                                                Console.WriteLine("\nU heeft op Backspace geklikt. De laatste route is verwijderd");
                                                RoutesList.Remove(RoutesList[LastRouteIndex-1]);
                                                Thread.Sleep(2000);
                                            }
                                            else
                                            {
                                                Console.WriteLine("\nGeen routes om te verwijderen.");
                                                Thread.Sleep(1000);
                                            }
                                            break;
                                        case ConsoleKey.Enter:
                                            Console.WriteLine("\nU heeft op Enter geklikt. De routelijst is toegevoegd");
                                            Thread.Sleep(2000);
                                            ListAllBusses[selectedRowIndex].Route = RoutesList;
                                            busLogic.UpdateList(ListAllBusses[selectedRowIndex]);
                                            SelectedRow = GenerateRow(ListAllBusses[selectedRowIndex]);

                                            break;
                                        default:
                                            Console.WriteLine("\nOngeldige invoer.");
                                            Thread.Sleep(1000);
                                            break;
                                    }
                                } while (keyInfo.Key != ConsoleKey.Enter);
                            }
                            else{
                                Console.WriteLine("");
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
            Thread.Sleep(2000);
            Console.Clear();
            return false;
        }

        Console.WriteLine(!IsUpdate ? $"U staat op het punt een nieuwe bus toe te voegen met de volgende info: zitplaatsen: {newBus.Seats}, Kenteken: {newBus.LicensePlate}" : $"U staat op het punt oude data te veranderen: {UpdatedValue}");
        Console.Write("Druk op ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Enter");
        Console.ResetColor();
        Console.Write(" om door te gaan of druk op ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Backspace");
        Console.ResetColor();
        Console.WriteLine(" om te annuleren.");

        ConsoleKeyInfo keyInfo;
        do
        {
            keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                ColorPrint.PrintRed("Toevoegen geannuleerd.");
                Thread.Sleep(2000);
                Console.Clear();
                return false;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                ColorPrint.PrintGreen("Data is toegevoegd!");
                Thread.Sleep(2000);
                Console.Clear();
                return true;
            }
            else
            {
                ColorPrint.PrintRed("Ongeldige invoer!");
                Thread.Sleep(2000);
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
        var Actief = busModel.IsActive;
        string Activiteit = "";
        if (Actief)
        {
            Activiteit = "Actief";
        }
        else{
            Activiteit = "Non-actief";
        }
        
        return new List<string> { $"{id}", $"{licensePlate}", $"{seats}", $"{routeNames}",$"{Activiteit}" };
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
}   
