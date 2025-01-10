using QuickConsole.RunCommand;

namespace QuickConsole;

public  class QuickEntryPoint
{
    private readonly RunCommandArgsManager? _argsParser;

    public QuickEntryPoint()
    {            
    }

    public QuickEntryPoint(RunCommandArgsManager argsParser)
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

