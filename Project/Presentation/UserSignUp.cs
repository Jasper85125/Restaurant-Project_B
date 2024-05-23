public static class UserSignUp
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void SignUp(bool IsAdminSign)
    {
        string? password;
        string? email;
        Console.WriteLine("Welkom op de registreer pagina.");
        Console.WriteLine("Om je account aan te maken vul de volgende vakken in.");

        Console.WriteLine("Uw voornaam: ");
        string? firstName = Console.ReadLine();
        while (!Helper.IsOnlyLetterSpaceDash(firstName))
        {
            ColorPrint.PrintRed($"'{firstName}' is geen geldige optie.");
            Console.WriteLine("Uw voornaam kan alleen bestaan uit letters.");
            Console.WriteLine("Uw voornaam: ");
            firstName = Console.ReadLine();
        }
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
            Console.WriteLine("Uw email: ");
            email = Console.ReadLine();

            if (!accountsLogic.IsValidEmail(email))
            {
                ColorPrint.PrintRed("Ongeldig emailadres. Probeer opnieuw.");
                continue;
            }
            if (!accountsLogic.EmailExists(email))
            {
                break;
            }
            else
            {
                ColorPrint.PrintRed("Er is al een account met dit email. \nProbeer een ander.");
            }
        }
        while (true)
        {
            Console.WriteLine("Uw wachtwoord:");
            password = Console.ReadLine();
            while (!Helper.IsValidString(password))
            {
                ColorPrint.PrintRed($"'{password}' is geen geldige optie.");
                Console.WriteLine("Uw wachtwoord kan niet leeg zijn.");
                Console.WriteLine("Uw wachtwoord: ");
                password = Console.ReadLine();
            }

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
        
        Console.WriteLine($"Welkom {acc.FullName}.");
        Thread.Sleep(3000);
        ColorPrint.PrintGreen("Uw account is aangemaakt.");

        Thread.Sleep(3000);
        Console.WriteLine("U keert terug naar het Startmenu.\n");
        Thread.Sleep(3000);
        Menu.Start();
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
        Menu.Start();
    }
}