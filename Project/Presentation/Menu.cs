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

        string? input = Console.ReadLine();
        if (input == "1")
        {
            UserLogin.Start();
        }
        else if (input == "2")
        {
            UserSignUp.Start();
        }
        else if (input == "3")
        {
            Information.Info();
        }
        else if (input == "4")
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