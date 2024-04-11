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
        AccountModel existingAccount = new AccountModel (1, "Yep@g.com", "1234", "Alice");
        AccountModel updatedAccount = new AccountModel (1, "no@g.com", "4312", "Bob" );

        _accounts.UpdateList(existingAccount);

        // Act
        _accounts.UpdateList(updatedAccount);

        // Assert
        Assert.AreEqual("Bob", _accounts.GetById(1).FullName);
    }

    [TestMethod]
    public void TestUpdateList_AddNewAccount()
    {
        // Arrange
        AccountModel newAccount = new AccountModel (2, "kan@g.com", "6245","Charlie" );

        // Act
        _accounts.UpdateList(newAccount);

        // Assert
        Assert.AreEqual("Charlie", _accounts.GetById(2).FullName);
    }
}
