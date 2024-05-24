//namespace StopToRouteTest;

[TestClass]
public class StopToRouteTest
{
    [TestMethod]
    public void CorrectStopToRouteTest()
    {
        //Arrange 
        StopModel stopModel = new StopModel(1, "Beurs");
        RouteModel routeModel = new RouteModel(1, 1, "Euromast");
        List<StopModel> testList = new(){stopModel};

        StopModel stopModel2 = new StopModel(2, "Blaak");
        RouteModel routeModel2 = new RouteModel(2, 2, "Noord");
        List<StopModel> testList2 = new(){stopModel2};

        //Act
        RouteModel newRoute = RouteLogic.AddToRoute(stopModel, routeModel);
        RouteModel newRoute2 = RouteLogic.AddToRoute(stopModel2, routeModel2);

        //Assert
        for (int i = 0; i < testList.Count; i++)
        {
            Assert.AreEqual(newRoute.Stops[i], testList[i]);
        }

        for (int i = 0; i < testList.Count; i++)
        {
            Assert.AreEqual(newRoute2.Stops[i], testList2[i]);
        }
    }
}


