using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualBasic;

public class TableLogic<T>
{
    public static int tableWidth = 145;
    public static int selectedOption = 1;

    public List<string> PrintTable(List<string> Header, IEnumerable<T> Data, Func<T, List<string>> GenerateRow)
    {
        ConsoleKeyInfo keyInfo;
        List<string> geselecteerdeRow = new List<string>();

        do
        {
            Console.Clear(); // Clear the console to redraw the table

            PrintLine();
            PrintRow(Header);
            PrintLine();

            int rowNumber = 1;
            foreach (T obj in Data)
            {
               if (rowNumber == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    geselecteerdeRow = GenerateRow(obj);
                    PrintRow(geselecteerdeRow);
                    Console.ResetColor();
                    PrintLine();
                }
                else
                {
                    PrintRow(GenerateRow(obj));
                    PrintLine();
                }
                rowNumber++;
            }
            SelectionExplanation();

            keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedOption = Math.Max(1, selectedOption - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedOption = Math.Min(Data.Count(), selectedOption + 1);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear(); // Console leegmaken voordat je de rij bewerkt
                    return geselecteerdeRow;
            }
        } while (keyInfo.Key != ConsoleKey.Backspace);
        Console.WriteLine("U keert terug naar het menu");
        return null;
    }
        
    public (string,int)? PrintSelectedRow(List<string> selectedRow, List<string> header)
    {
        int selectedIndex = 0;
        ConsoleKeyInfo key;

        do
        {
            Console.Clear();
            Console.WriteLine("Geselecteerde rij:");
            PrintRow(header);
            PrintLine();
            PrintRowForSelected(selectedRow, selectedIndex); // Pass selectedIndex to highlight the selected item
            PrintLine();

            key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    selectedIndex = Math.Max(0, selectedIndex - 1);
                    break;
                case ConsoleKey.RightArrow:
                    selectedIndex = Math.Min(selectedRow.Count - 1, selectedIndex + 1);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    Console.WriteLine($"Geselecteerd {header[selectedIndex]}: {selectedRow[selectedIndex]}");
                    Console.ReadLine();
                    return (selectedRow[selectedIndex],selectedIndex);
                case ConsoleKey.Backspace:
                    return null;
            }

        } while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace);

        return null;
    }
            
    private static void PrintLine()
    {
        Console.WriteLine(new string('-', tableWidth));
    }

    private static void PrintRow(List<string> columns)
    {
        int width = (tableWidth - columns.Count) / columns.Count;
        string row = "|";

        foreach (string column in columns)
        {
            row += AlignCentre(column, width) + "|";
        }

        Console.WriteLine(row);
    }

    private static void PrintRowForSelected(List<string> columns, int selectedIndex)
    {
        int width = (tableWidth - columns.Count) / columns.Count;
        string row = "|";

        for (int i = 0; i < columns.Count; i++)
        {
            string column = columns[i];
            if (i == selectedIndex) // Check if current column index matches selectedIndex
            {
                    Console.ForegroundColor = ConsoleColor.Green;
                    row += ">> " + AlignCentre(column, width - 3) + "|"; // Example: Prefix with ">>"
                    Console.ResetColor();
   
            }
            else
            {
                row += AlignCentre(column, width) + "|";
            }
        }

        Console.WriteLine(row);
    }


    private static string AlignCentre(string text, int width)
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

    private static void SelectionExplanation(){
        Console.WriteLine("Selecteer een rij doormiddel van de pijltjes.");
        Console.Write("Als je de juiste rij hebt geselecteerd klik op ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Enter");
        Console.ResetColor();
        Console.Write(" om de rij te bewerken.");

    }
}