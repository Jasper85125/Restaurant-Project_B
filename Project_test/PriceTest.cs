namespace PriceTest;

[TestClass]
public class PriceTest
{
    private PriceLogic priceLogic;

    [TestInitialize]
    public void TestInitialize()
    {
        priceLogic = new PriceLogic();
    }

    [DataTestMethod]
    [DataRow("old", 15.00)]
    [DataRow("adult", 20.00)]
    public void TestAddPrice(string passenger, double price)
    {
        // Arrange
        PriceModel expected = new(priceLogic.GenerateNewId(), passenger,  price);

        // Act
        priceLogic.UpdateList(expected);
        List<PriceModel> prices = priceLogic.GetAll();
        PriceModel actual = prices.Last();

        // Assert
        Assert.AreEqual(actual.Passenger, expected.Passenger);
        Assert.AreEqual(actual, expected);
    }

    
}

