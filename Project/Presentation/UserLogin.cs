using System.Formats.Asn1;

public static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();
    public static AccountModel loggedInAccount {get; set;}

    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("Welkom op de inlogpagina.");
        Console.Write("Om te bevestigen druk "); 
        ColorPrint.PrintGreen("Enter.");
        Console.Write("Om terug te gaan druk ");
        ColorPrint.PrintRed("Escape.");
        Console.WriteLine("Vul uw e-mail in: ");
        string? email = Helper.StringHelper();
        if (email == "Escape/GoBack.") Menu.Start();
        Console.WriteLine();
        while (true)
        {
            if (!accountsLogic.IsValidEmail(email) && email != "Admin")
            {
                ColorPrint.PrintRed("Ongeldig e-mailadres. Probeer opnieuw.");
            }
            else
            {
                if (!accountsLogic.EmailExists(email))
                {
                    ColorPrint.PrintRed("Uw e-mail is niet geregistreerd in ons systeem.");
                    EnterOrEscape();

                }
                else
                {
                    break;
                }
            }
            Console.WriteLine("Vul uw e-mail in: ");
            email = Helper.StringHelper();
            if (email == "Escape/GoBack.") Menu.Start();
            Console.WriteLine();
        }

        Console.WriteLine("Vul uw wachtwoord in: ");
        string? password = Helper.StringHelper();
        if (password == "Escape/GoBack.") Menu.Start();
        Console.WriteLine();
        while (!Helper.IsValidString(password))
        {
            ColorPrint.PrintRed("Uw wachtwoord kan niet leeg zijn.");
            Console.WriteLine("Vul uw wachtwoord in: ");
            password = Helper.StringHelper();
            if (password == "Escape/GoBack.") Menu.Start();
            Console.WriteLine();
        }
        AccountModel acc = accountsLogic.CheckLogin(email, password);
        loggedInAccount = acc;

        if (acc == null)
        {
            ColorPrint.PrintRed("Geen account gevonden met de opgegeven e-mail en wachtwoord combinatie.");
            EnterOrEscape();
        }

        if (acc != null && acc.IsAdmin == false)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Welkom " + acc.FullName);

            //Write some code to go back to the menu
            Thread.Sleep(3000);
            ColorPrint.PrintGreen("U gaat nu naar de startpagina voor klanten.\n");
            CustomerStartMenu.Start();
        }
        else if (acc != null && acc.IsAdmin == true)
        {
            ColorPrint.PrintGreen("Welkom " + acc.FullName);

            //Write some code to go back to the menu
            Thread.Sleep(3000);
            ColorPrint.PrintGreen("U gaat nu naar de startpagina voor admins.\n");
            Thread.Sleep(3000);
            AdminStartMenu.Start();
        }
    }

    public static void EnterOrEscape()
    {
        Console.Write("Om naar het startmenu te gaan, druk op");
        ColorPrint.PrintWriteRed(" Escape.\n");
        Console.Write("Om nog een poging te doen tot inloggen, druk op");
        ColorPrint.PrintWriteGreen(" Enter.\n");
        while (true)
        {
            // Wait for key press
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            // Check arrow key presses
            switch (keyInfo.Key)
            {
                case ConsoleKey.Escape:
                    // Move to the previous option
                    BackToStartMenu();
                    break;
                case ConsoleKey.Enter:
                    Start();
                    break;
            }
        }
    }

    public static void BackToStartMenu()
    {
        Console.Clear();
        ColorPrint.PrintYellow("U keert terug naar het Startmenu.");
        Thread.Sleep(3000);
        Menu.Start();
    }
}