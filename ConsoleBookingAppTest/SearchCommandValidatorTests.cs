using ConsoleBookingApp.CommandHandler;
using System.Collections;

namespace ConsoleBookingAppTest;

[TestFixture]
public class SearchCommandValidatorTests
{
    private static DateOnly GetTommorowUtcDateOnly() => DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

    [TestCaseSource(nameof(ValidatorPositiveTestCases))]
    public bool Validate_Returns_True(string hotelId, (DateOnly from, DateOnly to) availabilityPeriod, string roomType)
    {
        return SearchCommandValidator.Validate(hotelId, availabilityPeriod, roomType);    
    }

    [TestCaseSource(nameof(ValidatorNegativeTestCases))]
    public void Validate_Throws_SearchCommandHandlerValidateException(string hotelId, (DateOnly from, DateOnly to) availabilityPeriod, string roomType)
    {
        Assert.That(() => SearchCommandValidator.Validate(hotelId, availabilityPeriod, roomType), 
            Throws.Exception.TypeOf<SearchCommandHandlerValidateException>());
    }

    private static IEnumerable<TestCaseData> ValidatorPositiveTestCases()
    {
        var tommorow = GetTommorowUtcDateOnly();

        yield return
            new TestCaseData("ABC", (tommorow, tommorow), "Standard")
                .SetName("Validate_Returns_True1")
                .Returns(true);
        yield return
            new TestCaseData("DE-FG", (tommorow, tommorow.AddDays(1)), "Del uxe")
                .SetName("Validate_Returns_True2")
                .Returns(true);
        yield return
            new TestCaseData("111 GHI", (tommorow, tommorow.AddMonths(10)), "Standard ")
                .SetName("Validate_Returns_True3")
                .Returns(true);
        yield return
            new TestCaseData("GH4I", (tommorow, tommorow.AddYears(3)), "Standard")
                .SetName("Validate_Returns_True4")
                .Returns(true);
    }

    private static IEnumerable<TestCaseData> ValidatorNegativeTestCases()
    {
        var tommorow = GetTommorowUtcDateOnly();

        yield return
            new TestCaseData("A", (tommorow, tommorow), "Standard")
            .SetName("Validate_Throws_SearchCommandHandlerValidateException1");     // A
        yield return
            new TestCaseData("DE43345345345FGWERRTY", (tommorow.AddDays(-5), tommorow), "Deluxee")
            .SetName("Validate_Throws_SearchCommandHandlerValidateException2");     // DE43345345345FGWERRTY   
        yield return
            new TestCaseData("DE-FG", (tommorow, tommorow.AddDays(1)), "")
            .SetName("Validate_Throws_SearchCommandHandlerValidateException3");     // e
        yield return
            new TestCaseData("DE-FG", (tommorow, tommorow.AddDays(1)), "Deluxeeeeeeeeee")
            .SetName("Validate_Throws_SearchCommandHandlerValidateException4");     // Deluxe
        yield return
            new TestCaseData("111GHI", (tommorow.AddDays(-10), tommorow.AddMonths(10)), "Standard")
            .SetName("Validate_Throws_SearchCommandHandlerValidateException5");     // from the past 
        yield return
            new TestCaseData("GH4I", (tommorow.AddDays(-1), tommorow.AddYears(3)), "Standard")
            .SetName("Validate_Throws_SearchCommandHandlerValidateException6");     // from today
        yield return
            new TestCaseData("C", (tommorow.AddDays(-2), tommorow.AddDays(-2)), "Standard")
            .SetName("Validate_Throws_SearchCommandHandlerValidateException7");     // yesterday only
        yield return
            new TestCaseData("111GHI", (tommorow.AddDays(-1), tommorow.AddDays(-1)), "Standard")
            .SetName("Validate_Throws_SearchCommandHandlerValidateException8");     // today only
    }
}
