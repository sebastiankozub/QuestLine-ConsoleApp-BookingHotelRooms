namespace ConsoleBookingApp.CommandHandler;

public class CommandHandlerResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? ResultData { get; set; }
    public Action? PostResultAction { get; set; }
}

public class AvailabilityCommandHandlerResult : CommandHandlerResult
{

}

public class SearchCommandHandlerResult : CommandHandlerResult
{

}