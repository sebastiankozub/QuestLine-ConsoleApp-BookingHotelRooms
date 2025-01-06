using ConsoleBookingApp;

namespace ConsoleBookingAppTest;

[TestFixture]
public class ConsoleBookingAppArgsParserTests
{
    [Test]
    public void Parse_WithValidArgs_ReturnsFilenames()
    {
        var args = new string[]
        {
            "--hotels",
            "hotels.txt",
            "--bookings",
            "bookings.txt"
        };

        var parser = new ConsoleBookingAppArgsParser(new ConsoleAppArgs { args = args });

        var result = parser.Parse();

        Assert.Multiple(() =>
        {
            Assert.That(result.HotelsFilename, Is.EqualTo("hotels.txt"));
            Assert.That(result.BookingsFilname, Is.EqualTo("bookings.txt"));
        });
    }

    [Test]
    public void Parse_WithArgsOrderChanged_ReturnsFilenames()  // change into parametrized with TestCaseSource
    {
        var args = new string[]
        {
            "--bookings",
            "bookings.txt",
            "--hotels",
            "hotels.txt"
        };

        var parser = new ConsoleBookingAppArgsParser(new ConsoleAppArgs { args = args });

        var result = parser.Parse();

        Assert.Multiple(() =>
        {
            Assert.That(result.HotelsFilename, Is.EqualTo("hotels.txt"));
            Assert.That(result.BookingsFilname, Is.EqualTo("bookings.txt"));
        });
    }

    [Test]
    public void Parse_WithMissingArgs_ThrowsArgumentException()
    {
        var args = new string[]
        {
            "--hotels",
            "hotels.txt"
        };

        Assert.Throws<ArgumentException>(() => new ConsoleBookingAppArgsParser(new ConsoleAppArgs { args = args }));
    }

    [Test]
    public void Parse_WithInvalidArgs_ThrowsArgumentException()
    {
        var args = new string[]
        {
            "--hotels",
            "hotels.txt",
            "--bookings"
        };

        Assert.Throws<ArgumentException>(() => new ConsoleBookingAppArgsParser(new ConsoleAppArgs { args = args }));
    }
}
