using BookingApp.Service;
using BookingData.Model;

namespace ConsoleBookingApp.CommandHandler;

public class AutoQueryHandler(IRoomAvailabilityService roomAvailabilityService) : QueryHandler<List<Booking>>("QueryAutoAdded")
{
    private readonly IRoomAvailabilityService _roomAvailabilityService = roomAvailabilityService;

    public async override Task<IHandlerResult> BuildResultFrom(List<Booking> internalHandlerResulttt)
    {
        return await Task.FromResult(new QueryHandlerResult()
        {
            Success = true,
            ResultData = ["1", "1", "1"]
        });       
    }

    protected async override Task<Dictionary<string, IEnumerable<object>>> BuildParametersForHandleInternal(string[] parameters)
    {
        var parametersDictionary = new Dictionary<string, IEnumerable<object>>();
        return await Task.FromResult(parametersDictionary);
    }

    protected async override Task<List<Booking>> HandleInternalAsync(Dictionary<string, IEnumerable<object>> parametersDictionary)
    {

        return await Task.FromResult(new List<Booking>());
    }
}