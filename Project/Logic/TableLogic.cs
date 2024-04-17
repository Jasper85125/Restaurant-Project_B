using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualBasic;

public class TableLogic<T>
{
    public static int tableWidth = 145;
    public static int selectedOption = 1;

    public void PrintTable(List<string> Header, IEnumerable<T> Data, Func<T, List<string>> GenerateRow)
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
                    Console.WriteLine("Bewerk de geselecteerde rij..."); 
                    PrintSelectedRow(geselecteerdeRow, Header);

                    Console.WriteLine("Klik op een knop om verder te gaan...");
                    Console.ReadKey(true);
                    break;
            }
        } while (keyInfo.Key != ConsoleKey.Backspace);
        Console.WriteLine("U keert terug naar het menu...");
    }
        
    public void PrintSelectedRow(List<string> selectedRow, List<string> header){
        PrintLine();
        PrintRow(header);
        PrintLine();
        Console.ForegroundColor = ConsoleColor.Green;
        PrintRow(selectedRow);
        Console.ResetColor();
        PrintLine();
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