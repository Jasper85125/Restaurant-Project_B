using System.Runtime.CompilerServices;

public static class BusMenu
{
    private static TableLogic<BusModel> tableBus = new();
    private static BusLogic busLogic = new();
    private static RouteLogic routeLogic = new();
    private static TableLogic<BusModel> tableRoutes = new();
    
    


    public static void Start()
    {

        Console.Clear();
        int selectedOption = 1; // Default selected option

        // Display options
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
                            AddBus();
                            Start();
                            break;
                        case 2:
                            UpdateBus();
                            Start();
                            break;
                        case 3:
                            AddTime();
                            Start();
                            break;
                        case 4:
                            BusLogic busLogic = new();
                            List<BusModel> ListAllBusses = busLogic.GetAll();
                            ShowAllBusInformation(ListAllBusses);
                            Console.WriteLine("U keert terug naar het busmenu");
                            Thread.Sleep(1000);
                            Start();
                            break;
                        case 5:
                            Menu.Start();
                            break;
                    }
                    break;
            }

            // Clear console and display options
            Console.Clear();
            DisplayOptions(selectedOption);
        }
    }

    static void DisplayOptions(int selectedOption)
    {
        Console.WriteLine("\nWelkom bij het overzicht voor de bussen.");

        // Display option 1
        Console.ForegroundColor = selectedOption == 1 ? ConsoleColor.Green: ConsoleColor.White;
        Console.Write(selectedOption == 1 ? ">> " : "   ");
        Console.WriteLine("[1] Een bus toevoegen.");

        // Display option 2
        Console.ForegroundColor = selectedOption == 2 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 2 ? ">> " : "   ");
        Console.WriteLine("[2] Een bus updaten.");

        // Display option 3
        Console.ForegroundColor = selectedOption == 3 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 3 ? ">> " : "   ");
        Console.WriteLine("[3] Tijden toevoegen aan haltes en routes.");

        // Display option 4
        Console.ForegroundColor = selectedOption == 4 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 4 ? ">> " : "   ");
        Console.WriteLine("[4] Een overzicht van alle bussen.");

        // Display option 5
        Console.ForegroundColor = selectedOption == 5 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 5 ? ">> " : "   ");
        Console.WriteLine("[5] Ga terug naar het vorige menu.");

        // Reset text color
        Console.ResetColor();
    }

    public static List<BusModel> Overview()
    {
        BusLogic LogicInstance = new BusLogic();
        List<BusModel> overview = LogicInstance.GetAll();
        return overview;
    }

    public static void AddBus()
    {
        List<BusModel> listOfBuses = Overview();
        
        Console.WriteLine("Hoeveel zitplaatsen heeft deze bus?");
        string? newSeats = Console.ReadLine();
        
        Console.WriteLine("Wat is de kenteken van de bus?");
        string? newLicensePlate = Console.ReadLine().ToUpper();
        
        try
        {
            BusLogic newLogic = new BusLogic();
            BusModel newBus = new BusModel(newLogic.GenerateNewId(), Convert.ToInt32(newSeats), newLicensePlate);
            
            if (ConfirmValue(newBus))
            {
                newLogic.UpdateList(newBus);
            }
            else
            {
                Start();
            }
            
        }
        catch (FormatException)
        {
            Console.WriteLine("\nOngeldige invoer!");
            Thread.Sleep(3000);
            Console.Clear();
        }
    }

    public static void UpdateBus()
    {
        BusLogic loading = new BusLogic();
        Console.Clear();
        Console.WriteLine("Welk busnummer wilt U updaten?");
        ShowAllBusInformation(Overview());
        string? id_to_be_updated = Console.ReadLine();
        try
        {
            BusModel RouteObject = loading.GetById(Convert.ToInt32(id_to_be_updated));
            if (RouteObject == null)
            {
                Console.WriteLine("\nOngeldige invoer!");
                Thread.Sleep(3000);
                Console.Clear();
            }
            else
            {   
                Console.Clear();
                Console.WriteLine("Wat wilt U bijwerken?\n1. Kenteken\n2. Aantal zitplekken");
                string? option = Console.ReadLine();
                
                if (option == "1")
                {
                    Console.WriteLine("Wat is de nieuwe kenteken?");
                    string? UpdatedValue = Console.ReadLine().ToUpper();
                    if (ConfirmValue(null, UpdatedValue, true)){
                        RouteObject.LicensePlate = UpdatedValue; // Update the license plate
                    }
                }
                else if (option == "2")
                {
                    Console.WriteLine("Wat is het nieuwe aantal zitplekken?");
                    string? UpdatedValue = Console.ReadLine();
                    if (ConfirmValue(null, UpdatedValue, true)){
                        RouteObject.Seats = Convert.ToInt32(UpdatedValue); // Update the number of seats
                    }
                }
                else
                {
                    Console.WriteLine("\nOngeldige invoer!");
                    Thread.Sleep(3000);
                    Console.Clear();
                    UpdateBus();
                }
                
                loading.UpdateList(RouteObject);
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("\nOngeldige invoer!");
            Thread.Sleep(3000);
            Console.Clear();
        }
    }
    public static void ShowAllBusInformation (List<BusModel> ListAllBusses)
    {
        List<string> Header = new() { "Busnummer", "Kenteken", "Zitplaatsen", "Route(s)"};
        List<RouteModel> RoutesList = new() {};
        if (ListAllBusses == null || ListAllBusses.Count == 0)
        {
            Console.WriteLine("Lege data.");
        }
        else
        {
            while(true){
                (List<string> SelectedRow, int SelectedRowIndex)? TableInfo= tableBus.PrintTable(Header, ListAllBusses, GenerateRow);
                if(TableInfo != null){
                    int selectedRowIndex = TableInfo.Value.SelectedRowIndex;
                    while(true){
                        (string SelectedItem, int SelectedIndex)? result = tableBus.PrintSelectedRow(TableInfo.Value.SelectedRow, Header);
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
                                ListAllBusses[selectedRowIndex].LicensePlate = Input;
                                busLogic.UpdateList(ListAllBusses[selectedRowIndex]);
                                break;
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
                                    Console.Write(" Escape.");
                                    Console.ResetColor();

                                    keyInfo = Console.ReadKey(true);
                                    
                                    switch (keyInfo.Key)
                                    {
                                        case ConsoleKey.Spacebar:
                                            Console.WriteLine("\nU heeft op Spatie geklikt. Voeg een nieuwe route toe.");
                                            RouteModel Input = RouteMenu.SelectRoute();
                                            RoutesList.Add(Input);
                                            Console.WriteLine($"{Input.Name} is toegevoegd");
                                            Thread.Sleep(2000);
                                            break;
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
                                        case ConsoleKey.Escape:
                                            Console.WriteLine("\nU heeft op Escape geklikt. De routelijst is toegevoegd");
                                            Thread.Sleep(2000);
                                            ListAllBusses[selectedRowIndex].Route = RoutesList;
                                            busLogic.UpdateList(ListAllBusses[selectedRowIndex]);
                                            break;
                                        default:
                                            Console.WriteLine("\nOngeldige invoer.");
                                            Thread.Sleep(1000);
                                            break;
                                    }
                                } while (keyInfo.Key != ConsoleKey.Escape);

                                ListAllBusses[selectedRowIndex].Route = RoutesList;
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
    public static void AddTime()
    {
        ShowAllBusInformation(Overview());
        Console.WriteLine("Aan welke bus met route wilt U een tijd geven?");
        string? busID = Console.ReadLine();
        try
        {
            BusLogic LogicInstance = new BusLogic ();
            int intInputBus = Convert.ToInt32(busID);
            BusModel bus = LogicInstance.GetById(intInputBus);
            if (HasRoute(bus))
            {
                foreach (RouteModel Route in bus.Route)
                {
                    Console.WriteLine("Wat is de begintijd voor de route?");
                    string? beginTimeRoute = Console.ReadLine();
                    while (beginTimeRoute == null)
                    {
                        Console.WriteLine("Vul een correcte waarde in");
                    }
                    Route.beginTime = beginTimeRoute;

                    foreach (StopModel Stop in Route.Stops)
                    {
                        Console.WriteLine($"Wat is de tijd voor halte {Stop.Name}");
                        string? newTime = Console.ReadLine();
                        while (newTime == null)
                        {
                            Console.WriteLine("Vul een correcte waarde in");
                        }
                        Stop.Time = newTime;
                    }

                    Console.WriteLine("Wat is de eindtijd voor de route?");
                    string? endTimeRoute = Console.ReadLine();
                    while (endTimeRoute == null)
                    {
                        Console.WriteLine("Vul een correcte waarde in");
                    }
                    Route.endTime = endTimeRoute;
                }
                LogicInstance.UpdateList(bus);
            }
            else
            {
                Console.WriteLine("Deze bus heeft nog geen route aangewezen gekregen.");
                Start();
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Verkeerde input ");
        }

    }

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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(IsUpdate ? "Ongeldige invoer." : "Fout: Nieuwe busgegevens ontbreken!");
            Console.ResetColor();
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Toevoegen geannuleerd.");
                Console.ResetColor();
                Thread.Sleep(2000);
                Console.Clear();
                return false;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Data is toegevoegd!");
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
        
        return new List<string> { $"{id}", $"{licensePlate}", $"{seats}", $"{routeNames}" };
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
