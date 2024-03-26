static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        Console.WriteLine("[1] Inloggen op uw account");
        Console.WriteLine("[2] informatie over de app.");
        Console.WriteLine("[3] Menu voor routes toevoegen");

        string? input = Console.ReadLine();
        if (input == "1")
        {
            UserLogin.Start();
        }
        else if (input == "2")
        {
            Console.WriteLine("Ons pannenkoeken restaurant is een bus waar je op");
            Console.WriteLine("meerderen plekken kan instappen zodat je kan eten terwijl de bus een rondje rijdt."); 
            Console.WriteLine("En op deze app/site kan je reserveren waar en wanneer je met ons mee wil rijden.");
        }
        else if (input == "3")
        {
            RouteMenu.Welcome();
        }
        else
        {
            Console.WriteLine("Invalid input");
            Menu.Start();
        }

    }
}