using QuickConsole.RunCommand;

namespace QuickConsole;

public  class QuickConsoleEntryPoint
{
    private readonly RunCommandArgsManager? _argsParser;

    public QuickConsoleEntryPoint()
    {            
    }

    public QuickConsoleEntryPoint(RunCommandArgsManager argsParser)
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

