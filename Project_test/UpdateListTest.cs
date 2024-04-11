//namespace UpdateListTest;

[TestClass]
public class UpdateListTest
{
    private AccountsLogic _accounts;

    [TestInitialize]
    public void TestInitialize()
    {
        _accounts = new AccountsLogic();
    }

    [TestMethod]
    public void TestUpdateList_UpdateExistingAccount()
    {
        // Arrange
        AccountModel existingAccount = new AccountModel { Id = 1, Name = "Alice" };
        AccountModel updatedAccount = new AccountModel { Id = 1, Name = "Bob" };

        _accounts.UpdateList(existingAccount);

        // Act
        _accounts.UpdateList(updatedAccount);

        // Assert
        Assert.AreEqual("Bob", _accounts.GetAccountById(1).Name);
    }

    [TestMethod]
    public void TestUpdateList_AddNewAccount()
    {
        // Arrange
        AccountModel newAccount = new AccountModel { Id = 2, Name = "Charlie" };

        // Act
        _accounts.UpdateList(newAccount);

        // Assert
        Assert.AreEqual("Charlie", _accounts.GetAccountById(2).Name);
    }
}
