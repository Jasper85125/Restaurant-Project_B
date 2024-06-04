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

        StopModel stopModel3 = new StopModel(3, "Maashaven");
        RouteModel routeModel3 = new RouteModel(3, 3, "Zuid");
        List<StopModel> testList3 = new(){stopModel2};

        StopModel stopModel4 = new StopModel(4, "Stadhuis");
        RouteModel routeModel4 = new RouteModel(4, 4, "Centrum");
        List<StopModel> testList4 = new(){stopModel};

        //Act
        RouteModel newRoute = RouteLogic.AddToRoute(stopModel, routeModel);
        RouteModel newRoute2 = RouteLogic.AddToRoute(stopModel2, routeModel2);
        RouteModel newRoute3 = RouteLogic.AddToRoute(stopModel3, routeModel3);
        RouteModel newRoute4 = RouteLogic.AddToRoute(stopModel4, routeModel4);

        //Assert
        for (int i = 0; i < testList.Count; i++)
        {
            Assert.AreEqual(newRoute.Stops[i], testList[i]);
        }

        for (int i = 0; i < testList.Count; i++)
        {
            Assert.AreEqual(newRoute2.Stops[i], testList2[i]);
        }

        for (int i = 0; i < testList.Count; i++)
        {
            Assert.AreNotEqual(newRoute3.Stops[i], testList3[i]);
        }

        for (int i = 0; i < testList.Count; i++)
        {
            Assert.AreNotEqual(newRoute4.Stops[i], testList4[i]);
        }
    }
}


