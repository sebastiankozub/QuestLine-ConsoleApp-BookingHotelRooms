using BookingApp.Service;

namespace ConsoleBookingApp.CommandHandler;

public class TestCommandHandler(IRoomAvailabilityService roomAvailabilityService) : CommandHandler<bool>("TestCommand")
{
    private readonly IRoomAvailabilityService _roomAvailabilityService = roomAvailabilityService;

    public async override Task<CommandHandlerResult> ResolveCommandHandlerInternalResult(object internalHandlerResult, Exception? internalHandlerException = null)
    {
        if (internalHandlerResult is not null && internalHandlerException is null)
        {
            return await Task.FromResult(new CommandHandlerResult { Success = (bool)internalHandlerResult, ResultData = "1" });
        }
        else
        {
            return await Task.FromResult(new CommandHandlerResult { Success = false, ExceptionType = internalHandlerException?.GetType(), ExceptionMessage = internalHandlerException?.Message  });
        }
    }

    protected async override Task<Dictionary<string, IEnumerable<object>>> BuildParametersForHandleInternalAsync(string[] parameters)
    {
        var parametersDictionary = new Dictionary<string, IEnumerable<object>>();
        return await Task.FromResult(parametersDictionary);
    }

    protected async override Task<bool> HandleInternalAsync(Dictionary<string, IEnumerable<object>> parametersDictionary)
    {
        // core logic to get data from underlying service

        return await Task.FromResult(true);
    }
}
