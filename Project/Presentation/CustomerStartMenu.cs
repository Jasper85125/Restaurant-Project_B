public class CustomerStartMenu
{
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
                    selectedOption = Math.Min(4, selectedOption + 1);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    // Perform action based on selected option (e.g., execute corresponding function)
                    switch (selectedOption)
                    {
                        case 1:
                            CustomerRouteMenu.Start();
                            break;
                        case 2:                            
                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                        default:
                            Console.WriteLine("Verkeerde input!");
                            Thread.Sleep(3000);
                            Start();
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

    public static void DisplayOptions(int selectedOption)
    {
        Console.WriteLine("Welkom op de startpagina");
        Console.WriteLine("Selecteer een optie:\n");

        // Display option 1
        Console.ForegroundColor = selectedOption == 1 ? ConsoleColor.Green: ConsoleColor.White;
        Console.Write(selectedOption == 1 ? ">> " : "   ");
        Console.WriteLine("Bekijk het overzicht voor reserveringen.");

        // Display option 2
        Console.ForegroundColor = selectedOption == 2 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 2 ? ">> " : "   ");
        Console.WriteLine("Placeholder.");

        // Display option 3
        Console.ForegroundColor = selectedOption == 3 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 3 ? ">> " : "   ");
        Console.WriteLine("Placeholder.");

        // Display option 4
        Console.ForegroundColor = selectedOption == 4 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 4 ? ">> " : "   ");
        Console.WriteLine("Placeholder.");

        // Reset text color
        Console.ResetColor();

        //Display General information
        Console.Write("\nKlik");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(" Enter ");
        Console.ResetColor();
        Console.WriteLine("om een optie te selecteren");

        Console.Write("Klik");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(" Escape ");
        Console.ResetColor();
    }

    public static void BackToLogInMenu()
    {
        ColorPrint.PrintYellow("U keert terug naar het inlogmenu.");
        Thread.Sleep(3000);
        Menu.Start();
    }
}
