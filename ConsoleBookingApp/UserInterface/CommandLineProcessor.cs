using ConsoleBookingApp.CommandHandler;
using ConsoleBookingApp.Configuration;
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

    private readonly Action<int>? _closeApplicationAction;

    //test only
    private readonly MyFirstClass _first;
    private readonly SecondOptions _second;

    public CommandLineProcessor(ICommandLineParser parser, Dictionary<string, ICommandHandler> handlers, IOptions<UserInterfaceOptions> userInterfaceOptions, 
                            IOptions<UserInterfaceCommandsOptions> userInterfaceCommandsOptions, MyFirstClass first, SecondOptions second, Action<int>? closeApplicationAction)
    {
        _parser = parser;
        _commandLineHandlers = handlers;

        _uiOptions = userInterfaceOptions.Value;
        _uiCommandsOptions = userInterfaceCommandsOptions.Value;

        _helpCommand = _uiCommandsOptions.Help ?? nameof(UserInterfaceCommandsOptions.Help);
        _exitCommand = _uiCommandsOptions.Exit ?? nameof(UserInterfaceCommandsOptions.Exit);

        _closeApplicationAction = closeApplicationAction;

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
            return new ExitCommandLineProcessorResult(givenCommand, _closeApplicationAction);

        // TODO The Dictionary is a list of Transient instances but not reinstatined as new transient but taken instantinated from dictionaty

        // TODO refactor to use CommandLineAliasResolver or better naming BookingAppAliasResolver : IAliasResolver?
        // given command or alias Resolve() return default command string  - null reference warning also cleaned then
        if ((IsAlias(givenCommand, out var commandFromAlias) 
            && _commandLineHandlers.TryGetValue(commandFromAlias, out var commandHandler))
            || _commandLineHandlers.TryGetValue(givenCommand, out commandHandler))  
        {
            var commandResult = await commandHandler.HandleAsync(givenParameters);

            return new CommandLineProcessorResult
            {
                Message = commandResult.Message,
                Success = commandResult.Success,
                Result = commandResult.ResultData,
                PostProcess = null
            };
        }

        // snippet to get transient every user command triggered
        //
        //using (var scope = _serviceProvider.CreateScope())
        //{
        //    var handlers = scope.ServiceProvider.GetServices<ICommandHandler>();
        //    var handler = handlers.FirstOrDefault(h => input.StartsWith(h.CommandName));

        //    if (handler != null)
        //    {
        //        string commandData = input.Substring(handler.CommandName.Length).Trim();
        //        await handler.HandleAsync(commandData);
        //    }
        //    else
        //    {
        //        Console.WriteLine("Unknown command.");
        //    }
        //}

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


