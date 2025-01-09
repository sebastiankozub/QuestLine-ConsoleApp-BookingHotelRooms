using QuickConsole.OldCommandHandler;
using System.Text;

namespace QuickConsole.Core;

internal class CommandLineProcessorResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public Action<int>? PostProcess { get; set; }
    public string? Result { get; set; }
    public string? ExceptionMessage { get; set; }
}

internal class EmptyCommandLineProcessorResult : CommandLineProcessorResult
{
    public EmptyCommandLineProcessorResult(string helpCommand)
    {
        Success = false;
        ExceptionMessage = $"Empty command. Try {helpCommand}() to check for existing commands.";
        PostProcess = null;
    }
}

internal class NotResolvedCommandLineProcessorResult : CommandLineProcessorResult
{
    public NotResolvedCommandLineProcessorResult(string fullCommand)
    {
        Success = false;
        ExceptionMessage = $"Command ['{fullCommand}'] not found. Try Help() to check for existing commands.";
        PostProcess = null;
    }
}

internal class HelpCommandLineProcessorResult : CommandLineProcessorResult   // TODO add aliases
{
    private readonly Dictionary<string, IOldCommandHandler> _commandLineHandlers;

    public HelpCommandLineProcessorResult(Dictionary<string, IOldCommandHandler> commandLineHandler)
    {
        _commandLineHandlers = commandLineHandler;

        Success = true;
        Result = BuildHelpInfo();
        PostProcess = null;
    }

    private string BuildHelpInfo()   // add and use aliases engine instead of method and wrong design - use CommandLineAliasResolver or simply aliases in Dictionary from IOptions - refactor configuration to use Dictionary
    {
        var sb = new StringBuilder();
        sb.AppendLine("Available commands:");

        foreach (var handler in _commandLineHandlers)
            sb.AppendLine($"{handler.Value.DefaultCommandName}()");

        sb.AppendLine("Help()");
        sb.AppendLine("Exit()");

        return sb.ToString();
    }
}

internal class ExitCommandLineProcessorResult : CommandLineProcessorResult
{
    public ExitCommandLineProcessorResult(string command, Action<int>? closeAppDelegate)
    {
        Message = $"{command}() command received. Application is closing...";
        Success = true;
        PostProcess = closeAppDelegate;
    }
}

internal class InvalidFormatCommandLineProcessorResult : CommandLineProcessorResult
{
    public InvalidFormatCommandLineProcessorResult(string helpCommand)
    {
        ExceptionMessage = $"Invalid command format. Mind even empty parameter list command has to end with round brackets. Try {helpCommand}() to check for existing commands.";
        Success = false;
        PostProcess = null;
    }
}

