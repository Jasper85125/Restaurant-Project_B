public static class ColorPrint
{
    public static void PrintGreen(string text)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static void PrintRed(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static void PrintCyan(string text)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static void PrintYellow(string text)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static void PrintMagenta(string text)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static void PrintWriteGreen(string text)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(text);
        Console.ResetColor();
    }
    public static void PrintWriteRed(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(text);
        Console.ResetColor();
    }
    public static void PrintWriteCyan(string text)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(text);
        Console.ResetColor();
    }

    public static void PrintWriteBlue(string text)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(text);
        Console.ResetColor();
    }
}