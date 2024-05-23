namespace AddBusTest;

[TestClass]
public class AddBusTest
{
    private BusLogic busLogic;

    [TestInitialize]
    public void TestInitialize()
    {
        busLogic = new BusLogic();
    }

    [TestMethod]
    public void TestAddBus()
    {
        // Arrange
        string? newLicensePlate = "ABC-123";
        BusModel expected = new BusModel(busLogic.GenerateNewId(), 40, newLicensePlate);

        // Act
        busLogic.UpdateList(expected);
        List<BusModel> busses = busLogic.GetAll();
        BusModel actual = busses.Last();

        // Assert
        Assert.AreEqual(actual.LicensePlate, expected.LicensePlate);
        Assert.AreEqual(actual, expected);
    }
}

