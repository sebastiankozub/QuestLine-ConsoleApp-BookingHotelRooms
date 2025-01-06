using BookingData.Model;
using ConsoleBookingApp.CommandHandler;

namespace ConsoleBookingAppTest;

public class SearchCommandParserTests
{
    private static DateOnly GetTommorowUtcDateOnly() => DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

    [TestCaseSource(nameof(ParserTestPositiveCases))]
    public (string hotelId, (DateOnly from, DateOnly to) availabitlityPeriod, string roomType) 
    Parse_Returns_ParsedValues(string[] parameters)
    {
        return SearchCommandValidator.Parse(parameters);
    }

    [TestCaseSource(nameof(ParserTestNegativeCases))]
    public void Parse_Throws_SearchCommandHandlerParseException(string[] parameters)
    {
        Assert.That(() => SearchCommandValidator.Parse(parameters),
            Throws.Exception.TypeOf<SearchCommandHandlerParseException>());
    }

    //public static object[] DivideCases =
    //{
    //    new object[] { "AB", "zdy", "Standard" },
    //    new object[] { "AB", "dy", "Standard" },
    //    new object[] { "AB", "", "Standard" },
    //    new object[] { "AB", "p", "Standard" },
    //};

    public static IEnumerable<TestCaseData> ParserTestPositiveCases()
    {
        var tommorow = GetTommorowUtcDateOnly();

        int[] testCaseDays = [1, 4, 4, 4, 4];

        var testCaseDaysAsString = testCaseDays.Select(d => d.ToString()).ToArray();
        testCaseDays = testCaseDays.Select(d => (d - 1)).ToArray();

        string[][] p1 = [[" A B ", testCaseDaysAsString[0], "Standard "]];
        var t1 = ("A B", (tommorow, tommorow.AddDays(testCaseDays[0])), "Standard");
        yield return
            new TestCaseData(p1).SetName("SearchParser1").Returns(t1);

        string[][] p2 = [["D E", testCaseDaysAsString[1], "Delu xe"]];
        var t2 = ("D E", (tommorow, tommorow.AddDays(testCaseDays[1])), "Delu xe");
        yield return
            new TestCaseData(p2).SetName("SearchParser2").Returns(t2);

        string[][] p3 = [["G666HI", testCaseDaysAsString[2], "Standard"]];
        var t3 = ("G666HI", (tommorow, tommorow.AddDays(testCaseDays[2])), "Standard");
        yield return
            new TestCaseData(p3).SetName("SearchParser3").Returns(t3);

        string[][] p4 = [["1010101010", testCaseDaysAsString[3], "  Standard  "]];
        var t4 = ("1010101010", (tommorow, tommorow.AddDays(testCaseDays[3])), "Standard");
        yield return
            new TestCaseData(p4).SetName("SearchParser4").Returns(t4);

        string[][] p5 = [["AB", testCaseDaysAsString[4], "Standard"]];
        var t5 = ("AB", (tommorow, tommorow.AddDays(testCaseDays[4])), "Standard");
        yield return
            new TestCaseData(p5).SetName("SearchParser5").Returns(t5);
    }

    public static IEnumerable<TestCaseData> ParserTestNegativeCases()
    {
        var tommorow = GetTommorowUtcDateOnly();

        int[] testCaseDays = [0, -100, 10000, -300, 99999];
        var testCaseDaysAsString = testCaseDays.Select(d => d.ToString()).ToArray();

        string[][] p1 = [[" A B ", testCaseDaysAsString[0], "Standard "]];
        yield return new TestCaseData(p1);

        string[][] p2 = [["D E", testCaseDaysAsString[1], "Deluxe"]];
        yield return new TestCaseData(p2);

        string[][] p3 = [["G666HI", testCaseDaysAsString[2], "Standard"]];
        yield return new TestCaseData(p3);

        string[][] p4 = [["101   0101    010", testCaseDaysAsString[3], " S Standard"]];
        yield return new TestCaseData(p4);

        string[][] p5 = [[" AB ", testCaseDaysAsString[4], "Sta         ndard"]];
        yield return new TestCaseData(p5);
    }
}




