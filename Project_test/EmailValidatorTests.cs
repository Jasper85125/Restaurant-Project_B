namespace EmailValidatorTests;

[TestClass]
public class EmailValidatorTests
{
    [TestMethod]
    public void IsValidEmail_True()
    {
        string validEmail = "test@email.com";

        AccountsLogic validator = new AccountsLogic();

        bool result = validator.IsValidEmail(validEmail);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsValidEmail_False()
    {
        string invalidEmail = "invalid.email";

        AccountsLogic validator = new AccountsLogic();

        bool result = validator.IsValidEmail(invalidEmail);

        Assert.IsFalse(result);
    }
}