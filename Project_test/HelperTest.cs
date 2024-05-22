namespace HelperTest;

[TestClass]
public class HelperTest
{
    [DataTestMethod]
    [DataRow("", false)]
    [DataRow("Hello", true)]
    public void TestIsValidString(string str, bool expected)
    {
        // Arrange/ Act
        bool actual = Helper.IsValidString(str);
        // Assert
        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DataRow("8", true)]
    [DataRow("1.0", false)]
    [DataRow("Hello World!", false)]
    [DataRow("", false)]
    public void TestIsValidInteger(string str, bool expected)
    {
        // Arrange/ Act
        bool actual = Helper.IsValidInteger(str);
        // Assert
        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DataRow("2", true)]
    [DataRow("3.0", true)]
    [DataRow("-5.0", true)]
    [DataRow("Hello World!", false)]
    [DataRow("", false)]
    public void TestIsValidDouble(string str, bool expected)
    {
        // Arrange/ Act
        bool actual = Helper.IsValidDouble(str);
        // Assert
        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DataRow("6", false)]
    [DataRow("15.0", false)]
    [DataRow("-9.0", false)]
    [DataRow("Hello", true)]
    [DataRow("Hello World", false)]
    [DataRow("Hello World!?@", false)]
    [DataRow("Hello1999", false)]
    [DataRow("", false)]
    public void TestIsOnlyLetter(string str, bool expected)
    {
        // Arrange/ Act
        bool actual = Helper.IsOnlyLetter(str);
        // Assert
        Assert.AreEqual(expected, actual);
    }
}
