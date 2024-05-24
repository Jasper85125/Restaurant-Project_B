using System.Drawing;

static public class Information
{
    public static void Info()
    {
        Console.WriteLine("Ons pannenkoeken restaurant is een bus waar je op");
        Console.WriteLine("meerderen plekken kan instappen zodat je kan eten terwijl de bus een rondje rijdt."); 
        Console.WriteLine("En op deze app/site kan je reserveren waar en wanneer je met ons mee wil rijden.");
        Start();
    }

    public static void Start()
    {
        Console.Write("\nKlik op");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(" Escape ");
        Console.ResetColor();
        Console.WriteLine("om terug te gaan naar het inlogmenu.");
        while (true)
        {
            // Wait for key press
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            // Check arrow key presses
            switch (keyInfo.Key)
            {
                case ConsoleKey.Escape:
                    // Move to the previous option
                    BackToStartMenu();
                    break;
            }
        }
    }

    public static void BackToStartMenu()
    {
        Console.Clear();
        ColorPrint.PrintYellow("U keert terug naar het Startmenu.");
        Thread.Sleep(500);
        Menu.Start();
    }

}
