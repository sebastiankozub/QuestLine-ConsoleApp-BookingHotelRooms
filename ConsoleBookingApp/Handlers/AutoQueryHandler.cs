using BookingApp.Service;
using BookingData.Model;

namespace ConsoleBookingApp.CommandHandler;

//public class AutoQueryHandler(IRoomAvailabilityService roomAvailabilityService) : QueryHandler<QueryHandlerInternalResult<List<Booking>>>("TestIfAutoAdded")
//{
//    private readonly IRoomAvailabilityService _roomAvailabilityService = roomAvailabilityService;


//    public override Task<QueryHandlerInternalResult<List<Booking>>> HandleInternalAsync(string[] parameters)
//    {
//        throw new NotImplementedException();
//    }

//    protected override Task<Dictionary<string, IEnumerable<object>>> BuildParametersForHandleInternalAsync(string[] parameters)
//    {
//        throw new NotImplementedException();
//    }

//    protected override Task<QueryHandlerInternalResult<QueryHandlerInternalResult<List<Booking>>>> HandleInternalAsync(Dictionary<string, IEnumerable<object>> parametersDictionary)
//    {
//        throw new NotImplementedException();
//    }
//}