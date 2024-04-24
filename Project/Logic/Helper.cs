using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

public static class Helper
{
    public static bool IsString(object parameter)
    {
        return parameter.GetType() == typeof(string);
    }

    public static bool IsInteger(object parameter)
    {
        return parameter.GetType() == typeof(int);
    }


    public static void function(string message, Func<bool> func)
    {
        try
        {
            func();
        }
        catch
        {
            Console.WriteLine(message);
        }
    }
}