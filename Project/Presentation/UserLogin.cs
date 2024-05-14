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
            Console.WriteLine("Welkom " + acc.FullName);

            //Write some code to go back to the menu
            Thread.Sleep(3000);
            Console.WriteLine("U gaat nu naar de startpagina voor klanten.\n");
            Thread.Sleep(3000);
            CustomerStartMenu.Start();
        }
        else if (acc != null && acc.IsAdmin == true)
        {
            Console.WriteLine("Welkom " + acc.FullName);

            //Write some code to go back to the menu
            Thread.Sleep(3000);
            Console.WriteLine("U gaat nu naar de startpagina voor admins.\n");
            Thread.Sleep(3000);
            AdminStartMenu.Start();
        }
        else
        {
            Console.WriteLine("Geen account gevonden met die email en wachtwoord combinatie.");
            Thread.Sleep(3000);
        }
    }
}