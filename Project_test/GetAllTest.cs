namespace GetAllTests;

[TestClass]
public class GetAllTests
{
    [TestMethod]
    public void TestGetAll()
    {
        // Arrange
        var accountsLogic = new AccountsLogic();

        // Act
        List<AccountModel> accounts = accountsLogic.GetAll();

        // Assert
        Assert.IsNotNull(accounts, "Accounts list should not be null.");
        Assert.IsTrue(accounts.Count > 0, "Accounts list should contain elements.");
    }
}
