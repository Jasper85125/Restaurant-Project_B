using System;
public static class Menu
{
    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    public static void Start()
    {
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
                    // Perform action based on selected option (e.g., execute corresponding function)
                    Console.WriteLine("Option " + selectedOption + " selected.");
                    switch (selectedOption)
                    {
                        case 1:
                            UserLogin.Start();
                            break;
                        case 2:
                            UserSignUp.Start();
                            break;
                        case 3:
                            Information.Info();
                            break;
                        case 4:
                            RouteMenu.Welcome();
                            break;
                        case 5:
                            PriceMenu.Start();
                            break;
                        case 6:
                            BusMenu.Start();
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
        Console.WriteLine("Select an option:");

        // Display option 1
        Console.ForegroundColor = selectedOption == 1 ? ConsoleColor.Green: ConsoleColor.White;
        Console.Write(selectedOption == 1 ? ">> " : "   ");
        Console.WriteLine("[1] Inloggen op uw account.");

        // Display option 2
        Console.ForegroundColor = selectedOption == 2 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 2 ? ">> " : "   ");
        Console.WriteLine("[2] Account aanmaken.");

        // Display option 3
        Console.ForegroundColor = selectedOption == 3 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 3 ? ">> " : "   ");
        Console.WriteLine("[3] informatie over de app.");

        // Display option 4
        Console.ForegroundColor = selectedOption == 4 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 4 ? ">> " : "   ");
        Console.WriteLine("[4] Menu voor routes toevoegen");

        // Display option 5
        Console.ForegroundColor = selectedOption == 5 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 5 ? ">> " : "   ");
        Console.WriteLine("[5] Menu voor prijscategorieën");

        // Display option 6
        Console.ForegroundColor = selectedOption == 6 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write(selectedOption == 6 ? ">> " : "   ");
        Console.WriteLine("[6] Menu voor bussen");

        // Reset text color
        Console.ResetColor();
    }
}