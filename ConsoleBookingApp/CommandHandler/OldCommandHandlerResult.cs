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
//public interface IQueryIHandlerInternalResult : IHandlerInternalResult
//{ }


//public interface IEnumerableQueryHandlerInternalResult<ET> : IQueryIHandlerInternalResult
//{
//    IEnumerable<ET> Result { get; set; }
//}

//public class EnumerableCommandHandlerInternalResult<ET> : IEnumerableQueryHandlerInternalResult<ET> where ET : IEnumerable<ET>
//{
//    public IEnumerable<ET> Result { get; set; }

//    public Type GetResultType()
//    {
//        throw new NotImplementedException();
//    }
//}
//{
//    public bool Result { get; set; }

//    public bool GetResultType()
//    {
//        throw new NotImplementedException();
//    }
//}

//public interface IReferenceTypeQueryHandlerInternalResult<RT> : IHandlerInternalResult<Type> where RT : class
//{
//    RT Result { get; set; }
//}

//public class CommandHandlerInternalResult : ICommandHandlerInternalResult
//{
//    public bool Result { get; set; }

//    public bool GetResultType()
//    {
//        throw new NotImplementedException();
//    }
//}

//public interface IValueTypeQueryHandlerInternalResult<VT> : IHandlerInternalResult<Type> where VT : struct
//{
//    VT Result { get; set; }
//}

//public class ValueTypeCommandHandlerInternalResult<T> : IValueTypeQueryHandlerInternalResult
//{
//    public bool Result { get; set; }

//    public bool GetResultType()
//    {
//        throw new NotImplementedException();
//    }
//}


//public abstract class QueryHandledInternalResult : IQueryHandlerInternalResult<IQueryHandlerResult>
//{
//    public abstract IQueryHandlerResult Result { get; set; }
//}

//public abstract class CommandHandledInternalResult : ICommandHandlerInternalResult<ICommandHandlerResult>
//{
//    public abstract ICommandHandlerResult Result { get; set; }
//}

////  interfaces used by query handler internal
////   returning a list of objects or values
//public interface IHandlerInternalEnumerableResult : IHandlerInternalResult<LE> where LE : IHandlerInternalEnumerableResultItem
//{
//    IHandlerInternalEnumerableResultItem Result { get; set; }
//}
//public interface IHandlerInternalEnumerableResultItem
//{
//}
//public interface IHandlerInternalEnumerableResultObject<O> : IHandlerInternalEnumerableResultItem  // when list of entites
//{
//}
//public interface IHandlerInternalEnumerableResultValue : IHandlerInternalEnumerableResultItem  // when list of values
//{
//}
////   returning single class object
//public interface IHandlerInternalObjectResult<R> : IHandlerInternalResult where R : class
//{
//    R Result { get; set; }
//}
////   returing signle value: string, int, bool, etc.
//public interface IHandlerInternalValueResult<V> : IHandlerInternalResult where V : struct
//{
//    V Result { get; set; }
//}


//// the only one used by command handler internal
////  returing success/failure boolean
//interface ICommandHandlerInternalSuccessFailureResult : ICommandHandledInternalResult
//{
//    bool Result { get; set; }
//}















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
    string[] ResultData { get; set; }
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
    public string[]? ResultData { get; set; }   // many rows of items or one row entity
    public Action? PostResultAction { get; set; }
}