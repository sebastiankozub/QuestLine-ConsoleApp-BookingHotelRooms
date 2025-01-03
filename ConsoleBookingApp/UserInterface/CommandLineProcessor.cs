using ConsoleBookingApp.Configuration;
using ConsoleBookingApp.Core.CommandHandler;
using Microsoft.Extensions.Options;

namespace ConsoleBookingApp.UserInterface;

public class CommandLineProcessor
{
    private readonly ICommandLineParser _parser;
    private readonly Dictionary<string, ICommandHandler> _commandLineHandlers;

    private readonly string _helpCommand;
    private readonly string _exitCommand;

    private readonly UserInterfaceOptions _uiOptions;
    private readonly UserInterfaceCommandsOptions _uiCommandsOptions;

    //test only
    private readonly MyFirstClass _first;
    private readonly SecondOptions _second;

    public CommandLineProcessor(ICommandLineParser parser, Dictionary<string, ICommandHandler> handlers, IOptions<UserInterfaceOptions> userInterfaceOptions, IOptions<UserInterfaceCommandsOptions> userInterfaceCommandsOptions, MyFirstClass first, SecondOptions second)
    {
        _parser = parser;
        _commandLineHandlers = handlers;

        _uiOptions = userInterfaceOptions.Value;
        _uiCommandsOptions = userInterfaceCommandsOptions.Value;

        _helpCommand = _uiCommandsOptions.Help ?? nameof(UserInterfaceCommandsOptions.Help);
        _exitCommand = _uiCommandsOptions.Exit ?? nameof(UserInterfaceCommandsOptions.Exit);

        _first = first;
        _second = second;
    }

    public async Task<CommandLineProcessorResult> ProcessCommandAsync(string commandLine)  
    {
        if(string.IsNullOrEmpty(commandLine))        
            return new EmptyCommandLineProcessorResult(_helpCommand);
        
        var (givenCommand, givenParameters) = _parser.Parse(commandLine);

        if (givenCommand == null)        
            return new InvalidFormatCommandLineProcessorResult(_helpCommand);        

        if (givenCommand == _helpCommand)        
            return new HelpCommandLineProcessorResult(_commandLineHandlers);
        
        if (givenCommand == _exitCommand)        
            return new ExitCommandLineProcessorResult(givenCommand);

        // TODO refactor to use CommandLineAliasResolver better BookingAppAliasResolver : IAliasResolver
        // given command or alias Resolve() return default command string  - null reference warning also cleaned then
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
                    // TODO refactor to use CommandLineAliasResolver - given command or alias Resolve return default command string
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


