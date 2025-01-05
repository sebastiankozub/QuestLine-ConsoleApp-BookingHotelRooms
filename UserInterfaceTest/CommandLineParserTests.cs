using ConsoleBookingApp.UserInterface;

namespace UserInterfaceTest
{
    public class CommandLineParserTests
    {
        private ICommandLineParser _commandLineParser;

        [OneTimeSetUp]
        public void Setup()
        {
            _commandLineParser = new CommandLineParser();
        }

        [Test]
        [TestCase("AddBooking(John Doe,2022-01-01)")]
        [TestCase("AddBooking (John Doe,     2022-01-01)")]
        [TestCase("AddBooking ( John Doe   ,  2022-01-01     )")]
        [TestCase("AddBooking(John Doe, 2022-01-01        )")]
        [TestCase("     AddBooking      (           John Doe, 2022-01-01)")]
        [TestCase("AddBooking(John Doe, 2022-   01-01)")]  // command line parser does validate only command and parameters format - not the parameters value
        [Parallelizable(ParallelScope.All)]
        public void Parse_ValidCommand_ReturnsCommandNameAndParameters(string command)
        { 
            var result = _commandLineParser.Parse(command);

            // Assert
            Assert.That(result.CommandName, Is.EqualTo("AddBooking"));
            Assert.That(result.Parameters.Length, Is.EqualTo(2));
            Assert.That(result.Parameters[0], Is.EqualTo("John Doe"));
            Assert.That(result.Parameters.Length, Is.EqualTo(2));
        }

        [Test]
        [TestCase("AddBooking John Doe, 20220101)")]
        [TestCase("AddBo oking(John Doe, 20220101)")]
        [TestCase("AddBooking(John Doe, 20220101")]
        [TestCase(" (John Doe, 2022-01-01)")]
        [TestCase("AddBooking)")]
        [TestCase("AddBooking")]
        public void Parse_InvalidCommand_ReturnsEmptyCommandNameAndParameters(string command)
        {
            var result = _commandLineParser.Parse(command);

            Assert.That(result.CommandName, Is.EqualTo(""));
            Assert.That(result.Parameters.Length, Is.EqualTo(0));
        }
    }
}