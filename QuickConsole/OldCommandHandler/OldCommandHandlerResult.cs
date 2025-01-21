namespace QuickConsole.OldCommandHandler;

public class OldCommandHandlerResult : IOldCommandHandlerResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? ExceptionMessage { get; set; }
    public string? ResultData { get; set; }    // TODO for future possibilities of CommmandLineProcessor, ConsoleAppInterface changes string? to []string
    public Action? PostResultAction { get; set; }
}

public class AvailabilityCommandHandlerResult : OldCommandHandlerResult
{}

public class SearchCommandHandlerResult : OldCommandHandlerResult
{}

public interface IOldCommandHandlerResult
{
    bool Success { get; set; }
    string? Message { get; set; }
    public string? ExceptionMessage { get; set; }
    string? ResultData { get; set; }  
    Action? PostResultAction { get; set; }
}
