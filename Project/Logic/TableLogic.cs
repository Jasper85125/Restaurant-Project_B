using System.Runtime.CompilerServices;

public class TableLogic<T>
{
    public static int selectedOption = 0;

    public static int tableWidth = 145;
     public static int TableCustomeWidth;
    public (List<string>, int)? PrintTable(List<string> Header, List<T> Data, Func<T, List<string>> GenerateRow, string Title,  Action<T> Listupdater, string Kind)
    {
        ConsoleKeyInfo keyInfo;
        List<string> geselecteerdeRow = new List<string>();
        List<string> NewRow = new() {$"nieuwe {Kind}"};
        for (int i = 0; i < Header.Count() - 1; i++)
        {
            NewRow.Add("+");
        }
        TableCustomeWidth = PrintRow(Header, false, true).Length;
        do
        {
            Console.Clear();
            Console.WriteLine($"{Title}\n");
            PrintRow(Header, false, true);

            for (int rowNumber = 1; rowNumber <= Data.Count() + 1; rowNumber++)
            {
                tableWidth = 145;
                if (rowNumber <= Data.Count())
                {
                    if (rowNumber == selectedOption)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        geselecteerdeRow = GenerateRow(Data[rowNumber - 1]);
                        PrintRow(geselecteerdeRow, true, false);
                        Console.ResetColor();
                    }
                    else
                    {
                        PrintRow(GenerateRow(Data[rowNumber - 1]), false, false);
                    }
                }
                else
                {
                    if (rowNumber == selectedOption)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        geselecteerdeRow = NewRow;
                        PrintRow(geselecteerdeRow, true, true);
                        Console.ResetColor();
                    }
                    else
                    {
                        PrintRow(NewRow, false, true);
                    }
                }
            }
            SelectionExplanation(true);

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
                case ConsoleKey.Delete:
                    if (selectedOption > 0 && selectedOption <= Data.Count())
                    {
                        dynamic item = Data[selectedOption - 1];
                        if (item.IsActive)
                        {
                            item.IsActive = false;
                        }
                        else{
                        item.IsActive = true;
                        }
                        Listupdater(item);
                        
                    }
                    break;
                case ConsoleKey.Escape:
                    return null;
            }
        } while (keyInfo.Key != ConsoleKey.Escape);
        
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
            PrintRow(header, false, true);
            PrintRowForSelected(selectedRow, selectedIndex); // Pass selectedIndex to highlight the selected item
            SelectionExplanation(false);
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
                    return (selectedRow[selectedIndex],selectedIndex);
                case ConsoleKey.Escape:
                    return null;
            }

        } while (key.Key != ConsoleKey.Escape);

        return null;
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
            Console.WriteLine(new string('-', TableCustomeWidth));
        }
        Console.WriteLine(row);
        if(PrintLineBool){
            tableWidth = row.Length;
            Console.WriteLine(new string('-', TableCustomeWidth));
        }
        return row;
    }

    private static void PrintRowForSelected(List<string> columns, int selectedIndex)
    {
        int width = (tableWidth - (columns.Count() + 1)) / columns.Count();
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
                Console.Write($"{AlignCentre(column, width)}|");
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

    private static void SelectionExplanation(bool UsedInMainTable){
        Console.WriteLine("Selecteer een rij doormiddel van de pijltjes.");
        Console.Write("Als je de juiste rij hebt geselecteerd klik op ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Enter");
        Console.ResetColor();
        Console.Write(" om de rij te bewerken.");
        if (UsedInMainTable){
            Console.Write("\nOm de rij actief/non-actief te maken klik op ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Delete");
            Console.ResetColor();
            Console.Write(".");
        }
        Console.Write("\nOm een stap terug te gaan druk op");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(" Escape");
        Console.ResetColor();
        Console.Write(".\n");


    }
}