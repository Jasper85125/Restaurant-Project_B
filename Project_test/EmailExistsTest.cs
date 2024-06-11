namespace EmailExistsTest;

[TestClass]
public class EmailExistsTest
{
    private AccountsLogic _accounts;

    [TestInitialize]
    public void Setup()
    {
        _accounts = new AccountsLogic();
    }

    [TestMethod]
    public void EmailExists_ReturnsTrue()
    {
        string existingEmail = "Admin";

        bool emailExists = _accounts.EmailExists(existingEmail);

        Assert.IsTrue(emailExists);
    }

    [TestMethod]
    public void EmailExists_ReturnsFalse()
    {
        string nonExistingEmail = "nonexisting@example.com";

        bool emailExists = _accounts.EmailExists(nonExistingEmail);

        Assert.IsFalse(emailExists);
    }


}