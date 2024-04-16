public class StopMenu
{
    public static StopModel MakeStop()
    {
        bool checkStopName = true;
        while (checkStopName)
        {
            Console.WriteLine("Wat is de naam van de halte?");
            string? newName = Console.ReadLine();
            if (newName != null && newName.All(char.IsLetter))
            {
                StopModel newStop = new StopModel(Convert.ToString(newName));
                return newStop;
            }
            Console.WriteLine("De naam van een halte kan alleen letters zijn.");
            Console.WriteLine("Probeer het nog een keer.");
        }
        return null;
    }
}