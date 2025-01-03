namespace ConsoleBookingApp.UserInterface;

internal class ConsoleAppArgsParser
{
    private readonly string[] _args;

    private readonly string _hotelsArgsSwitch;
    private readonly string _bookingsArgsSwitch;

    public ConsoleAppArgsParser(ConsoleAppArgsParserOptions options)
    {
        if (options.args == null || options.args.Length < 4)
            throw new ArgumentException("Command line arguments not satisfied. Run app with proper --bookings and --hotels parameters' values.");

        _args = options.args;

        _hotelsArgsSwitch = "hotels";
        _bookingsArgsSwitch = "bookings";
    }

    public (string HotelsFilename, string BookingsFilname) Parse()
    {
        var hotelRepositoryFilename = GetParameterValue(_hotelsArgsSwitch);
        var bookingRepositoryFilename = GetParameterValue(_bookingsArgsSwitch);

        if (hotelRepositoryFilename is null || bookingRepositoryFilename is null)
            throw new ArgumentException("One or more underlying datafile names not given properly.");

        return (hotelRepositoryFilename, bookingRepositoryFilename);
    }

    private string? GetParameterValue(string key)
    {
        int index = Array.IndexOf(_args, "--" + key);

        if (index >= 0 && _args.Length > index)
            return _args[index + 1];

        return null;
    }
}

internal class ConsoleAppArgsParserOptions
{
    public string[] args = [];
}
