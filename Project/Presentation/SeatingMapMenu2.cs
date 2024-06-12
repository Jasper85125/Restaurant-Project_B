public static class SeatingMapMenu2
{
    public static SeatLogic seatLogic = new();
    
    private static BusLogic busLogic = new();

    static private AccountsLogic accountsLogic = new AccountsLogic();

    public static void Start(SeatModel[,] seatModels, BusModel busModel, RouteModel routeModel, StopModel stopModel)
    {
        List<(int Row, int Col)> selectedSeats = new ();
        
        int rowLength = seatModels.GetLength(0) - 1;
        int colLength = seatModels.GetLength(1) - 1;

        Console.Clear();
        (int Row, int Col) selectedOption = new(0, 0); // Default selected option

        // Display options
        DisplayOptions(selectedOption, seatModels, selectedSeats);

        while (true)
        {
            // Wait for key press
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            // Check arrow key presses
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    // Move up
                    if (selectedOption.Row == 5 && (selectedOption.Col >= 2 && selectedOption.Col <= 4) /* if selectedOption.Row == 4*/)
                    {
                        selectedOption =  (Math.Max(0, selectedOption.Row - 4), selectedOption.Col);
                        break;
                    }else if(selectedOption.Row == 2 && (selectedOption.Col >= 7 && selectedOption.Col <= 11))
                    {
                        selectedOption =  (Math.Max(0, selectedOption.Row - 0), selectedOption.Col);
                        break;
                    }
                    if (selectedOption.Row == seatModels.GetLength(0) / 2 + 1 /* if selectedOption.Row == 4*/)
                    {
                        selectedOption =  (Math.Max(0, selectedOption.Row - 2), selectedOption.Col);
                        break;
                    }
                    selectedOption = (Math.Max(0, selectedOption.Row - 1), selectedOption.Col);
                    break;
                case ConsoleKey.DownArrow:
                    // Move down
                    if (selectedOption.Row == 1 && (selectedOption.Col >= 2 && selectedOption.Col <= 4) /* if selectedOption.Row == 4*/)
                    {
                        selectedOption =  (Math.Max(0, selectedOption.Row + 4), selectedOption.Col);
                        break;
                    
                    }
                    else if(selectedOption.Row == 4 && (selectedOption.Col >= 7 && selectedOption.Col <= 11))
                    {
                        selectedOption =  (Math.Max(0, selectedOption.Row - 0), selectedOption.Col);
                        break;
                    }
                    if (selectedOption.Row == seatModels.GetLength(0) / 2 - 1 /* if selectedOption.Row == 2*/)
                    {
                        selectedOption =  (Math.Min(rowLength, selectedOption.Row + 2), selectedOption.Col);
                        break;
                    }
                    selectedOption = (Math.Min(rowLength, selectedOption.Row + 1), selectedOption.Col); ;
                    break;
                case ConsoleKey.RightArrow:
                    // Move right
                    if (selectedOption.Col == 1 && (selectedOption.Row == 2 || selectedOption.Row == 4) /* if selectedOption.Col == 4*/)
                    {
                        selectedOption =  (Math.Max(0, selectedOption.Row), selectedOption.Col + 4);
                        break;
                    }
                    else if(selectedOption.Col == 6 && (selectedOption.Row == 0 || selectedOption.Row == 1 || selectedOption.Row == 5 || selectedOption.Row == 6))
                    {
                        selectedOption = (selectedOption.Row, Math.Min(colLength, selectedOption.Col + 0));
                    }else{
                        selectedOption = (selectedOption.Row, Math.Min(colLength, selectedOption.Col + 1));
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    // Move left
                    if (selectedOption.Col == 5 && (selectedOption.Row == 2 || selectedOption.Row == 4) /* if selectedOption.Col == 4*/)
                    {
                        selectedOption =  (Math.Min(rowLength, selectedOption.Row), selectedOption.Col - 4);
                        break;
                    }
                    selectedOption = (selectedOption.Row, Math.Max(0, selectedOption.Col - 1));
                    break;
                case ConsoleKey.Spacebar:
                    Console.Clear();
                    if (seatModels[selectedOption.Row, selectedOption.Col].IsOccupied)
                    {
                        ColorPrint.PrintRed("Is al bezet!");
                        Thread.Sleep(3000);
                    }
                    else
                    {
                        // seatModels[selectedOption.Row, selectedOption.Col].IsOccupied = true;
                        if (!selectedSeats.Contains((selectedOption.Row, selectedOption.Col)))
                        {
                            selectedSeats.Add((selectedOption.Row, selectedOption.Col));
                        }

                    }
                    break;
                case ConsoleKey.Enter:
                    ReservationModel reservation = new (accountsLogic.GenerateNewReservationId(UserLogin.loggedInAccount), stopModel.Name, routeModel.Name, busModel.Id);
                    switch(JaNee())
                    {
                        case true:
                            foreach ((int Row, int Col) coordinaten in selectedSeats)
                            {
                                seatModels[coordinaten.Row, coordinaten.Col].IsOccupied = true;
                                reservation.AddSeatRow(coordinaten.Row);
                                reservation.AddSeatCol(coordinaten.Col);
                            }
                            if (reservation.SeatCol != null && reservation.SeatCol.Count != 0)
                            {
                                UserLogin.loggedInAccount.Reservations.Add(reservation);
                                accountsLogic.UpdateList(UserLogin.loggedInAccount);
                            }
                            if (selectedSeats.Count == 1)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"U heeft {selectedSeats.Count} stoel gereserveerd.");
                                Console.ResetColor();
                            }
                            else if (selectedSeats.Count > 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"U heeft {selectedSeats.Count} stoelen gereserveerd.");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"U heeft geen stoelen gereserveerd!");
                                Console.ResetColor();
                            }
                            break;
                        case false:
                            SeatingMapMenu2.Start(seatModels, busModel, routeModel, stopModel);
                            break;
                    }
                    Console.WriteLine("");
                    ColorPrint.PrintYellow("U keert terug naar het overzicht!");
                    Dictionary<(int Row, int Col), SeatModel> updatedSeatingMap = seatLogic.ConvertToDict(seatModels);
                    busModel.SeatingMap = updatedSeatingMap;
                    busLogic.UpdateList(busModel);
                    Thread.Sleep(3000);
                    CustomerRouteMenu.Start();
                    break;
                case ConsoleKey.Backspace:
                    try
                    {
                        if (selectedSeats.Contains((selectedOption.Row, selectedOption.Col)))
                        {
                            selectedSeats.Remove((selectedOption.Row, selectedOption.Col));
                        }
                    }
                    catch
                    {

                    }
                    break;

                case ConsoleKey.Escape:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("U gaat terug naar het overzicht voor reserveringen.");
                    Thread.Sleep(3000);
                    Console.ResetColor();
                    CustomerRouteMenu.PrintedOverview();
                    break;

            }
            // Clear console and display options
            Console.Clear();
            DisplayOptions(selectedOption, seatModels, selectedSeats);
        }
    }

    public static void DisplayOptions((int Row, int Col) selectedOption, SeatModel[,] seatModels,  List<(int Row, int Col)> selectedSeats)
    {
        Console.WriteLine("Selecteer een optie:\n");

        // Print top border
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("         Party        |     Business");
        Console.ResetColor();
        Console.WriteLine(" " + new string('=', seatModels.GetLength(1) * 3 + 4));        
        for (int row = 0; row < seatModels.GetLength(0); row++)
        {
            if (row == 1 || row == 5)
                ColorPrint.PrintWriteRed("|"); // Print the left side
            else
                Console.Write("|"); // Print the left side
            for (int col = 0; col < seatModels.GetLength(1); col++)
            {

                if (col == 7) // Assuming the split happens after the 3rd column, adjust as necessary
                {
                    Console.Write("|"); // Add the vertical line separator
                }
                if(seatModels[row, col] != null)
                {
                    if (seatModels[row, col].IsOccupied)
                    {
                        Console.ForegroundColor = selectedOption.Row == row && selectedOption.Col == col ? ConsoleColor.Green : ConsoleColor.Red;
                        Console.Write(selectedOption.Row == row && selectedOption.Col == col ? " * " : " * ");
                    }
                    else if(selectedSeats.Contains((row, col)))
                    {
                        Console.ForegroundColor = selectedOption.Row == row && selectedOption.Col == col ? ConsoleColor.Green : ConsoleColor.Cyan;
                        Console.Write(selectedOption.Row == row && selectedOption.Col == col ? " * " : " * ");
                    }
                    else
                    {
                        Console.ForegroundColor = selectedOption.Row == row && selectedOption.Col == col ? ConsoleColor.Green : ConsoleColor.White;
                        Console.Write(selectedOption.Row == row && selectedOption.Col == col ? " O " : " - ");
                    }
                    Console.ResetColor();
                }
                else
                {
                    Console.Write("   "); // Space for the pad
                }
            }
            if (row == 1 || row == 5)
                ColorPrint.PrintYellow("   |"); // Print the right side
            else
                Console.WriteLine("   |"); // Print the right side
        }

        // Print bottom border
        int totalColumns = seatModels.GetLength(1) * 3 + 4;
        for (int i = 0; i < totalColumns; i++)
        {
            if (i == 10 || i == 11 || i == 12 || i == 38 || i == 39)
            {
                Console.Write(" ");
            }
            else
            {
                Console.Write("=");
            }
        }
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("  Achterkant --------------> Voorkant");
        Console.ResetColor();
        Console.ResetColor();
        ColorPrint.PrintWriteRed("\n* ");
        Console.WriteLine("bezet");
        ColorPrint.PrintWriteCyan("* ");
        Console.WriteLine("geselecteerd");
        Console.WriteLine("- beschikbaar");

        Console.WriteLine("\nOm te bewegen door de bus gebruik de pijltjes toetsen.");


        Console.Write("Klik");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(" spatie ");
        Console.ResetColor();
        Console.WriteLine("om een optie te selecteren.");
        Console.Write("Klik");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(" enter ");
        Console.ResetColor();
        Console.WriteLine("om een optie te bevestigen.");

        Console.Write("Klik");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(" backspace ");
        Console.ResetColor();
        Console.WriteLine("om te deselecteren.");

        Console.Write("\nOm een stap terug te gaan druk op");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(" Escape\n");
        Console.ResetColor();
    }

    public static bool JaNee()
    {
        Console.Clear();
        int selectedOption = 1;

        DisplayOptionsJaNee(selectedOption);

        while (true)
        {
            // Console.Write("\nOm terug te keren klik op");
            // ColorPrint.PrintWriteRed(" Escape");
            // Console.WriteLine(".\n");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedOption = Math.Max(1, selectedOption - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedOption = Math.Min(2, selectedOption + 1);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    switch (selectedOption)
                    {
                        case 1:
                            return true;
                        case 2:
                            return false;
                    }
                    break;
            }
            Console.Clear();
            DisplayOptionsJaNee(selectedOption);
        }
    }
    public static void DisplayOptionsJaNee(int selectedOption)
    {
        Console.WriteLine($"Weet u zeker dat u deze stoel(en) wilt reserveren.");
        Console.WriteLine("Selecteer een optie:");

        // Display option 1
        Console.ForegroundColor = selectedOption == 1 ? ConsoleColor.Green: ConsoleColor.White;
        Console.Write(selectedOption == 1 ? ">> " : "   ");
        Console.WriteLine("Ja.");

        // Display option 2
        Console.ForegroundColor = selectedOption == 2 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 2 ? ">> " : "   ");
        Console.WriteLine("Nee.");

        Console.ResetColor();
    }

}