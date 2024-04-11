using System;
using System.Collections.Generic;
using System.Data;
class TableLogic<T>
{
    static int tableWidth = 73;

   public void PrintTable(string[] Header, IEnumerable<T> Data, Func<T, string[]> GenerateRow)
    {
        PrintLine();
        PrintRow(Header);
        PrintLine();
        foreach(T Object in Data)
        {
            PrintRow(GenerateRow(Object));
            PrintLine();
        }
    }


    static void PrintLine()
    {
        Console.WriteLine(new string('-', tableWidth));
    }

    static void PrintRow(params string[] columns)
    {
        int width = (tableWidth - columns.Length) / columns.Length;
        string row = "|";

        foreach (string column in columns)
        {
            row += AlignCentre(column, width) + "|";
        }

        Console.WriteLine(row);
    }

    static string AlignCentre(string text, int width)
    {
        text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

        if (string.IsNullOrEmpty(text))
        {
            return new string(' ', width);
        }
        else
        {
            return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
        }
    }
}