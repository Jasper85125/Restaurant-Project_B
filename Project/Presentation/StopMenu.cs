public class StopMenu
{
    public static StopModel MakeStop()
    {
        bool checkStopName = true;
        while (checkStopName)
        {
            Console.WriteLine("Wat is de naam van de tussenstop?");
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

    public static RouteModel AddToRoute(StopModel stop, RouteModel inputRoute)
    {
        inputRoute.Stops.Add(stop);
        return inputRoute;
    }
}