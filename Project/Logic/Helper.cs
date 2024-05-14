
public static class Helper
{
    public static bool IsValidString(object input)
    {
        if (input is string str)
        {
            return !string.IsNullOrEmpty(str);
        }
        return false;
    }

    public static bool IsValidInteger(object input)
    {
       if(!IsValidString(input)) return false;

        try
        {
            string? str = input as string;
            int integerValue = Convert.ToInt32(str);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidDouble(object input)
    {
        if(!IsValidString(input)) return false;
        try
        {
            string? str = input as string;
            double doubleValue  = Convert.ToDouble(str);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsDateValid(object input)
    {
        if (!IsValidString(input)) return false;
        
        try
        {
            string? str = input as string;
            DateTime dateValue = Convert.ToDateTime(str); // "2024-05-06" // YYYY_MM_DD
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public static bool IsOnlyLetter(object input)
    {
        if (!IsValidString(input)) return false;

        try
        {
            string? str = input as string;
            if (str == null) return false;
            return str.All(char.IsLetter);
        }
        catch
        {
            return false;
        }
    }
    
    // public static void Function(string message, Func<bool> func)
    // {
    //     try
    //     {
    //         func();
    //     }
    //     catch
    //     {
    //         Console.WriteLine(message);
    //     }
    // }

}