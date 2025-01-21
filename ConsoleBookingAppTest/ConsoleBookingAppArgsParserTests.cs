using QuickConsole;

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

        var (HotelsFilename, BookingsFilname) = parser.Parse();

        Assert.Multiple(() =>
        {
            Assert.That(HotelsFilename, Is.EqualTo("hotels.txt"));
            Assert.That(BookingsFilname, Is.EqualTo("bookings.txt"));
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

        var (HotelsFilename, BookingsFilname) = parser.Parse();

        Assert.Multiple(() =>
        {
            Assert.That(HotelsFilename, Is.EqualTo("hotels.txt"));
            Assert.That(BookingsFilname, Is.EqualTo("bookings.txt"));
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

        Assert.That(() => new ConsoleBookingAppArgsParser(new ConsoleAppArgs { args = args }), Throws.Exception.TypeOf<ArgumentException>());
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

        Assert.That(() => new ConsoleBookingAppArgsParser(new ConsoleAppArgs { args = args }), Throws.Exception.TypeOf<ArgumentException>());
    }

    [Test]
    public void Parse_WithInvalidSwitchArg_ThrowsArgumentException()
    {
        var args = new string[]
        {
            "--hotel",   // should be --hotels
            "hotels.txt",
            "--bookings",
            "bookings.txt"
        };

        Assert.That(() => { var parser = new ConsoleBookingAppArgsParser(new ConsoleAppArgs { args = args }); parser.Parse(); },
            Throws.Exception.TypeOf<ArgumentException>());
    }
}
