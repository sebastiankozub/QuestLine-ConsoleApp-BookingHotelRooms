// Using CQRS convention most of IOldCommandHandler implementations are query handlers
// In the IOldCommandHandler interface command simply stands for user executed command line action
// it was ICommandLineHandler at the beginning but I decide that CommandLineProcessor is the last step where this command is command line
// left as it is for now - refactor when more functionalities mixing query and command handlers will be added
using System;

namespace ConsoleBookingApp.CommandHandler;

public interface IOldCommandHandler
{
    Task<IOldCommandHandlerResult> HandleAsync(string[] parameters);
    string DefaultCommandName { get; }
}


public abstract class OldCommandHandler(string defaultCommandName) : IOldCommandHandler
{
    public string DefaultCommandName { get { return _defaultCommandName; }}

    protected readonly string _defaultCommandName = defaultCommandName;

    public abstract Task<IOldCommandHandlerResult> HandleAsync(string[] parameters);

    protected static T HandleExceptionMessage<T>(Exception ex) where T : OldCommandHandlerResult, new()
    {
        return  new T
        {
            Success = false,
            ExceptionMessage = ex.Message
        };
    }
}





// new approach  
// servicing Command and Query 
public interface IHandler
{
    string DefaultHandlerName { get; }
    Task<IHandlerResult> HandleAsync(string[] parameters);
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T">Type parameters is model returned by HandleInternalAsync</typeparam>
/// <param name="defaultHandlerName"></param>
public abstract class CommandHandler<T>(string defaultHandlerName) : IHandler where T : struct    //  // not everything  Async
{
    public string DefaultHandlerName { get { return _defaultHandlerName; } }

    protected readonly string _defaultHandlerName = defaultHandlerName;

    public abstract Task<CommandHandlerResult> BuildResultFrom(T internalHandlerResult);

    protected abstract Task<Dictionary<string, IEnumerable<object>>> BuildParametersForHandleInternal(string[] parameters);

    protected abstract Task<T> HandleInternalAsync(Dictionary<string, IEnumerable<object>> parametersDictionary);

    private async Task<IHandlerResult> CatchHandleInternalAsync(string[] parameters)
    {
        try
        {
            var parametersDictionary = await BuildParametersForHandleInternal(parameters);  
            var internalHandlerResult = await HandleInternalAsync(parametersDictionary);
            return await BuildResultFrom(internalHandlerResult);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(new ExceptionHandlerResult()
            {
                Success = false,
                ExceptionMessage = ex.Message,
                ExceptionType = ex.GetType(),
            });
        }
    }

    public async Task<IHandlerResult> HandleAsync(string[] parameters)
    {
        return await CatchHandleInternalAsync(parameters);
    }
}

public abstract class QueryHandler<T>(string defaultHandlerName) : IHandler where T : notnull
{
    public string DefaultHandlerName { get { return _defaultHandlerName; } }

    protected readonly string _defaultHandlerName = defaultHandlerName;

    public abstract Task<IHandlerResult> BuildResultFrom(T internalHandlerResult);

    protected abstract Task<Dictionary<string, IEnumerable<object>>> BuildParametersForHandleInternal(string[] parameters);

    protected abstract Task<T> HandleInternalAsync(Dictionary<string, IEnumerable<object>> parametersDictionary);

    private async Task<IHandlerResult> CatchHandleInternalAsync(string[] parameters)
    {
        try
        {
            var parametersDictionary = await BuildParametersForHandleInternal(parameters);
            var internalHandlerResult = await HandleInternalAsync(parametersDictionary);
            return await BuildResultFrom(internalHandlerResult);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(new ExceptionHandlerResult() { 
                Success = false,
                ExceptionMessage = ex.Message,
                ExceptionType = ex.GetType(), 
            });
        }
    }

    public async Task<IHandlerResult> HandleAsync(string[] parameters)
    {
        return await CatchHandleInternalAsync(parameters);
    }

    

}


