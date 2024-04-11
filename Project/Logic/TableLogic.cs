using System;
using System.Collections.Generic;
using System.Data;

public class TableLogic<T>
{
    public static int tableWidth = 73;

    public void PrintTable(List<string> Header, IEnumerable<T> Data, Func<T, List<string>> GenerateRow)
    {
        PrintLine();
        PrintRow(Header);
        PrintLine();
        foreach (T obj in Data)
        {
            PrintRow(GenerateRow(obj));
            PrintLine();
        }
    }

    public static void PrintLine()
    {
        Console.WriteLine(new string('-', tableWidth));
    }

    public static void PrintRow(List<string> columns)
    {
        int width = (tableWidth - columns.Count) / columns.Count;
        string row = "|";

        foreach (string column in columns)
        {
            row += AlignCentre(column, width) + "|";
        }

        Console.WriteLine(row);
    }

    public static string AlignCentre(string text, int width)
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