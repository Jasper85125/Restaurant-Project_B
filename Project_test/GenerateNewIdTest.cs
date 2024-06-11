namespace GenerateNewIdTest;

[TestClass]
public class GenerateNewIdTest
{
    private AccountsLogic _accounts;

    [TestInitialize]
    public void TestInitialize()
    {
        _accounts = new AccountsLogic();
    }

    public void Test_NoExistingAccounts()
    {
        // Act
        int newId = _accounts.GenerateNewId();

        // Assert
        Assert.AreEqual(1, newId);
    }

    [TestMethod]
    public void Test_WithExistingAccounts()
    {
        // Arrange
        _accounts = new();
        _accounts.UpdateList(new AccountModel ( 1, "Yep@g.com", "1234", "Alice", false ));
        _accounts.UpdateList(new AccountModel ( 3, "no@g.com", "4312", "Bob", false ));

        // Act
        int newId = _accounts.GenerateNewId();

        // Assert
        Assert.AreEqual(4, newId);
    }
}
