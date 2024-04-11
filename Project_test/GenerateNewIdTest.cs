//namespace GenerateNewIdTest;

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
        // Arrange
        // _accounts.ResetAccounts();

        // Act
        int newId = _accounts.GenerateNewId();

        // Assert
        Assert.AreEqual(1, newId);
    }

    [TestMethod]
    public void Test_WithExistingAccounts()
    {
        // Arrange
        //_accounts.ResetAccounts();
        _accounts.UpdateList(new AccountModel { Id = 1, Name = "Alice" });
        _accounts.UpdateList(new AccountModel { Id = 3, Name = "Bob" });

        // Act
        int newId = _accounts.GenerateNewId();

        // Assert
        Assert.AreEqual(4, newId);
    }
}
