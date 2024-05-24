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
                    selectedOption = Math.Min(5, selectedOption + 1);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    // Perform action based on selected option
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
                        case 5:
                            Menu.Start();
                            break;
                        default:
                            Console.WriteLine("Verkeerde input!");
                            Thread.Sleep(3000);
                            Start();
                            break;
                    }
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
        Console.WriteLine("Selecteer een optie:");

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

        // Display option 5
        Console.ForegroundColor = selectedOption == 5 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 5 ? ">> " : "   ");
        Console.WriteLine("Keer terug naar de beginpagina.");

        // Reset text color
        Console.ResetColor();
    }
}
