
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

    public static bool IsValidDate(object input)
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
    
    public static bool IsOnlyLetterSpaceDash(object input)
    {
        if (!IsValidString(input)) return false;

        try
        {
            string? str = input as string;
            if (str == null) return false;

            char charFirst = str[0]; // first character should be a valid character
            if (!char.IsLetter(charFirst)) return false;

            char charLast = str[^1]; // last character should be a valid character
            if (!char.IsLetter(charLast)) return false;

            return str.All(c => char.IsLetter(c) || c == ' ' || c == '-');
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