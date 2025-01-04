// Using CQRS convention most of ICommandHandler implementations are query handlers
// In the ICommandHandler interface command simply stands for user executed command line action
// left as it is for now - refactor when more functionalities mixing query and command handlers will be added

using BookingData;

namespace ConsoleBookingApp.CommandHandler;

public interface ICommandHandler
{
    Task<CommandHandlerResult> HandleAsync(string[] parameters);
    string DefaultCommandName { get; }
}

public abstract class CommandHandler : ICommandHandler
{
    public CommandHandler(string defaultCommandName)
    { 
        _defaultCommandName = defaultCommandName;  
    }

    public string DefaultCommandName { get { return _defaultCommandName; }}

    protected readonly string _defaultCommandName;

    public abstract Task<CommandHandlerResult> HandleAsync(string[] parameters);
}

