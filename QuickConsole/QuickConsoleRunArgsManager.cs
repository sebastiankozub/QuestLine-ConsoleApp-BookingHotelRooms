namespace QuickConsole;


public class QuickConsoleRunArgsManager(QuickConsoleRunArgs runCommandArgs)
{
    private readonly string[] _args = runCommandArgs.args;

    private readonly string _hotelsArgsSwitch = "hotels";
    private readonly string _bookingsArgsSwitch = "bookings";

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
