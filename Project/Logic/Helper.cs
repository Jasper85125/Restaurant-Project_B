using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

public static class Helper
{
    public static bool IsString(object parameter)
    {
        return parameter.GetType() == typeof(string);
    }

    public static void funtion(string message, Func<bool> func)
    {
        try
        {
            func();
        }
        catch
        {
            System.Console.WriteLine(message);
        }
    }
}