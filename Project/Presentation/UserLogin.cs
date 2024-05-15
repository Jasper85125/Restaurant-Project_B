using System.Formats.Asn1;

public static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void Start()
    {
        Console.WriteLine("Welkom op de inlogpagina");
        Console.WriteLine("Vul uw email in: ");
        string email = Console.ReadLine();
        Console.WriteLine("Vul uw wachtwoord in: ");
        string password = Console.ReadLine();
        AccountModel acc = accountsLogic.CheckLogin(email, password);

        if (acc != null && acc.IsAdmin == false)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Welkom " + acc.FullName);

            //Write some code to go back to the menu
            Thread.Sleep(3000);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("U gaat nu naar de startpagina voor klanten.\n");
            Console.ResetColor();
            Thread.Sleep(3000);
            CustomerStartMenu.Start();
        }
        else if (acc != null && acc.IsAdmin == true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Welkom " + acc.FullName);
            Console.ResetColor();

            //Write some code to go back to the menu
            Thread.Sleep(3000);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("U gaat nu naar de startpagina voor admins.\n");
            Console.ResetColor();
            Thread.Sleep(3000);
            AdminStartMenu.Start();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Geen account gevonden met die email en wachtwoord combinatie.");
            Console.ResetColor();
            Thread.Sleep(3000);
        }
    }
}