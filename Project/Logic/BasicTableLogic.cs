using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text.Json.Nodes;
using Microsoft.VisualBasic;

public class BasicTableLogic<T>
{
    public static int tableWidth = 145;
    public static int selectedOption = 1;

    
   public (int?, string) PrintTable(List<string> Header, List<T> Data, Func<T, List<string>> GenerateRow, string Title)
    {
        ConsoleKeyInfo keyInfo;
        List<string> geselecteerdeRow = new List<string>();
        do
        {
            Console.Clear();
            Console.WriteLine($"{Title}\n");
            PrintRow(Header, false, true);
            int maxOptions = 0;
            int jsonIndex = 0;
            for (int rowNumber = 1; rowNumber <= Data.Count(); rowNumber++)
            {
                maxOptions++;
                tableWidth = 145;
                if (selectedOption == maxOptions)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    geselecteerdeRow = GenerateRow(Data[rowNumber - 1]);
                    PrintRow(geselecteerdeRow, true, false);
                    Console.ResetColor();
                    jsonIndex = rowNumber - 1;
                }
                else
                {
                    PrintRow(GenerateRow(Data[rowNumber - 1]), false, false);
                }            
            }
            SelectionExplanation();

            keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedOption = Math.Max(1, selectedOption - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedOption = Math.Min(maxOptions, selectedOption + 1);
                    break;
                case ConsoleKey.Spacebar:
                    Console.Clear();
                    return (jsonIndex, "space");
                case ConsoleKey.Enter:
                    Console.Clear();
                    return (jsonIndex, "enter");
                case ConsoleKey.Backspace:
                    Console.Clear();
                    return (jsonIndex, "backspace");
                case ConsoleKey.Escape:
                    Console.Clear();
                return (null, null);
            }
        } while (keyInfo.Key != ConsoleKey.Escape);
        
        Console.WriteLine("U keert terug naar het menu");
        return (null, null);
    }
        
    public (string,int)? PrintSelectedRow(List<string> selectedRow, List<string> header)
    {
        int selectedIndex = 0;
        ConsoleKeyInfo key;

        do
        {
            Console.Clear();
            Console.WriteLine("Geselecteerde reservering:");
            PrintLine();
            PrintRow(header, false, true);
            PrintLine();
            PrintRowForSelected(selectedRow, selectedIndex); // Pass selectedIndex to highlight the selected item
            SelectionExplanation();
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
                    return (selectedRow[selectedIndex],selectedIndex);
                case ConsoleKey.Escape:
                    return null;
            }

        } while (key.Key != ConsoleKey.Backspace);

        return null;
    }
            
    private static void PrintLine()
    {
        Console.WriteLine(new string('-', tableWidth));
    }

   private static string PrintRow(List<string> columns, bool selected, bool PrintLineBool)
    {
        int columnWidth = (tableWidth - 1 - columns.Count) / columns.Count;
        string row = "|";

        foreach (string column in columns)
        {
            if (selected)
            {

                row += $">> {AlignCentre(column, columnWidth - 6)} <<|";   
            }
            else{
                row += $"{AlignCentre(column, columnWidth)}|";
            }
        }
        if(PrintLineBool){
            tableWidth = row.Length;
            Console.WriteLine(new string('-', row.Length));
        }
        Console.WriteLine(row);
        if(PrintLineBool){
            tableWidth = row.Length;
            Console.WriteLine(new string('-',  row.Length));
        }
        return row;
    }

    private static void PrintRowForSelected(List<string> columns, int selectedIndex)
    {
        int width = (tableWidth - columns.Count) / columns.Count;
        Console.Write("|");
        string output = "|";

        for (int i = 0; i < columns.Count; i++)
        {
            string column = columns[i];
            if (i == selectedIndex) // Check if current column index matches selectedIndex
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(">> " + AlignCentre(column, width - 6) + " <<"); // Example: Prefix and suffix with ">>" and "<<"
                Console.ResetColor();
                Console.Write("|");
                output += $"{" >> " + AlignCentre(column, width - 6) + " <<"}|";

            }
            else
            {
                Console.Write(AlignCentre(column, width) + "|");
                output += $"{AlignCentre(column, width)}|";
            }
        }
        Console.WriteLine();
        Console.WriteLine(new string('-', output.Length-1));
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
            Console.Write(" om de rij te selecteren.");
            Console.Write("\nOm uw reservering te annuleren klik op ");
            ColorPrint.PrintWriteCyan("backspace");
            Console.Write("\nOm terug te gaan klik op ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Escape");
            Console.ResetColor();
            Console.Write(".\n");
    }
}