namespace QuickConsole;


public class QuickConsoleRunArgsManager                                    
{
    private readonly string[] _args;

    private readonly string _hotelsArgsSwitch;
    private readonly string _bookingsArgsSwitch;

    public QuickConsoleRunArgsManager(QuickConsoleRunArgs runCommandArgs)
    {
        _args = runCommandArgs.args;

        _hotelsArgsSwitch = "hotels";
        _bookingsArgsSwitch = "bookings";
    }

    public (string HotelsFilename, string BookingsFilname) Parse()
    {
        var hotelRepositoryFilename = GetParameterValue(_hotelsArgsSwitch);
        var bookingRepositoryFilename = GetParameterValue(_bookingsArgsSwitch);

        if (hotelRepositoryFilename is null || bookingRepositoryFilename is null)
            throw new ArgumentException("One or more underlying datafile names or parameters' switch not given properly.");

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

public class QuickConsoleRunArgs
{
    public string[] args = [];
}
