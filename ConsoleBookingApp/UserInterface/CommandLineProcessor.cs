using ConsoleBookingApp.Configuration;
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

    //test only
    private readonly MyFirstClass _first;
    private readonly SecondOptions _second;

    public CommandLineProcessor(ICommandLineParser parser, Dictionary<string, ICommandLineHandler> handlers, IOptions<UserInterfaceOptions> userInterfaceOptions, MyFirstClass first, SecondOptions second)
    {
        _parser = parser;
        _commandLineHandlers = handlers;

        _uiOptions = userInterfaceOptions.Value;

        _helpCommandName = _uiOptions.CommandsAliases?.Help ?? nameof(UserInterfaceOptions.CommandsAliases.Help);
        _exitCommandName = _uiOptions.CommandsAliases?.Exit ?? nameof(UserInterfaceOptions.CommandsAliases.Exit);

        _first = first;
        _second = second;
    }

    public async Task<CommandLineProcessorResult> ProcessCommandAsync(string commandLine)
    {
        if(string.IsNullOrEmpty(commandLine))
        {
            return new CommandLineProcessorResult
            {
                Message = $"Empty command. Try {_helpCommandName}() to check for existing commands.",
                Success = false,
                PostResultAction = null
            };
        }

        var (commandName, parameters) = _parser.Parse(commandLine);

        if (commandName == null)
        {
            return new CommandLineProcessorResult
            {
                Message = $"Invalid command format. Mind even empty parameters list command has to end with round brackets. Try {_helpCommandName}() to check for existing commands.",
                Success = false,
                PostResultAction = null
            };                       
        }

        if (commandName == _helpCommandName)
        {
            return new CommandLineProcessorResult
            {
                Message = BuildHelpInfo(),
                Success = true,
                PostResultAction = null
            };
        }

        if (commandName == _exitCommandName)
        {
            return new CommandLineProcessorResult
            {
                Message = $"{_exitCommandName}() command received. Exiting application...",
                Success = true,
                PostResultAction = () => ConsoleBookingAppEntry.ExitApplication(0)
            };
        }

        if (_commandLineHandlers.TryGetValue(commandName, out var commandLineHandler))
        {
            var commandResult = await commandLineHandler.HandleAsync(parameters);

            return new CommandLineProcessorResult
            {
                Message = commandResult.Message,
                Success = true,
                PostResultAction = null
            };
        }
        else
        {
            return new CommandLineProcessorResult
            {
                Message = $"Command '{commandName}()' not found. Try {_helpCommandName}() to check for existing commands.",
                Success = false,
                PostResultAction = () => Environment.Exit(0)
            };
        }
    }
    
    private string BuildHelpInfo()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Available commands:");

        foreach (var handler in _commandLineHandlers)
            sb.AppendLine($"{handler.Value.CommandName}()");

        return sb.ToString();
    }
}

public class CommandLineProcessorResult
{
    public bool Success { get; set; }
    public required string Message { get; set; }
    public Action? PostResultAction { get; set; }
}
