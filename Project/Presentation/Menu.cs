static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        Console.WriteLine("[1] Inloggen op uw account.");
        Console.WriteLine("[2] Account aanmaken.");
        Console.WriteLine("[3] informatie over de app.");
        Console.WriteLine("[4] Menu voor routes toevoegen");
        Console.WriteLine("[5] Menu voor prijscategorieën");

        string? input = Console.ReadLine();
        switch (input)
        {
            case "1":
                UserLogin.Start();
                break;
            case "2":
                UserSignUp.Start();
                break;
            case "3":
                Console.WriteLine("Ons pannenkoeken restaurant is een bus waar je op");
                Console.WriteLine("meerderen plekken kan instappen zodat je kan eten terwijl de bus een rondje rijdt.");
                Console.WriteLine("En op deze app/site kan je reserveren waar en wanneer je met ons mee wil rijden.");
                break;
            case "4":
                RouteMenu.Welcome();
                break;
            case "5":
                PriceMenu.Start();
                break;
            default:
                Console.WriteLine("Invalid input");
                Menu.Start();
                break;
        }
    }
}