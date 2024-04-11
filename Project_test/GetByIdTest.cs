//namespace GetByIdTest;

[TestClass]
public class GetByIdTest
{
    
    private AccountsLogic _accounts;

    [TestInitialize]
    public void TestInitialize()
    {
        _accounts = new AccountsLogic();
    }
    
    [TestMethod]
    public void GetById_ReturnsCorrectAccount()
    {
        // Arrange
        int accountId = 1;

        // Act
        var account = _accounts.GetById(accountId);

        // Assert
        Assert.IsNotNull(account);
        Assert.AreEqual(accountId, account.Id);
    }
}
