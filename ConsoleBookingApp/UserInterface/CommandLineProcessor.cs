using ConsoleBookingApp.Configuration;
using ConsoleBookingApp.UserInterfacepublic;
using Microsoft.Extensions.Options;
using System.Text;

namespace ConsoleBookingApp.UserInterface;

public class CommandLineProcessor
{
    private readonly ICommandLineParser _parser;
    private readonly Dictionary<string, ICommandLineHandler> _commandLineHandlers;

    private readonly string _helpCommandName;
    private readonly string _exitCommandName;

    private readonly UserInterfaceOptions _uiOptions;
    private readonly UserInterfaceCommandsOptions _uiCommandsOptions;

    //test only
    private readonly MyFirstClass _first;
    private readonly SecondOptions _second;

    public CommandLineProcessor(ICommandLineParser parser, Dictionary<string, ICommandLineHandler> handlers, IOptions<UserInterfaceOptions> userInterfaceOptions, IOptions<UserInterfaceCommandsOptions> userInterfaceCommandsOptions, MyFirstClass first, SecondOptions second)
    {
        _parser = parser;
        _commandLineHandlers = handlers;

        _uiOptions = userInterfaceOptions.Value;
        _uiCommandsOptions = userInterfaceCommandsOptions.Value;

        _helpCommandName = _uiCommandsOptions.Help ?? nameof(UserInterfaceCommandsOptions.Help);
        _exitCommandName = _uiCommandsOptions.Exit ?? nameof(UserInterfaceCommandsOptions.Exit);

        _first = first;
        _second = second;
    }

    public async Task<CommandLineProcessorResult> ProcessCommandAsync(string commandLine)  
    {
        if(string.IsNullOrEmpty(commandLine))        
            return new EmptyCommandLineProcessorResult();
        
        var (givenCommand, givenParameters) = _parser.Parse(commandLine);

        if (givenCommand == null)        
            return new InvalidFormatCommandLineProcessorResult(_helpCommandName);        

        if (givenCommand == _helpCommandName)        
            return new HelpCommandLineProcessorResult(_commandLineHandlers);
        
        if (givenCommand == _exitCommandName)        
            return new ExitCommandLineProcessorResult(givenCommand);

        // TODO create and use AliasResolver
        if ((IsAlias(givenCommand, out var commandFromAlias) && _commandLineHandlers.TryGetValue(commandFromAlias, out var commandLineHandler))
            || _commandLineHandlers.TryGetValue(givenCommand, out commandLineHandler))  
        {
            var commandResult = await commandLineHandler.HandleAsync(givenParameters);

            return new CommandLineProcessorResult
            {
                Message = commandResult.Message,
                Success = true,
                PostProcess = null
            };
        }
        else        
            return new NotFoundCommandLineProcessorResult(givenCommand);        
    }
    
    private bool IsAlias(string alias, out string? defaultCommand)   
                    // TODO refactor to use CommandLineAliasResolver - given command or alias return default command
    {
        var aliasFound = false;
        defaultCommand = null;

        if (_uiCommandsOptions.Search == alias)
        {
            defaultCommand = "Search";
            aliasFound = true;
        }

        if (_uiCommandsOptions.Availability == alias)
        {
            defaultCommand = "Availability";
            aliasFound = true;
        }

        return aliasFound;
    }
}


