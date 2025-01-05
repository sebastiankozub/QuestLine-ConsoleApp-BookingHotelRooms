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

    private readonly UserInterfaceCommandsOptions _uiCommandsOptions;

    private readonly Action<int>? _closeApplicationAction;

    public CommandLineProcessor(ICommandLineParser parser, Dictionary<string, ICommandHandler> handlers,
        IOptions<UserInterfaceCommandsOptions> userInterfaceCommandsOptions, Action<int>? closeApplicationAction)
    {
        _parser = parser;
        _commandLineHandlers = handlers;

        _uiCommandsOptions = userInterfaceCommandsOptions.Value;

        _helpCommand = _uiCommandsOptions.Help ?? nameof(UserInterfaceCommandsOptions.Help);
        _exitCommand = _uiCommandsOptions.Exit ?? nameof(UserInterfaceCommandsOptions.Exit);

        _closeApplicationAction = closeApplicationAction;
    }

    public async Task<CommandLineProcessorResult> ProcessCommandAsync(string commandLine)  
    {
        var (givenCommand, givenParameters) = _parser.Parse(commandLine);

        if (string.IsNullOrEmpty(commandLine))
            // return new EmptyCommandLineProcessorResult(_helpCommand);
            // fulfilling one of task description constraints - maybe alias functionality when refactored will be better?
            return new ExitCommandLineProcessorResult(givenCommand, _closeApplicationAction); 

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
                ExceptionMessage = commandResult.ExceptionMessage,
                PostProcess = null
            };
        }

        // TODO  -- change dictionary and handlers to real transient - now registered as transient but work like singleton

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
            return new NotResolvedCommandLineProcessorResult(commandLine);        
    }
    
    private bool IsAlias(string alias, out string? defaultCommand)   
    // TODO refactor to use CommandLineAliasResolver - given command or alias Resolve return default command string
    {
        var aliasFound = false;
        defaultCommand = null;

        if (_uiCommandsOptions.Search == alias)  // add aliases to config as a json dictionary to foreach them
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


