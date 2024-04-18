// namespace AddRouteTest;

// [TestClass]
// public class AddRouteTest
// {
//     [TestMethod]
//     public void AddRouteTestCorrect()
//     {
//         //Arrange
//         string input = "Beurs";
//         StringReader reader = new StringReader(input);
//         Console.SetIn(reader);
        
//         StopModel expectedOutput = new StopModel("Beurs");

//         //Act
//         StopModel actualOutput = StopMenu.MakeStop();

//         //Assert
//         Assert.AreEqual(expectedOutput.Name, actualOutput.Name);
//     }

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
// }