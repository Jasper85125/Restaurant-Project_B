public class SeatLogic //: AbstractLogic<SeatModel>
{
    // private List<SeatModel> _seats;

    // public static SeatModel? CurrentSeat{ get; private set; }


    // public SeatLogic()
    // {
    //    _seats = DataAccess<SeatModel>.LoadAll();
    // }

    // public override void UpdateList(SeatModel seat)
    // {
    //     //Find if there is already an model with the same id
    //     int index = _seats.FindIndex(p => p.Id == seat.Id);

    //     if (index != -1)
    //     {
    //         //update existing model
    //         _seats[index] = seat;
    //     }
    //     else
    //     {
    //         //add new model
    //         _seats.Add(seat);
    //     }
    //     DataAccess<SeatModel>.WriteAll(_seats);
    // }


    // public override SeatModel GetById(int id)
    // {
    //     return _seats.Find(p => p.Id == id);
    // }

    // public override int GenerateNewId() 
    // {
    //     if (_seats == null || _seats.Count == 0)
    //     {
    //         return 1;
    //     }
    //    return _seats.Max(seat => seat.Id) + 1;
    // } 
    // public override List<SeatModel> GetAll() => _seats;

    public SeatModel[,] ConvertTo2DArr(Dictionary<(int Row, int Col), SeatModel> seatingMap)
    {
        // Find the maximum rows and columns to determine the size of the 2D array
        int maxRow = 0;
        int maxCol = 0;
        
        foreach (var kvp in seatingMap)
        {
            if (kvp.Key.Row > maxRow) maxRow = kvp.Key.Row;
            if (kvp.Key.Col > maxCol) maxCol = kvp.Key.Col;
        }

        SeatModel[,] array = new SeatModel[maxRow + 1, maxCol + 1];

        foreach (var kvp in seatingMap)
        {
            array[kvp.Key.Row, kvp.Key.Col] = kvp.Value;
        }

        return array;
    }

    public Dictionary<(int Row, int Col), SeatModel> ConvertToDict(SeatModel[,] seatingMap)
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

    public void CreateSeats(SeatModel[,] seatModels)
    {
        for (int row = 0; row < seatModels.GetLength(0); row++)
        {
            for (int col = 0; col < seatModels.GetLength(1); col++)
            {
                if (seatModels[row, col] == null && row != seatModels.GetLength(0)/2 /* row != 3 */)
                {
                    seatModels[row, col] = new SeatModel();
                }
            }
        }
    }

    public void CreateBusinessSeats(SeatModel[,] seatModels)
    {
        for (int row = 0; row < seatModels.GetLength(0); row++)
        {
            for (int col = 0; col < seatModels.GetLength(1); col++)
            {
                if (seatModels[row, col] == null && row != seatModels.GetLength(0)/2 /* row != 3 */)
                {
                    if (row == 2 && (col == 2 || col == 3 || col == 4) || row == 4 && (col == 2 || col == 3 || col == 4)
                    || row == 0 && (col > 6) || row == 1 && (col > 6) || row == 5 && (col > 6) || row == 6 && (col > 6))
                    {
                       seatModels[row, col] = null;
                    }
                    else
                    {
                        seatModels[row, col] = new SeatModel();
                    }
            
                }
            }
        }
    }

    public void Print(SeatModel[,] seatModels)
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


    public void PrintArr(SeatModel[,] seatModels)
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


   public  void PrintDict(Dictionary<(int Row, int Col), SeatModel> seatingMap)
    {
        foreach (var kvp in seatingMap)
        {
            Console.WriteLine(kvp.Key + " " + kvp.Value.IsOccupied);
        }
    }
}
