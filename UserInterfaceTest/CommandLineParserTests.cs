using ConsoleBookingApp.UserInterface;

namespace UserInterfaceTest;

public class CommandLineParserTests
{
    private CommandLineParser _commandLineParser;

    [OneTimeSetUp]
    public void Setup()
    {
        _commandLineParser = new CommandLineParser();
    }

    [TestCase("AddBooking(John Doe,2022-01-01)")]
    [TestCase("AddBooking(John Doe,)")]
    [TestCase("AddBooking(   John Doe  ,    )")]        // command parser should not validate parameters values only parameters lenght
    [TestCase("AddBooking(John Doe, 2022-   01-01)")] 
    [TestCase("AddBooking (John Doe,     2022-01-01)")]
    [TestCase("AddBooking ( John Doe   ,  2022-01-01     )")]
    [TestCase("AddBooking(John Doe, 2022-01-01        )")]
    [TestCase("     AddBooking      (           John Doe, 2022-01-01)")]
    [Parallelizable(ParallelScope.All)]
    public void Parse_ValidCommand_ReturnsCommandNameAndParameters(string command)
    { 
        var result = _commandLineParser.Parse(command);

        Assert.That(result.CommandName, Is.EqualTo("AddBooking"));
        Assert.That(result.Parameters.Length, Is.EqualTo(2));
        Assert.That(result.Parameters[0], Is.EqualTo("John Doe"));
    }

    [TestCase("AddBooking(,)")]
    public void Parse_ValidCommandWithEmptyParams_ReturnsCommandNameAndParameters(string command)
    {
        var result = _commandLineParser.Parse(command);

        Assert.That(result.CommandName, Is.EqualTo("AddBooking"));
        Assert.That(result.Parameters.Length, Is.EqualTo(2));
        Assert.That(result.Parameters[0], Is.EqualTo(""));
        Assert.That(result.Parameters[1], Is.EqualTo(""));
    }

    [TestCase("AddBooking John Doe, 20220101)")]
    [TestCase("AddBo oking(John Doe, 20220101)")]
    [TestCase("AddBooking(John Doe, 20220101")]
    [TestCase(" (John Doe, 2022-01-01)")]
    [TestCase("AddBooking)")]
    [TestCase("AddBooking")]
    [Parallelizable(ParallelScope.All)]
    public void Parse_InvalidCommand_ReturnsEmptyCommandNameAndParameters(string command)
    {
        var result = _commandLineParser.Parse(command);

        Assert.That(result.CommandName, Is.EqualTo(""));
        Assert.That(result.Parameters.Length, Is.EqualTo(0));
    }
}
