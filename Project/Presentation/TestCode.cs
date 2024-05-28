using System.ComponentModel.Design;
using System.Text.Json.Serialization;

public class TestCode
{
    public void Test()
    {
        SeatModelEpic seatModel = new(1);
        SeatModelEpic[,] seatModels = new SeatModelEpic[6, 10];
        CreateSeats(seatModels);
        Print(seatModels);
        Start(seatModels);
        Console.WriteLine(seatModels[0,0]);
    }

    void CreateSeats(SeatModelEpic[,] seatModels)
    {
        for (int row = 0; row < seatModels.GetLength(0); row++)
        {
            for (int col = 0; col < seatModels.GetLength(1); col++)
            {
                if (seatModels[row, col] == null)
                {
                    seatModels[row, col] = new SeatModelEpic(1);
                }
            }
        }
    }

void Print(SeatModelEpic[,] seatModels)
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



static void Start(SeatModelEpic[,] seatModels)
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

            case ConsoleKey.Enter:
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
        }

        // Clear console and display options
        Console.Clear();
        DisplayOptions(selectedOption, seatModels);
    }
}

static void DisplayOptions((int Row, int Col) selectedOption, SeatModelEpic[,] seatModels)
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
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write(" Enter ");
    Console.ResetColor();
    Console.WriteLine("om een optie te selecteren");
}
}
public class SeatModelEpic : IActivatable
    {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("isOccupied")]
    public bool IsOccupied { get; set; }

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    public SeatModelEpic(int id, bool isOccupied = false, bool isActive = true)
    {
        this.Id = id;
        this.IsOccupied = isOccupied;
        this.IsActive = isActive;
    }
}


