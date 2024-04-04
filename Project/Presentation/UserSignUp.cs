static class UserSignUp
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void Start()
    {
        string password;
        string email;
        Console.WriteLine("Welkom op de registreer pagina.");
        Console.WriteLine("Om je account aan te maken vul de volgende vakken in.");

        Console.WriteLine("Uw voornaam:");
        string firstName = Console.ReadLine();
        Console.WriteLine("Uw achternaam:");
        string lastName = Console.ReadLine();
        while (true){
            Console.WriteLine("Uw email address:");
            email = Console.ReadLine();

            if (!accountsLogic.IsValidEmail(email))
            {
                Console.WriteLine("Ongeldig emailadres. Probeer opnieuw.");
                continue;
            }
            if (!accountsLogic.EmailExists(email))
            {
                break;
            }
            else{
                Console.WriteLine("Er is al een account met dit email address. \nProbeer een ander.");
            }
        }
        while (true)
        {
            Console.WriteLine("Uw wachtwoord:");
            password = Console.ReadLine();
            Console.WriteLine("Controleer uw wachtwoord:");
            string password2 = Console.ReadLine();

            if (password == password2)
            {
                break;
            }
            else
            {
                Console.WriteLine("Uw wachtwoord komt niet overeen.\nProbeer het opnieuw.");
            }
        }
        string fullName = $"{firstName} {lastName}";
        AccountModel newAcc = new AccountModel(accountsLogic.GenerateNewId(), email, password, fullName);
        accountsLogic.UpdateList(newAcc);
        AccountModel acc = accountsLogic.CheckLogin(email, password);
        Console.WriteLine($"Welcome {acc.FullName}.");
        Thread.Sleep(3000);
        Console.WriteLine("Uw account is aangemaakt.");

        Thread.Sleep(3000);
        Console.WriteLine("U keert terug naar het Startmenu.\n");
        Thread.Sleep(3000);
        Menu.Start();
    }
}