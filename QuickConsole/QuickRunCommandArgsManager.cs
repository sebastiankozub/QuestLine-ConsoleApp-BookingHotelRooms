namespace QuickConsole;


public class QuickRunCommandArgsManager
{
    private readonly string[] _args;

    private readonly IQuickRunCommandnArgsParser _runArgsParser;

    private readonly Dictionary<string, string> _runArgsDict = new Dictionary<string, string>();

    public QuickRunCommandArgsManager(QuickRunCommandArgs commandRunArgs, IQuickRunCommandnArgsParser runArgsParser)
    {
        _args = commandRunArgs.args;
        _runArgsParser = runArgsParser;

        _runArgsDict = runArgsParser.Parse();
    }

    //private readonly string _hotelsArgsSwitch = "hotels";
    //private readonly string _bookingsArgsSwitch = "bookings";

    public (string HotelsFilename, string BookingsFilname) Parse()
    {
        //var hotelRepositoryFilename = GetParameterValue(_hotelsArgsSwitch);
        //var bookingRepositoryFilename = GetParameterValue(_bookingsArgsSwitch);

        //if (hotelRepositoryFilename is null || bookingRepositoryFilename is null)
            throw new ArgumentException("One or more underlying datafile names or parameters' switch not given properly.");

        //return (hotelRepositoryFilename, bookingRepositoryFilename);
    }

    private string? GetParameterValue(string key)
    {
        int index = Array.IndexOf(_args, "--" + key);

        if (index >= 0 && _args.Length > index)
            return _args[index + 1];

        return null;
    }
}

public class QuickRunCommandArgs
{
    public string[] args = [];
}
