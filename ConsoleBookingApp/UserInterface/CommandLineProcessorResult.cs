using ConsoleBookingApp.CommandHandler;
using System.Text;

namespace ConsoleBookingApp.UserInterface;

public class CommandLineProcessorResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public Action<int>? PostProcess { get; set; }
}

public class EmptyCommandLineProcessorResult : CommandLineProcessorResult
{
    public EmptyCommandLineProcessorResult(string helpCommand)
    {
        Success = false;
        Message = $"Empty command. Try {helpCommand}() to check for existing commands.";
        PostProcess = null;
    }
}

public class NotFoundCommandLineProcessorResult : CommandLineProcessorResult
{
    public NotFoundCommandLineProcessorResult(string command)
    {
        Success = false;
        Message = $"Command '{command}()' not found. Try Help() to check for existing commands.";
        PostProcess = null;
    }
}

public class HelpCommandLineProcessorResult : CommandLineProcessorResult   // TODO add aliases
{
    private readonly Dictionary<string, ICommandHandler> _commandLineHandlers;

    public HelpCommandLineProcessorResult(Dictionary<string, ICommandHandler> commandLineHandler)
    {
        _commandLineHandlers = commandLineHandler;

        Success = true;
        Message = BuildHelpInfo(); //"Available commands: Help(), Exit(), Search(), Availability().";
        PostProcess = null;
    }

    private string BuildHelpInfo()   // add and use aliases - use CommandLineAliasResolver or simply aliases in Dictionary from IOptions - refactor configuration to use Dictionary
    {
        var sb = new StringBuilder();
        sb.AppendLine("Available commands:");

        foreach (var handler in _commandLineHandlers)
            sb.AppendLine($"{handler.Value.CommandName}()");

        return sb.ToString();
    }
}

public class ExitCommandLineProcessorResult : CommandLineProcessorResult
{
    public ExitCommandLineProcessorResult(string command, Action<int>? cloasApp)
    {
        Message = $"{command}() command received. Application is closing...";
        Success = true;
        PostProcess = cloasApp;
    }
}

public class InvalidFormatCommandLineProcessorResult : CommandLineProcessorResult
{
    public InvalidFormatCommandLineProcessorResult(string helpCommand)
    {
        Message = Message = $"Invalid command format. Mind even empty parameter list command has to end with round brackets. Try {helpCommand}() to check for existing commands.";
        Success = false;
        PostProcess = null;
    }
}

