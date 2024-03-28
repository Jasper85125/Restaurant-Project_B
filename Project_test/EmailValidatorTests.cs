using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class EmailValidatorTests
{
    [TestMethod]
    public void IsValidEmail_ValidEmail_ReturnsTrue()
    {
        AccountsLogic validator = new AccountsLogic ();

        bool result = validator.IsValidEmail("test@example.com");

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsValidEmail_InvalidEmail_ReturnsFalse()
    {
        AccountsLogic validator = new AccountsLogic ();

        bool result = validator.IsValidEmail("invalid.email");

        Assert.IsFalse(result);
    }
}