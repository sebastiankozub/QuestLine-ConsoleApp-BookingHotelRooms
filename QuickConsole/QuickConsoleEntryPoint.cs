namespace QuickConsole;

public  class QuickConsoleEntryPoint
{
    private readonly QuickRunCommandArgsManager? _argsParser;

    public QuickConsoleEntryPoint()
    {            
    }

    public QuickConsoleEntryPoint(QuickRunCommandArgsManager argsParser)
    {
        _argsParser = argsParser;
    }



    public void Run()
    {
        if (_argsParser != null)
        {
            _argsParser.Parse();
        }
        else 
        {
            ;// application will not consider command line params
        }
    }
}

