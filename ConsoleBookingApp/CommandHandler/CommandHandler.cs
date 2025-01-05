// Using CQRS convention most of ICommandHandler implementations are query handlers
// In the ICommandHandler interface command simply stands for user executed command line action
// it was ICommandLineHandler at the beginning but I decide that CommandLineProcessor is the last step where this command is command line
// left as it is for now - refactor when more functionalities mixing query and command handlers will be added
namespace ConsoleBookingApp.CommandHandler;

public interface ICommandHandler
{
    Task<ICommandHandlerResult> HandleAsync(string[] parameters);
    string DefaultCommandName { get; }
}

public abstract class CommandHandler(string defaultCommandName) : ICommandHandler
{
    public string DefaultCommandName { get { return _defaultCommandName; }}

    protected readonly string _defaultCommandName = defaultCommandName;

    public abstract Task<ICommandHandlerResult> HandleAsync(string[] parameters);

    protected T HandleExceptionMessage<T>(Exception ex) where T : CommandHandlerResult, new()
    {
        return  new T
        {
            Success = false,
            Message = $"Executing user command [{DefaultCommandName}] finieshed with error.",
            ExceptionMessage = ex.Message
        };
    }
}

