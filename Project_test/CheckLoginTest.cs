//namespace CheckLoginTest;

[TestClass]
public class CheckLoginTest
{
    private AccountsLogic _accounts;

    [TestInitialize]
    public void TestInitialize()
    {
        _accounts = new AccountsLogic();
    }

    [TestMethod]
    public void TestCheckLogin_ShouldReturnAccount()
    {
        // Arrange
        string email = "Admin";
        string password = "Admin";

        // Act
        var result = _accounts.CheckLogin(email, password);

        // Assert
        Assert.IsNotNull(AccountsLogic.CurrentAccount);
        Assert.AreEqual(email, AccountsLogic.CurrentAccount.EmailAddress);
        Assert.AreEqual(password, AccountsLogic.CurrentAccount.Password);
    }

    [TestMethod]
    public void TestCheckLogin_ShouldReturnNull()
    {
        // Act
        var result = _accounts.CheckLogin(null, null);

        // Assert
        Assert.IsNull(result);
    }
}

