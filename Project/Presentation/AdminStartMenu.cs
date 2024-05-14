using System;
public static class AdminStartMenu
{
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
                    selectedOption = Math.Min(6, selectedOption + 1);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    // Perform action based on selected option (e.g., execute corresponding function)
                    switch (selectedOption)
                    {
                        case 1:
                            RouteMenu.Welcome();
                            break;
                        case 2:
                            PriceMenu.Start();
                            break;
                        case 3:
                            BusMenu.Start();
                            break;
                        case 4:
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
        Console.WriteLine("Selecteer een optie:");

        // Display option 1
        Console.ForegroundColor = selectedOption == 1 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 1 ? ">> " : "   ");
        Console.WriteLine("[1] Menu voor routes toevoegen");

        // // Display option 2
        Console.ForegroundColor = selectedOption == 2 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 2 ? ">> " : "   ");
        Console.WriteLine("[2] Menu voor prijscategorieën");

        // // Display option 3
        Console.ForegroundColor = selectedOption == 3 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 3 ? ">> " : "   ");
        Console.WriteLine("[3] Menu voor bussen");

        // Display option 4
        Console.ForegroundColor = selectedOption == 4 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 4 ? ">> " : "   ");
        Console.WriteLine("[4] Ga terug naar het vorige menu.");

        // Reset text color
        Console.ResetColor();
    }
}