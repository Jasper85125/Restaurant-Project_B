public static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("[1] Inloggen op uw account.");
        Console.WriteLine("[2] Account aanmaken.");
        Console.WriteLine("[3] Informatie over de app.");
        Console.WriteLine("[4] Menu voor routes toevoegen");
        Console.WriteLine("[5] Menu voor prijscategorieën");
        Console.WriteLine("[6] Menu voor bussen");

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
                Information.Info();
                break;
            case "4":
                RouteMenu.Welcome();
                break;
            case "5":
                PriceMenu.Start();
                break;
            case "6":
                BusMenu.Start();
                break;
            default:
                Console.WriteLine("Verkeerde input");
                Menu.Start();
                break;
        }
    }
}