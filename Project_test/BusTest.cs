namespace BusTest;

[TestClass]
public class BusTest
{
    private BusLogic busLogic;

    [TestInitialize]
    public void TestInitialize()
    {
        busLogic = new BusLogic();
    }

    [DataTestMethod]
    [DataRow("ABC-123")]
    [DataRow("TEST-12")]
    [DataRow("123-PASS")]
    public void TestAddBus(string newLicensePlate)
    {
        // Arrange
        BusModel expected = new BusModel(busLogic.GenerateNewId(), "Business", newLicensePlate);

        // Act
        busLogic.UpdateList(expected);
        List<BusModel> busses = busLogic.GetAll();
        BusModel actual = busses.Last();

        // Assert
        Assert.AreEqual(actual.LicensePlate, expected.LicensePlate);
        Assert.AreEqual(actual.Id, expected.Id);
        Assert.AreEqual(actual, expected);
    }

    [DataTestMethod]
    [DataRow("ABC-123")]
    [DataRow("TEST-12")]
    [DataRow("123-PASS")]
    public void TestDeleteBus(string newLicensePlate)
    {
        // Arrange
        BusModel expected = new BusModel(busLogic.GenerateNewId(), "Business", newLicensePlate);

        // Act
        expected.IsActive = false;
        busLogic.UpdateList(expected);
        List<BusModel> busses = busLogic.GetAll();
        BusModel actual = busses.Last();

        // Assert
        Assert.AreEqual(actual.IsActive, expected.IsActive);
    }
}

