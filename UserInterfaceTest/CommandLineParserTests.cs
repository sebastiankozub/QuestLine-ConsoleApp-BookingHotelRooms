using QuickConsole.Core;

namespace UserInterfaceTest;

public class CommandLineParserTests
{
    private LineCommandParser _commandLineParser;

    [OneTimeSetUp]
    public void Setup()
    {
        _commandLineParser = new LineCommandParser();
    }

    [TestCase("AddBooking(John Doe,2022-01-01)")]
    [TestCase("AddBooking(John Doe,)")]
    [TestCase("AddBooking(   John Doe  ,    )")]
    [TestCase("AddBooking(John Doe, 2022-   01-01)")] 
    [TestCase("AddBooking (John Doe,     2022-01-01)")]
    [TestCase("AddBooking ( John Doe   ,  2022-01-01     )")]
    [TestCase("AddBooking(John Doe, 2022-01-01        )")]
    [TestCase("     AddBooking      (           John Doe, 2022-01-01)")]
    [Parallelizable(ParallelScope.All)]
    public void Parse_ValidCommand_ReturnsCommandNameAndParameters(string command)
    { 
        var (CommandName, Parameters) = _commandLineParser.Parse(command);

        Assert.Multiple(() =>
        {
            Assert.That(CommandName, Is.EqualTo("AddBooking"));
            Assert.That(Parameters, Has.Length.EqualTo(2));
            Assert.That(Parameters[0], Is.EqualTo("John Doe"));
        });
    }

    [TestCase("AddBooking(,)")]
    public void Parse_ValidCommandWithEmptyParams_ReturnsCommandNameAndParameters(string command)
    {
        var (CommandName, Parameters) = _commandLineParser.Parse(command);

        Assert.Multiple(() =>
        {
            Assert.That(CommandName, Is.EqualTo("AddBooking"));
            Assert.That(Parameters, Has.Length.EqualTo(2));
            Assert.That(Parameters[0], Is.EqualTo(""));
            Assert.That(Parameters[1], Is.EqualTo(""));
        });
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
        var (CommandName, Parameters) = _commandLineParser.Parse(command);

        Assert.Multiple(() =>
        {
            Assert.That(CommandName, Is.EqualTo(""));
            Assert.That(Parameters, Is.Empty);
        });
    }
}
