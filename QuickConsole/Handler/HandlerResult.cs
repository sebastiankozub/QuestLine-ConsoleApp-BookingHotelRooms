namespace QuickConsole.Handler;

public interface IHandlerResult
{
    bool Success { get; set; }
    string? Message { get; set; }
    Action? PostResultAction { get; set; }
}

public interface IQueryHandlerResult  : IHandlerResult
{
    string[] ResultData { get; set; }
}

public interface ICommandHandlerResult : IHandlerResult
{
    string ResultData { get; set; }
}

public class CommandHandlerResult : ICommandHandlerResult
{
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
    public required string ResultData { get; set; }
    public Action? PostResultAction { get; set; }
}

public class QueryHandlerResult : IQueryHandlerResult
{
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
    public required string[] ResultData { get; set; }
    public Action? PostResultAction { get; set; }
}

public class ExceptionHandlerResult : IHandlerResult
{
    public bool Success { get; set; } = false;
    public string? Message { get; set; }
    public Action? PostResultAction { get; set; }
    public required string ExceptionMessage { get; set; }
    public required Type ExceptionType { get; set; }
}
