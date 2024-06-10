public static class UserSignUp
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void SignUp(bool IsAdminSign)
    {
        string? password;
        string? email;
        Console.WriteLine("Welkom op de registreer pagina.");
        Console.WriteLine("Om je account aan te maken vul de volgende vakken in.");

        Console.Write("Om te bevestigen druk enter in "); 
        ColorPrint.PrintGreen("Enter");
        Console.Write("Om terug te gaan druk ");
        ColorPrint.PrintRed("Escape");
        Console.WriteLine("Uw voornaam: ");
        string? firstName = Helper.StringHelper();
        if (firstName == "Escape/GoBack.") Menu.Start();
        while (!Helper.IsOnlyLetterSpaceDash(firstName))
        {
            ColorPrint.PrintRed($"'{firstName}' is geen geldige optie.");
            Console.WriteLine("Uw voornaam kan alleen bestaan uit letters.");
            Console.WriteLine("Uw voornaam: ");
            firstName = Helper.StringHelper();
        }

        Console.Write("\nOm te bevestigen druk enter in "); 
        ColorPrint.PrintGreen("Enter");
        Console.Write("Om terug te gaan druk ");
        ColorPrint.PrintRed("Escape");
        Console.WriteLine("Uw achternaam: ");
        string? lastName = Helper.StringHelper();
        if (lastName == "Escape/GoBack.") Menu.Start();
        while (!Helper.IsOnlyLetterSpaceDash(lastName))
        {
            ColorPrint.PrintRed($"'{lastName}' is geen geldige optie.");
            Console.WriteLine("Uw achternaam kan alleen bestaan uit letters.");
            Console.WriteLine("Uw achternaam: ");
            lastName = Helper.StringHelper();
        }

        while (true)
        {
            Console.Write("\nOm te bevestigen druk enter in "); 
            ColorPrint.PrintGreen("Enter");
            Console.Write("Om terug te gaan druk ");
            ColorPrint.PrintRed("Escape");
            Console.WriteLine("Uw e-mail: ");
            email = Helper.StringHelper();
            if (email == "Escape/GoBack.") Menu.Start();

            if (!accountsLogic.IsValidEmail(email))
            {
                ColorPrint.PrintRed("Ongeldig e-mailadres. Probeer opnieuw.");
                continue;
            }
            if (!accountsLogic.EmailExists(email))
            {
                break;
            }
            else
            {
                ColorPrint.PrintRed("Er is al een account met dit e-mail. \nProbeer een ander.");
            }
        }

        while (true)
        {
            Console.Write("\nOm te bevestigen druk enter in "); 
            ColorPrint.PrintGreen("Enter");
            Console.Write("Om terug te gaan druk ");
            ColorPrint.PrintRed("Escape");
            Console.WriteLine("Uw wachtwoord:");
            password = Helper.StringHelper();
            if (password == "Escape/GoBack.") Menu.Start();
            while (!Helper.IsValidString(password))
            {
                ColorPrint.PrintRed($"'{password}' is geen geldige optie.");
                Console.WriteLine("Uw wachtwoord kan niet leeg zijn.");
                Console.WriteLine("Uw wachtwoord: ");
                password = Helper.StringHelper();
            }

            Console.Write("\nOm te bevestigen druk enter in "); 
            ColorPrint.PrintGreen("Enter");
            Console.Write("Om terug te gaan druk ");
            ColorPrint.PrintRed("Escape");
            Console.WriteLine("Controleer uw wachtwoord:");
            string? password2 = Helper.StringHelper();
            if (password2 == "Escape/GoBack.") Menu.Start();
            while (!Helper.IsValidString(password2))
            {
                ColorPrint.PrintRed($"'{password2}' is geen geldige optie.");
                Console.WriteLine("Uw wachtwoord kan niet leeg zijn.");
                Console.WriteLine("Controleer uw wachtwoord:");
                password2 = Helper.StringHelper();
            }

            if (password == password2)
            {
                break;
            }
            else
            {
                ColorPrint.PrintRed("Uw wachtwoord komt niet overeen.\nProbeer het opnieuw.");
            }
        }

        string fullName = $"{firstName} {lastName}";
        if (IsAdminSign)
        {
            AdminSignUp(email, password, fullName);
        }
        else 
        {
            CostumerSignUp(email, password, fullName);
        }
        
    }

    public static void CostumerSignUp(string email, string password, string fullName)
    {
        AccountModel newAcc = new AccountModel(accountsLogic.GenerateNewId(), email, password, fullName, false);
        accountsLogic.UpdateList(newAcc);
        AccountModel acc = accountsLogic.CheckLogin(email, password);
        UserLogin.loggedInAccount = acc;
        
        Console.WriteLine($"Welkom {acc.FullName}.");
        Thread.Sleep(3000);
        ColorPrint.PrintGreen("Uw account is aangemaakt.");

        Thread.Sleep(3000);
        ColorPrint.PrintMagenta("U bent ingelogd op uw account en gaat naar het startmenu.\n");
        Thread.Sleep(4000);
        Console.Clear();
        CustomerStartMenu.Start();
    }

    public static void AdminSignUp(string email, string password, string fullName)
    {
        AccountModel newAcc = new AccountModel(accountsLogic.GenerateNewId(), email, password, fullName, true);
        accountsLogic.UpdateList(newAcc);
        AccountModel acc = accountsLogic.CheckLogin(email, password);

        Console.WriteLine($"Welkom {acc.FullName}.");
        Thread.Sleep(3000);
        ColorPrint.PrintGreen("Uw Admin account is aangemaakt.");

        Thread.Sleep(3000);
        Console.WriteLine("U keert terug naar het Startmenu.\n");
        Thread.Sleep(3000);
        AdminStartMenu.Start();
    }
}