using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;


//This class is not static so later on we can use inheritance and interfaces
public class AccountsLogic : AbstractLogic<AccountModel>
{
    private List<AccountModel> _accounts;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself
    public static AccountModel? CurrentAccount { get; private set; }

    public AccountsLogic()
    {
        _accounts = DataAccess<AccountModel>.LoadAll();
        try 
        {
            bool foundAdmin = false;
            foreach (AccountModel account in _accounts)
            {
                if (account.IsAdmin == true)
                {
                    foundAdmin = true;
                    break;
                }
            }
            if (foundAdmin == false)
            {
                AccountModel newAcc = new AccountModel(GenerateNewId(), "Admin", "Admin", "Admin", true);
                UpdateList(newAcc);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }
    public bool EmailExists(string email)
    {
        foreach (var account in _accounts)
        {
            if (account.EmailAddress == email)
            {
                return true;
            }
        }
        return false;
    }
    public override int GenerateNewId() 
    {
        if (_accounts == null || _accounts.Count == 0)
        {
            return 1;
        }
       return _accounts.Max(account => account.Id) + 1;
    } 

    public override void UpdateList(AccountModel acc)
    {
        //Find if there is already an model with the same id
        int index = _accounts.FindIndex(s => s.Id == acc.Id);

        if (index != -1)
        {
            //update existing model
            _accounts[index] = acc;
        }
        else
        {
            //add new model
            _accounts.Add(acc);
        }
        DataAccess<AccountModel>.WriteAll(_accounts);

    }

    public override AccountModel GetById(int id)
    {
        return _accounts.Find(i => i.Id == id);
    }

    public AccountModel CheckLogin(string email, string password)
    {
        List<AccountModel> accounts = DataAccess<AccountModel>.LoadAll();
        if (email == null || password == null)
        {
            return null;
        }
        CurrentAccount = accounts.Find(i => i.EmailAddress == email && i.Password == password);
        return CurrentAccount;
    }

    public override List<AccountModel> GetAll() => _accounts;

    public void MakeReservation()
    {
        
    }

    public AccountModel AddReservations(AccountModel inputAccount, ReservationModel reservation)
    {
        inputAccount.Reservations.Add(reservation);
        return inputAccount;
    }
}




