using BookingApp.Service;

namespace ConsoleBookingApp.CommandHandler;

public class TestCommandHandler(IRoomAvailabilityService roomAvailabilityService) : CommandHandler<bool>("TestCommand")
{
    private readonly IRoomAvailabilityService _roomAvailabilityService = roomAvailabilityService;

    public async override Task<CommandHandlerResult> BuildResultFrom(bool internalHandlerResult)
    {
        return await Task.FromResult(new CommandHandlerResult()
        {
            Success = true,
            ResultData = (internalHandlerResult).ToString()
        });
    }

    protected async override Task<Dictionary<string, IEnumerable<object>>> BuildParametersForHandleInternal(string[] parameters)
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
