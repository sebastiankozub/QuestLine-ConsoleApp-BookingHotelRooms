using QuickConsole.OldCommandHandler;
using QuickConsole.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using QuickConsole.Handler;
using QuickConsole;
using QuickConsole.Options;
using Microsoft.Extensions.Options;

namespace QuickConsole.Core;

internal class CommandLineProcessor
{
    private readonly ILineCommandParser _parser;
    private readonly Dictionary<string, IOldCommandHandler> _oldCommandLineHandlers;
    //private readonly Dictionary<string, IHandler> _handlers;
    private readonly Dictionary<string, string> _newHandlerDefaultNames;

    private readonly string _helpCommand;
    private readonly string _exitCommand;

    private readonly UserInterfaceCommandsOptions _uiCommandsOptions;

    private readonly Action<int>? _closeApplicationAction;

    private readonly IServiceProvider _serviceProvider;

    public CommandLineProcessor(ILineCommandParser parser, Dictionary<string, IOldCommandHandler> oldCommandHandlers,
        Dictionary<string, string> newHandlerDefaultNames,
        IOptions<UserInterfaceCommandsOptions> userInterfaceCommandsOptions, Action<int>? closeApplicationAction, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        _parser = parser;
        _oldCommandLineHandlers = oldCommandHandlers;
        _newHandlerDefaultNames = newHandlerDefaultNames;
        _uiCommandsOptions = userInterfaceCommandsOptions.Value;

        _helpCommand = _uiCommandsOptions.Help ?? nameof(UserInterfaceCommandsOptions.Help);
        _exitCommand = _uiCommandsOptions.Exit ?? nameof(UserInterfaceCommandsOptions.Exit);

        _closeApplicationAction = closeApplicationAction;
    }

    public async Task<CommandLineProcessorResult> ProcessCommandAsync(string commandLine)  
    {
        var (givenCommand, givenParameters) = _parser.Parse(commandLine);

        if (string.IsNullOrEmpty(commandLine))
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

        // SERVICE NEW HANDLER
        var defaultHandlerName = _newHandlerDefaultNames[givenCommand];   // can throw exception when key found - will change into alias resolver

        using var scope = _serviceProvider.CreateScope();

        var handler = scope.ServiceProvider.GetKeyedService<IHandler>(defaultHandlerName);

        if (handler != null)
        {
            var handlerResult = await handler.HandleAsync(givenParameters);

            if (handlerResult is CommandHandlerResult commandHandlerResult)
            {
                var result = commandHandlerResult.ResultData;


                return new CommandLineProcessorResult
                {
                    Message = handlerResult.Message,
                    Success = handlerResult.Success,
                    Result = commandHandlerResult.ResultData,
                    PostProcess = null
                };
            }

            if (handlerResult is QueryHandlerResult queryHandlerResult)
            {
                var stringTable = queryHandlerResult.ResultData;
                var sb = new StringBuilder();

                foreach (var line in stringTable)
                    sb.AppendLine(line);                

                return new CommandLineProcessorResult
                {
                    Message = handlerResult.Message,
                    Success = handlerResult.Success,
                    Result = sb.ToString(),
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


