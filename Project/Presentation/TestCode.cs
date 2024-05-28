using System.ComponentModel.Design;
using System.Text.Json.Serialization;



Dictionary<(int Row, int Col), SeatModel> seatingMap = new ()
{
    {(0,0), new SeatModel(1)},
    {(0,1), new SeatModel(2)},
    {(0,2), new SeatModel(3)}
};


PrintArr(ConvertTo2DArr(seatingMap));
SeatModel[,] seatModels = new SeatModel[6, 10];
CreateSeats();
Start(seatModels);


Dictionary<(int Row, int Col), SeatModel> ConvertToDict(SeatModel[,] seatingMap)
{
    Dictionary<(int Row, int Col), SeatModel> dict = new();

    for (int row = 0; row < seatingMap.GetLength(0); row++)
    {
        for (int col = 0; col < seatingMap.GetLength(1); col++)
        {
            dict[(row, col)] = seatingMap[row, col];
        }
    }

    return dict;
}


SeatModel[,] ConvertTo2DArr(Dictionary<(int Row, int Col), SeatModel> seatingMap)
{
    int maxRow = 0;
    int maxCol = 0;
    
    foreach (KeyValuePair<(int Row, int Col), SeatModel> kvp in seatingMap)
    {
        if (kvp.Key.Row > maxRow) maxRow = kvp.Key.Row;
        if (kvp.Key.Col > maxCol) maxCol = kvp.Key.Col;
    }

    SeatModel[,] array = new SeatModel[maxRow + 1, maxCol + 1];

    foreach (KeyValuePair<(int Row, int Col), SeatModel> kvp in seatingMap)
    {
        array[kvp.Key.Row, kvp.Key.Col] = kvp.Value;
    }

    return array;
}


void CreateSeats()
{
    for (int row = 0; row < seatModels.GetLength(0); row++)
    {
        for (int col = 0; col < seatModels.GetLength(1); col++)
        {
            if (seatModels[row, col] == null)
            {
                seatModels[row, col] = new SeatModel(1); // !!!!!!!!!!!!!!
            }
        }
    }
}

void Print(SeatModel[,] seatModels)
{
    for (int row = 0; row < seatModels.GetLength(0); row++)
    {
        for (int col = 0; col < seatModels.GetLength(1); col++)
        {
            if (seatModels[row, col] != null)
            {
                Console.Write("-    ");
            }
        }
        Console.WriteLine();
    }
}


void PrintArr(SeatModel[,] seatModels)
{
    for (int row = 0; row < seatModels.GetLength(0); row++)
    {
        for (int col = 0; col < seatModels.GetLength(1); col++)
        {
            if (seatModels[row, col] != null)
            {
                Console.Write($"({row},{col}) {seatModels[row, col].IsOccupied}");
            }
        }
        Console.WriteLine();
    }
}


void PrintDict(Dictionary<(int Row, int Col), SeatModel> seatingMap)
{
    foreach (var kvp in seatingMap)
    {
        Console.WriteLine(kvp.Key + " " + kvp.Value.IsOccupied);
    }
}

static void Start(SeatModel[,] seatModels)
{
    
    int rowLength = seatModels.GetLength(0) - 1;
    int colLength = seatModels.GetLength(1) - 1;

    Console.Clear();
    (int Row, int Col) selectedOption = new (0, 0); // Default selected option

    // Display options
    DisplayOptions(selectedOption, seatModels);

    while (true)
    {
        // Wait for key press
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

        // Check arrow key presses
        switch (keyInfo.Key)
        {
            case ConsoleKey.UpArrow:
                // Move up
                selectedOption = (Math.Max(0, selectedOption.Row - 1), selectedOption.Col);
                break;
            case ConsoleKey.DownArrow:
                // Move down
                selectedOption = (Math.Min(rowLength, selectedOption.Row + 1), selectedOption.Col);;
                break;
            case ConsoleKey.RightArrow:
                // Move right
                selectedOption = (selectedOption.Row, Math.Min(colLength, selectedOption.Col + 1));
                break;

            case ConsoleKey.LeftArrow:
                // Move left
                selectedOption = (selectedOption.Row, Math.Max(0, selectedOption.Col - 1));
                break;

            case ConsoleKey.Spacebar:
                Console.Clear();
                if (seatModels[selectedOption.Row,selectedOption.Col].IsOccupied)
                {
                    Console.WriteLine("Is al bezet!");
                    Console.ReadLine();
                }
                else
                {
                    seatModels[selectedOption.Row,selectedOption.Col].IsOccupied = true;
                }
                break;

            case ConsoleKey.Enter:
                break;

        }
        break;
        // Clear console and display options
        Console.Clear();
        DisplayOptions(selectedOption, seatModels);
    }
}

static void DisplayOptions((int Row, int Col) selectedOption, SeatModel[,] seatModels)
{
    Console.WriteLine("Selecteer een optie:\n");

    for (int row = 0; row < seatModels.GetLength(0); row++)
    {
        for (int col = 0; col < seatModels.GetLength(1); col++)
        {
            if (seatModels[row, col].IsOccupied)
            {
                Console.ForegroundColor = selectedOption.Row == row && selectedOption.Col == col ? ConsoleColor.Green: ConsoleColor.Green;
                Console.Write(selectedOption.Row == row && selectedOption.Col == col ? " * " : " * "); 
            }
            else
            {
                Console.ForegroundColor = selectedOption.Row == row && selectedOption.Col == col ? ConsoleColor.Green: ConsoleColor.White;
                Console.Write(selectedOption.Row == row && selectedOption.Col == col ? " O " : " - ");
            }  
        }
        Console.WriteLine();
    }
    Console.ResetColor();

    Console.WriteLine("\n* bezet");
    Console.WriteLine("- beschikbaar");

    // Reset text color

    Console.Write("\nKlik");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write(" Spatie ");
    Console.ResetColor();
    Console.WriteLine("om een optie te selecteren");

    Console.Write("Klik");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write(" Enter ");
    Console.ResetColor();
    Console.WriteLine("om uw keuze te bevestigen.");
}

// public class SeatModel : IActivatable
// {
//     [JsonPropertyName("id")]
//     public int Id { get; set; }

//     [JsonPropertyName("isOccupied")]
//     public bool IsOccupied { get; set; }

//     [JsonPropertyName("isActive")]
//     public bool IsActive { get; set; }

//     public SeatModel(int id, bool isOccupied = false, bool isActive = true)
//     {
//         this.Id = id;
//         this.IsOccupied = isOccupied;
//         this.IsActive = isActive;
//     }
// }


// public interface IActivatable
// {
//     bool IsActive { get; set; }
// }