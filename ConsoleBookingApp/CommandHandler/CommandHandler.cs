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

