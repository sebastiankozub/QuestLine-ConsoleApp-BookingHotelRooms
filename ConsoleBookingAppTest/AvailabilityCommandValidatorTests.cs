//using BookingData.Model;
//using ConsoleBookingApp.CommandHandler;

//[TestFixture]
//public class AvailabilityCommandValidatorTests
//{
//    [Test]
//    public void Validate_ValidInput_ReturnsTrue(string hotelId, (DateOnly from, DateOnly to) availabilityPerdiod, string roomType)
//    {
//        // Arrange
//        //string hotelId = "ABC";
//        //var availabilityPeriod = (new DateOnly(2022, 1, 1), new DateOnly(2022, 1, 5));
//        //string roomType = "Standard";

//        // Act
//        bool result = SearchCommandValidator.Validate(hotelId, availabilityPeriod, roomType);

//        // Assert
//        Assert.IsTrue(result);
//    }

//    [Test]
//    public void Parse_ValidParameters_ReturnsParsedValues(string[] parameters)
//    {
//        // Arrange
//        //string[] parameters = { "ABC", "3", "Standard" };

//        // Act
//        var result = SearchCommandValidator.Parse(parameters);

//        // Assert
//        Assert.AreEqual("ABC", result.hotelId);
//        Assert.AreEqual((new DateOnly(2022, 1, 2), new DateOnly(2022, 1, 4)), result.availabitlityPeriod);
//        Assert.AreEqual("Standard", result.roomType);
//    }
//}
