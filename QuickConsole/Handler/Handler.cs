namespace QuickConsole.Handler;

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

    public abstract CommandHandlerResult BuildResultFrom(T internalHandlerResult);

    protected abstract Task<Dictionary<string, IEnumerable<object>>> BuildParametersForHandleInternal(string[] parameters);

    protected abstract Task<T> HandleInternalAsync(Dictionary<string, IEnumerable<object>> parametersDictionary);

    private async Task<IHandlerResult> CatchHandleInternalAsync(string[] parameters)
    {
        try
        {
            var parametersDictionary = await BuildParametersForHandleInternal(parameters);  
            var internalHandlerResult = await HandleInternalAsync(parametersDictionary);
            return BuildResultFrom(internalHandlerResult);
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

    public abstract IHandlerResult BuildResultFrom(T internalHandlerResult);

    protected abstract Task<Dictionary<string, IEnumerable<object>>> BuildParametersForHandleInternal(string[] parameters);

    protected abstract Task<T> HandleInternalAsync(Dictionary<string, IEnumerable<object>> parametersDictionary);

    private async Task<IHandlerResult> CatchHandleInternalAsync(string[] parameters)
    {
        try
        {
            var parametersDictionary = await BuildParametersForHandleInternal(parameters);
            var internalHandlerResult = await HandleInternalAsync(parametersDictionary);
            return BuildResultFrom(internalHandlerResult);
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





public class Parameter
{
    private readonly Type _parameterType;
    private readonly object? _parameterValue;
    private readonly IList<object>? _parameterValueArray = [];

    public Parameter(string commandLineParam)
    {
        // try parse and set Type int string bool i []

        _parameterValue = 5;
        _parameterValueArray = [4, 5, 6];
    }

    public (Type type, object value) GetValue(uint index)
    {
        return (_parameterType, _parameterValue);
    }

    public T CastObject<T>(object input)
    {
        return (T)input;
    }

    public T ConvertObject<T>(object input)
    {
        return (T)Convert.ChangeType(input, typeof(T));
    }

    public dynamic CastFromType(object input, Type type)
    {
        dynamic changedObj = Convert.ChangeType(input, type);
        //changedObj.Meod();
        return changedObj;
    }
}

public class ParameterBuiler
{
    private readonly IList<Parameter> _quickParameters = [];
    public ParameterBuiler(string[] commandLineParams)
    {
        foreach (var param in commandLineParams)
        {
            var quickParam = new Parameter(param);
            _quickParameters.Add(quickParam);
        }
    }

    public IList<Parameter> GetParameters()
    {
        return _quickParameters;
    }
}



