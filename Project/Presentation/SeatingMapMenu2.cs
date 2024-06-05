using System.Formats.Asn1;
using System.Text.Json.Serialization;

public static class SeatingMapMenu2
{
    public static SeatLogic seatLogic = new();

    static private AccountsLogic accountsLogic = new AccountsLogic();

    // static Dictionary<(int Row, int Col), SeatModel> seatingMap = new ()
    // {
    //     {(0,0), new SeatModel(1)},
    //     {(0,1), new SeatModel(2)},
    //     {(0,2), new SeatModel(3)}
    // };


    // public static void Main()
    // {
    //     SeatModel[,] seatModels = new SeatModel[6, 10];
    //     seatLogic.CreateSeats(seatModels);
    //     Start(seatModels);

    //     seatLogic.PrintArr(seatLogic.ConvertTo2DArr(seatingMap));
    //     // SeatModel[,] seatModels = new SeatModel[6, 10];
    //     // CreateSeats();
    //     // Start(seatModels);
    // }



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
                    if (selectedOption.Row == seatModels.GetLength(0) / 2 - 1 /* if selectedOption.Row == 2*/)
                    {
                        selectedOption =  (Math.Min(rowLength, selectedOption.Row + 2), selectedOption.Col);
                        break;
                    }
                    selectedOption = (Math.Min(rowLength, selectedOption.Row + 1), selectedOption.Col); ;
                    break;
                case ConsoleKey.RightArrow:
                    // Move right
                    if (selectedOption.Row == 1 && (selectedOption.Col >= 2 && selectedOption.Col <= 4) /* if selectedOption.Col == 4*/)
                    {
                        selectedOption =  (Math.Max(0, selectedOption.Col + 4), selectedOption.Row);
                        break;
                    }
                    selectedOption = (selectedOption.Row, Math.Min(colLength, selectedOption.Col + 1));
                    break;
                case ConsoleKey.LeftArrow:
                    // Move left
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
                    ReservationModel reservation = new (accountsLogic.GenerateNewReservationId(UserLogin.loggedInAccount), stopModel.Name, routeModel.Name, busModel.LicensePlate);
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

                    Thread.Sleep(3000);
                    return;
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

        for (int row = 0; row < seatModels.GetLength(0); row++)
        {
            for (int col = 0; col < seatModels.GetLength(1); col++)
            {
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
                }
                else
                {
                    Console.ForegroundColor = selectedOption.Row == row && selectedOption.Col == col ? ConsoleColor.Red : ConsoleColor.White;
                    Console.Write(selectedOption.Row == row && selectedOption.Col == col ? " X " : "   ");
                }
            }
            Console.WriteLine();
        }
        Console.ResetColor();
        ColorPrint.PrintWriteRed("\n* ");
        Console.WriteLine("bezet");
        ColorPrint.PrintWriteCyan("* ");
        Console.WriteLine("geselecteerd");
        Console.WriteLine("- beschikbaar");

        Console.Write("\nKlik");
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
}

