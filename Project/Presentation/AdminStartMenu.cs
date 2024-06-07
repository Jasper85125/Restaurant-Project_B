using System;
public static class AdminStartMenu
{
    private static SeatLogic seatLogic = new();
    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
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
                    // Perform action based on selected option
                    switch (selectedOption)
                    {
                        case 1:
                            AdminRouteMenu.Start();
                            break;
                        case 2:
                            AdminPriceMenu.Start();
                            break;
                        case 3:
                            AdminBusMenu.Start();
                            break;
                        case 4:
                            UserSignUp.SignUp(true);
                            break;
                        case 5:
                            CustomerStartMenu.Start();
                            break;
                    }
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    BackToLogInMenu();
                    break;
            }

            // Clear console and display options
            Console.Clear();
            DisplayOptions(selectedOption);
        }
    }

    static void DisplayOptions(int selectedOption)
    {
        Console.WriteLine("Adminhoofdmenu");
        Console.WriteLine("Selecteer een optie:\n");

        // Display option 1
        Console.ForegroundColor = selectedOption == 1 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 1 ? ">> " : "   ");
        Console.WriteLine("Menu voor routes.");

        // Display option 2
        Console.ForegroundColor = selectedOption == 2 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 2 ? ">> " : "   ");
        Console.WriteLine("Menu voor prijscategorieën.");

        // Display option 3
        Console.ForegroundColor = selectedOption == 3 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 3 ? ">> " : "   ");
        Console.WriteLine("Menu voor bussen.");

        // Display option 4
        Console.ForegroundColor = selectedOption == 4 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 4 ? ">> " : "   ");
        Console.WriteLine("Maak een nieuw adminaccount aan.");

        Console.ForegroundColor = selectedOption == 5 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 5 ? ">> " : "   ");
        Console.WriteLine("Klantmenu.");

        // Reset text color
        Console.ResetColor();

        Console.Write("\nKlik");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(" Enter ");
        Console.ResetColor();
        Console.WriteLine("om een optie te selecteren");
        Console.Write("Om naar het inlogmenu te gaan klik op");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(" Escape");
        Console.ResetColor();
        Console.WriteLine(".\n");
    }

    public static void BackToLogInMenu()
    {
        ColorPrint.PrintYellow("U keert terug naar het inlogmenu.");
        Thread.Sleep(3000);
        Menu.Start();
    }
}