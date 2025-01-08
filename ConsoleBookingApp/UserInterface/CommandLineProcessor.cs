using ConsoleBookingApp.CommandHandler;
using ConsoleBookingApp.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ConsoleBookingApp.UserInterface;

internal class CommandLineProcessor
{
    private readonly ICommandLineParser _parser;
    private readonly Dictionary<string, IOldCommandHandler> _oldCommandLineHandlers;
    private readonly Dictionary<string, IHandler<IHandlerResult>> _handlers;

    private readonly string _helpCommand;
    private readonly string _exitCommand;


    private readonly UserInterfaceCommandsOptions _uiCommandsOptions;

    private readonly Action<int>? _closeApplicationAction;

    private readonly IServiceProvider _serviceProvider;

    public CommandLineProcessor(ICommandLineParser parser, Dictionary<string, IOldCommandHandler> oldCommandHandlers, Dictionary<string, IHandler<IHandlerResult>> handlers,
        IOptions<UserInterfaceCommandsOptions> userInterfaceCommandsOptions, Action<int>? closeApplicationAction, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        _parser = parser;
        _handlers = handlers;
        _oldCommandLineHandlers = oldCommandHandlers;

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
            return new HelpCommandLineProcessorResult(_oldCommandLineHandlers);
        
        if (givenCommand == _exitCommand)        
            return new ExitCommandLineProcessorResult(givenCommand, _closeApplicationAction);

        //old handlers
        if ((IsAlias(givenCommand, out var commandFromAlias) 
            && _oldCommandLineHandlers.TryGetValue(commandFromAlias, out var commandHandler))  // not possible to null reference exception
            || _oldCommandLineHandlers.TryGetValue(givenCommand, out commandHandler))  
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

        // new handlers
        if (IsAlias(givenCommand, out commandFromAlias))
        {
            using var scope = _serviceProvider.CreateScope();

            var handler = scope.ServiceProvider.GetKeyedService<IHandler<IHandlerResult>>(commandFromAlias);   // this command is not keyed service key

            if (handler != null)
            {
                var handlerResult = await handler.HandleAsync(givenParameters);

                return new CommandLineProcessorResult
                {
                    Message = handlerResult.Message,
                    Success = handlerResult.Success,
                    //Result = handlerResult., // command & query registered separate
                    ExceptionMessage = handlerResult.ExceptionMessage,
                    PostProcess = null
                };
            }
        }

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


