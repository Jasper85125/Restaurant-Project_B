static public class Information
{
    static public void Info()
    {
        Console.WriteLine("Ons pannenkoeken restaurant is een bus waar je op");
        Console.WriteLine("meerderen plekken kan instappen zodat je kan eten terwijl de bus een rondje rijdt."); 
        Console.WriteLine("En op deze app/site kan je reserveren waar en wanneer je met ons mee wil rijden.\n");
        AfterShowingInformation();
    }


    public static void AfterShowingInformation()
    {
        string answer = "";
        while (answer.ToLower() !="y")
        {       
            Console.WriteLine("Om terug te gaan naar het Startmenu voer y in.");
            answer = Console.ReadLine();
        }
        BackToStartMenu();
    }

        public static void BackToStartMenu()
    {
        Console.WriteLine("U keert terug naar het Startmenu.\n");
        ;System.Threading.Thread.Sleep(3000);
        Menu.Start();
    }

}
