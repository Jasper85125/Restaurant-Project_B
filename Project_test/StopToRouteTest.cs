namespace StopToRouteTest;

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

        //Act
        RouteModel newRoute = RouteLogic.AddToRoute(stopModel, routeModel);

        //Assert
        for (int i = 0; i < testList.Count; i++)
        {
            Assert.AreEqual(newRoute.Stops[i], testList[i]);
        }
    }
    
    // [TestMethod]
    // public void CorrectMakeStopTest()
    // {
    //     //Arrange
    //     string input = "Beurs";
    //     StringReader reader = new StringReader(input);
    //     Console.SetIn(reader);
        
    //     StopModel expectedOutput = new StopModel("Beurs");

    //     //Act
    //     StopModel actualOutput = StopMenu.MakeStop();

    //     //Assert
    //     Assert.AreEqual(expectedOutput.Name, actualOutput.Name);
    // }

//     [TestMethod]
//     public void WrongInputStopTest()
//     {
//         //Arrange
//         StringWriter output = new StringWriter();
//         Console.SetOut(output);

//         string input = @"Beurs2";
//         StringReader reader = new StringReader(input);
//         Console.SetIn(reader);

//         string input2 = @"Blaak";
//         StringReader reader2 = new StringReader(input2);
//         Console.SetIn(reader2);

//         string expectedOutputConsole = @"Wat is de naam van de tussenstop?
// ";
//         StopModel expectedOutput = new StopModel("Blaak");

//         //Act
//         StopModel actualOutput = StopMenu.MakeStop();

//         //Assert
//         Assert.AreEqual(expectedOutputConsole, output.ToString());
//         Assert.AreEqual(actualOutput.Name, expectedOutput.Name);
//     }
}


//  public class Program
//     {
//         public static void Main(string[] args)
//         {
//             Console.WriteLine("What's your name?");
//             var name = Console.ReadLine();
//             Console.WriteLine("How old are you?");
//             var age = Console.ReadLine();
//             Console.WriteLine("Hello {name}, you are {age} years old!!");
//         }
//     }
    
//     [Fact]
//     public void TestConsoleMultpleInputs()
//     {
//         var output = new StringWriter();
//         Console.SetOut(output);

//         var input = new StringReader(@"Somebody 99");
//         Console.SetIn(input);

//         Program.Main(new string[] { });

//         var expectedOutput = @"What's your name?
// How old are you?
// Hello Somebody, you are 99 years old!!
// ";
//         Assert.Equal(expectedOutput, output.ToString());
//     }