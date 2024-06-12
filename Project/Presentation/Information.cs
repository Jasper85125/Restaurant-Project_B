static public class Information
{
    public static void Info()
    {
        Console.WriteLine("Ons pannenkoeken restaurant is een bus waar je op meerderen plekken kan instappen");
        Console.WriteLine("zodat je kan eten terwijl de bus een rondje rijdt."); 
        Console.WriteLine("En op deze app/site kan je reserveren waar en wanneer je met ons mee wil rijden.");
        Start();
    }

    public static void Start()
    {
        Console.Write("\nKlik op");
        ColorPrint.PrintWriteRed(" Escape ");
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
                    Menu.Start();
                    break;
            }
        }
    }
}
