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
public interface IHandler<IHandlerResult>
{
    string DefaultHandlerName { get; }
    Task<IHandlerResult> HandleAsync(string[] parameters);
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T">Type parameters is model returned by HandleInternalAsync</typeparam>
/// <param name="defaultHandlerName"></param>
public abstract class CommandHandler<T>(string defaultHandlerName) : IHandler<CommandHandlerResult> where T : class?
{
    public string DefaultHandlerName { get { return _defaultHandlerName; } }

    protected readonly string _defaultHandlerName = defaultHandlerName;

    public async virtual Task<CommandHandlerResult> ResolveCommandHandlerInternalResult(CommandHandlerInternalResult<T> internalHandlerResult)
    {
        var result = new CommandHandlerResult()
        {
            ExceptionType = internalHandlerResult.Exception?.GetType(), 
            ExceptionMessage = internalHandlerResult.Exception?.Message,
            ResultData = internalHandlerResult.Result?.ToString(),    // override ToString? for the type
            Success = internalHandlerResult.Success,
            Message = "Additional info",
            PostResultAction = null
        };
        return await Task.FromResult<CommandHandlerResult>(result);
    }

    protected abstract Task<Dictionary<string, IEnumerable<object>>> BuildParametersForHandleInternalAsync(string[] parameters);

    protected abstract Task<CommandHandlerInternalResult<T>> HandleInternalAsync(Dictionary<string, IEnumerable<object>> parametersDictionary);

    private async Task<CommandHandlerResult> CatchHandleInternalAsync(string[] parameters)
    {
        Exception? exc = null;
        CommandHandlerInternalResult<T>? internalHandlerResult = null;

        try
        {
            var parametersDictionary = await BuildParametersForHandleInternalAsync(parameters);
            internalHandlerResult = await HandleInternalAsync(parametersDictionary);            
        }
        catch (Exception exception)
        {
            exc = exception;
        }

        if (internalHandlerResult is null)
            return new CommandHandlerResult
            {
                Success = false,
                ExceptionMessage = exc?.Message
            };
        else
            return await ResolveCommandHandlerInternalResult(internalHandlerResult);
    }
    
    public async Task<CommandHandlerResult> HandleAsync(string[] parameters)
    {
        return await CatchHandleInternalAsync(parameters);
    }
}


public abstract class QueryHandler<T>(string defaultHandlerName) : IHandler<QueryHandlerResult> where T : class?
{
    public string DefaultHandlerName { get { return _defaultHandlerName; } }

    protected readonly string _defaultHandlerName = defaultHandlerName;

    public async virtual Task<QueryHandlerResult> ResolveCommandHandlerInternalResult(QueryHandlerInternalResult<T> internalHandlerResult)
    {
        var result = new QueryHandlerResult()
        {
            ExceptionType = internalHandlerResult.Exception?.GetType(),
            ExceptionMessage = internalHandlerResult.Exception?.Message,
            //ResultData = internalHandlerResult.Result?.ToString(),    // here for Query more complicated logic then command
            Success = internalHandlerResult.Success,
            Message = "Additional info",
            PostResultAction = null
        };
        return await Task.FromResult<QueryHandlerResult>(result);
    }

    protected abstract Task<Dictionary<string, IEnumerable<object>>> BuildParametersForHandleInternalAsync(string[] parameters);

    protected abstract Task<QueryHandlerInternalResult<T>> HandleInternalAsync(Dictionary<string, IEnumerable<object>> parametersDictionary);

    private async Task<QueryHandlerResult> CatchHandleInternalAsync(string[] parameters)
    {
        Exception? exc = null;
        QueryHandlerInternalResult<T>? internalHandlerResult = null;

        try
        {
            var parametersDictionary = await BuildParametersForHandleInternalAsync(parameters);
            internalHandlerResult = await HandleInternalAsync(parametersDictionary);
        }
        catch (Exception exception)
        {
            exc = exception;
        }

        if (internalHandlerResult is null)
            return new QueryHandlerResult
            {
                Success = false,
                ExceptionMessage = exc?.Message
            };
        else
            return await ResolveCommandHandlerInternalResult(internalHandlerResult);
    }

    public async Task<QueryHandlerResult> HandleAsync(string[] parameters)
    {
        return await CatchHandleInternalAsync(parameters);
    }

    public abstract Task<T> HandleInternalAsync(string[] parameters);
}


