namespace ConsoleBookingApp.CommandHandler;

public class CommandHandlerResult : ICommandHandlerResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? ExceptionMessage { get; set; }
    public string? ResultData { get; set; }
    public Action? PostResultAction { get; set; }
}

public class AvailabilityCommandHandlerResult : CommandHandlerResult
{ }

public class SearchCommandHandlerResult : CommandHandlerResult
{}

public interface ICommandHandlerResult
{
    bool Success { get; set; }
    string? Message { get; set; }
    public string? ExceptionMessage { get; set; }
    string? ResultData { get; set; }
    Action? PostResultAction { get; set; }
}