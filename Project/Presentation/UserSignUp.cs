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
        Console.WriteLine("Uw voornaam: ");
        string? firstName = Console.ReadLine();
        while (!Helper.IsOnlyLetterSpaceDash(firstName))
        {
            ColorPrint.PrintRed($"'{firstName}' is geen geldige optie.");
            Console.WriteLine("Uw voornaam kan alleen bestaan uit letters.");
            Console.WriteLine("Uw voornaam: ");
            firstName = Console.ReadLine();
        }

        Console.Write("Om te bevestigen druk enter in "); 
        ColorPrint.PrintGreen("Enter");
        Console.WriteLine("Uw achternaam: ");
        string? lastName = Console.ReadLine();
        while (!Helper.IsOnlyLetterSpaceDash(lastName))
        {
            ColorPrint.PrintRed($"'{lastName}' is geen geldige optie.");
            Console.WriteLine("Uw achternaam kan alleen bestaan uit letters.");
            Console.WriteLine("Uw achternaam: ");
            lastName = Console.ReadLine();
        }

        while (true)
        {
            Console.Write("Om te bevestigen druk enter in "); 
            ColorPrint.PrintGreen("Enter");
            Console.WriteLine("Uw e-mail: ");
            email = Console.ReadLine();

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
            Console.Write("Om te bevestigen druk enter in "); 
            ColorPrint.PrintGreen("Enter");
            Console.WriteLine("Uw wachtwoord:");
            password = Console.ReadLine();
            while (!Helper.IsValidString(password))
            {
                ColorPrint.PrintRed($"'{password}' is geen geldige optie.");
                Console.WriteLine("Uw wachtwoord kan niet leeg zijn.");
                Console.WriteLine("Uw wachtwoord: ");
                password = Console.ReadLine();
            }

            Console.Write("Om te bevestigen druk enter in "); 
            ColorPrint.PrintGreen("Enter");
            Console.WriteLine("Controleer uw wachtwoord:");
            string? password2 = Console.ReadLine();
            while (!Helper.IsValidString(password2))
            {
                ColorPrint.PrintRed($"'{password2}' is geen geldige optie.");
                Console.WriteLine("Uw wachtwoord kan niet leeg zijn.");
                Console.WriteLine("Controleer uw wachtwoord:");
                password2 = Console.ReadLine();
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