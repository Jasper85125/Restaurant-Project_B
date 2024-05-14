using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualBasic;

public class TableLogic<T>
{
    public static int tableWidth = 145;
    public static int selectedOption = 0;

    
    public (List<string>,int)? PrintTable(List<string> Header, List<T> Data, Func<T, List<string>> GenerateRow)
    {   

        ConsoleKeyInfo keyInfo;
        List<string> geselecteerdeRow = new List<string>();
        List<string> NewRow = new() {"Voeg een nieuwe rij toe"};

        do
        {
            Console.Clear();

            PrintLine();
            PrintRow(Header, false);
            PrintLine();


            for (int rowNumber = 1; rowNumber <= Data.Count() + 1; rowNumber++)
            {
                if (rowNumber <= Data.Count()){
                    if (rowNumber == selectedOption)
                        {  
                            Console.ForegroundColor = ConsoleColor.Green;
                            geselecteerdeRow = GenerateRow(Data[rowNumber-1]);
                            PrintRow(geselecteerdeRow, true);
                            Console.ResetColor();
                            PrintLine();
                            
                        }
                        else
                        {
                            PrintRow(GenerateRow(Data[rowNumber-1]), false);
                            PrintLine();
                        }
                }
                else{
                    if (rowNumber == selectedOption){
                        Console.ForegroundColor = ConsoleColor.Green;
                        geselecteerdeRow = NewRow;
                        PrintRow(geselecteerdeRow, true);
                        Console.ResetColor();
                        PrintLine();
                    }
                    else{
                        PrintRow(NewRow, false);
                        PrintLine();
                    }
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
                    selectedOption = Math.Min(Data.Count() + 1, selectedOption + 1);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    return (geselecteerdeRow, selectedOption - 1);
                case ConsoleKey.Backspace:
                    return null;
            }
        } while (keyInfo.Key != ConsoleKey.Backspace);
        Console.WriteLine("U keert terug naar het menu");
        return null;
    }

    public (List<string>,int)? PrintTable(List<string> Header, List<T> Data, Func<T, List<string>> GenerateRow, string Title)
    {
        ConsoleKeyInfo keyInfo;
        List<string> geselecteerdeRow = new List<string>();
        List<string> NewRow = new() {"Voeg een nieuwe rij toe", "-", "-"};

        do
        {
            Console.Clear();
            Console.WriteLine(Title);
           PrintLine();
            PrintRow(Header, false);
            PrintLine();


            for (int rowNumber = 1; rowNumber <= Data.Count() + 1; rowNumber++)
            {
                if (rowNumber <= Data.Count()){
                    if (rowNumber == selectedOption)
                        {  
                            Console.ForegroundColor = ConsoleColor.Green;
                            geselecteerdeRow = GenerateRow(Data[rowNumber-1]);
                            PrintRow(geselecteerdeRow, true);
                            Console.ResetColor();
                            PrintLine();
                            
                        }
                        else
                        {
                            PrintRow(GenerateRow(Data[rowNumber-1]), false);
                            PrintLine();
                        }
                }
                else{
                    if (rowNumber == selectedOption){
                        Console.ForegroundColor = ConsoleColor.Green;
                        geselecteerdeRow = NewRow;
                        PrintRow(geselecteerdeRow, true);
                        Console.ResetColor();
                        PrintLine();
                    }
                    else{
                        PrintRow(NewRow, false);
                        PrintLine();
                    }
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
                    selectedOption = Math.Min(Data.Count() + 1, selectedOption + 1);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    return (geselecteerdeRow, selectedOption - 1);
                case ConsoleKey.Backspace:
                    return null;
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
            PrintLine();
            PrintRow(header, false);
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
                case ConsoleKey.Backspace:
                    return null;
            }

        } while (key.Key != ConsoleKey.Backspace);

        return null;
    }
            
    private static void PrintLine()
    {
        Console.WriteLine(new string('-', tableWidth));
    }

    private static void PrintRow(List<string> columns, bool selected)
    {
        int width = (tableWidth - columns.Count) / columns.Count;
        string row = "|";

        foreach (string column in columns)
        {
            if (selected){
                row += ">> " + AlignCentre(column, width - 6) + " <<|";
            }
            else{
                row += AlignCentre(column, width) + "|";
            }
        }

        Console.WriteLine(row);
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
        Console.Write(" om de rij te bewerken.");
        Console.Write("\nOm een stap terug te gaan druk op");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(" Backspace");
        Console.ResetColor();
        Console.WriteLine();


    }
}