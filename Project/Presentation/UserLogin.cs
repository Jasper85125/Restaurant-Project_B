using System.Formats.Asn1;

public static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();
    public static AccountModel loggedInAccount {get; set;}


    public static void Start()
    {
        Console.WriteLine("Welkom op de inlogpagina");
        Console.WriteLine("Vul uw email in: ");
        string? email = Console.ReadLine();
        Console.WriteLine("Vul uw wachtwoord in: ");
        string? password = Console.ReadLine();
        AccountModel acc = accountsLogic.CheckLogin(email, password);
        loggedInAccount = acc;

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
        else
        {
            ColorPrint.PrintRed("Geen account gevonden met die email en wachtwoord combinatie.");
            Thread.Sleep(3000);
        }
    }
}