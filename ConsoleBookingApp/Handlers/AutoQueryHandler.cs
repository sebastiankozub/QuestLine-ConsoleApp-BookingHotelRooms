using BookingApp.Service;
using BookingData.Model;
using QuickConsole.Handler;

namespace ConsoleBookingApp.CommandHandler;

public class AutoQueryHandler(IRoomAvailabilityService roomAvailabilityService) : QueryHandler<List<Booking>>("QueryAutoAdded")
{
    private readonly IRoomAvailabilityService _roomAvailabilityService = roomAvailabilityService;

    public override IHandlerResult BuildResultFrom(List<Booking> internalHandlerResulttt)
    {
        return new QueryHandlerResult()
        {
            Success = true,
            ResultData = ["1", "1", "1","4th line"]
        };
    }

    protected async override Task<Dictionary<string, IEnumerable<object>>> BuildParametersForHandleInternal(string[] parameters)
    {
        var parametersDictionary = new Dictionary<string, IEnumerable<object>>();  // IHandler abstract build generic parameters builder
        return await Task.FromResult(parametersDictionary);
    }

    protected async override Task<List<Booking>> HandleInternalAsync(Dictionary<string, IEnumerable<object>> parametersDictionary)
    {


        return await Task.FromResult(new List<Booking>());
    }
}
