using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBookingApp.UserInterface;

public class CommandLineProcessor
{
    private readonly ICommandLineParser _parser;
    private readonly Dictionary<string, ICommandLineHandler> _commandLineHandlers;
    private readonly string _helpCommandName = "Help";
    private readonly string _exitCommandName = "Exit";

    public CommandLineProcessor(ICommandLineParser parser, Dictionary<string, ICommandLineHandler> handlers)
    {
        _parser = parser;
        _commandLineHandlers = handlers;
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
