namespace ConsoleBookingApp.CommandHandler;

public class OldCommandHandlerResult : IOldCommandHandlerResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? ExceptionMessage { get; set; }
    public string? ResultData { get; set; }    // TODO for future possibilities of CommmandLineProcessor, ConsoleAppInterface changes string? to []string
    public Action? PostResultAction { get; set; }
}

public class AvailabilityCommandHandlerResult : OldCommandHandlerResult
{ }

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



// NEW APPROACH 
public interface IHandlerInternalResult
{
}

// Command Internal
public interface ICommandHandlerInternalResult : IHandlerInternalResult
{
}

public class CommandHandlerInternalResult<T> : ICommandHandlerInternalResult 
{
    public T? Result { get; set; } = default(T); 

    public Type GetResultType()
    {
        return typeof(T);
    }

    public bool Success { get; set; }

    public Exception? Exception { get; set; }
}

// Query Internal

public interface IQueryHandlerInternalResult : IHandlerInternalResult
{
}

public class QueryHandlerInternalResult<T> : IQueryHandlerInternalResult
{
    public T? Result { get; set; } = default(T);

    public Type GetResultType()
    {
        return typeof(T);
    }

    public bool Success { get; set; }

    public Exception? Exception { get; set; }
}


// HandleAsync results 

public interface IHandlerResult    
{
    bool Success { get; set; }
    string? Message { get; set; }
    string? ExceptionMessage { get; set; }
    Action? PostResultAction { get; set; }
}

public interface IQueryHandlerResult  : IHandlerResult 
{
    string[]? ResultData { get; set; }
}

public interface ICommandHandlerResult : IHandlerResult
{
    string? ResultData { get; set; }
}

public class CommandHandlerResult : ICommandHandlerResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? ExceptionMessage { get; set; }
    public Type? ExceptionType { get; set; }
    public string? ResultData { get; set; } 
    public Action? PostResultAction { get; set; }
}

public class QueryHandlerResult : IQueryHandlerResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? ExceptionMessage { get; set; }
    public Type? ExceptionType { get; set; }
    public string[]? ResultData { get; set; }   // many rows of items or one row entity
    public Action? PostResultAction { get; set; }
}