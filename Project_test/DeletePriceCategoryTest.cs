//namespace DeletePriceCategoryTest;

[TestClass]
public class DeletePriceCategoryTest
{
    private PriceLogic _prices;

    [TestInitialize]
    public void Initialize()
    {
        _prices = new PriceLogic();
    }

    [TestMethod]
    public void DeletePriceCategory_RemovesPriceModel()
    {
        // Arrange
        int idToRemove = 1; 
        int initialCount = _prices.GetAll().Count;

        // Act
        _prices.DeletePriceCategory(idToRemove);

        // Assert
        Assert.AreEqual(initialCount - 1, _prices.GetAll().Count, "PriceModel count should decrease by 1 after deletion");
    }

    [TestMethod]
    public void DeletePriceCategory_NoChanges()
    {
        // Arrange
        int idNotInList = -1;
        int initialCount = _prices.GetAll().Count;

        // Act
        _prices.DeletePriceCategory(idNotInList);

        // Assert
        Assert.AreEqual(initialCount, _prices.GetAll().Count, "PriceModel count should remain the same if ID is not found");
    }
}
