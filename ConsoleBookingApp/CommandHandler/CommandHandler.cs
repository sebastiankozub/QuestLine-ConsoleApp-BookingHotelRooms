// Using CQRS convention most of ICommandHandler implementations are query handlers
// In the ICommandHandler interface command simply stands for user executed command line action
// left as it is for now - refactor when more functionalities mixing query and command handlers will be added

using BookingApp.Service;
using BookingData;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace ConsoleBookingApp.CommandHandler;

public interface ICommandHandler
{
    Task<CommandHandlerResult> HandleAsync(string[] parameters);
    string CommandName { get; }
}


public class AvailabilityCommandHandler(IRoomAvailabilityService roomAvailabilityService) : ICommandHandler
{
    private readonly IRoomAvailabilityService _roomAvailabilityService = roomAvailabilityService;

    public string CommandName => "Availability";

    public async Task<CommandHandlerResult> HandleAsync(string[] parameters)
    {

        //var from = new DateOnly(2000, 10, 1);  // parse from parameters
        //var to = from.AddMonths(2);
        //var roomType = "SGL";
        //var hotelId = "H1";

        var parsedParameters = RoomAvailabilityCommandParse(parameters);
        var validationSuccess = RoomAvailabilityCommandValidate(parsedParameters.hotelId, parsedParameters.availabitlityPeriod, parsedParameters.roomType);

        var roomAvailabilities = await _roomAvailabilityService.GetRoomAvailabilityByType(parsedParameters.hotelId, parsedParameters.availabitlityPeriod, parsedParameters.roomType);

        return new CommandHandlerResult { Success = true, Message = "Availability without exception" };

        //var bookings = _dataContext.Bookings.Where(h => h != null);
        //var hotels = _dataContext.Hotels.Where(h => h != null);

        //var sb = new StringBuilder();

        //if (parameters != null)
        //{
        //    sb.AppendLine("Parameters:");
        //    foreach (var parameter in parameters)
        //    {
        //        sb.AppendLine($"{parameter.Trim()}");
        //    }
        //    sb.AppendLine("");
        //}

        //sb.AppendLine("Hotels:");
        //foreach (var h in hotels)
        //{
        //    sb.AppendLine($"{h.Name}");
        //}

        //sb.AppendLine("Bookings:");
        //foreach (var b in bookings)
        //{
        //    sb.AppendLine($"{b.HotelId} {b.RoomType}");
        //}

        //return new CommandHandlerResult { Success = true, Message = sb.ToString() };
    }

    private (string hotelId, (DateOnly from, DateOnly to) availabitlityPeriod, string roomType) RoomAvailabilityCommandParse(string[] parameters)
    {
        parameters = parameters.Select(p => p.Trim()).ToArray();

        if (parameters.Any(p => string.IsNullOrEmpty(p)))
            throw new ArgumentException();

        if (parameters.Length != 3)
            throw new ArgumentException();

        var hotelId = parameters[0];
        var roomType = parameters[2];

        var date = parameters[1];

        DateOnly from;
        DateOnly to;

        if (date.Length == 8)
        {
            from = to = DateOnly.TryParseExact(date, "yyyyMMdd", out var dateOnly) ? dateOnly : throw new ArgumentException();
        }
        if (date.Length == 17)
        {
            var dateRange = date.Split('-');

            if (dateRange.Length != 2 || dateRange.Any(date => date.Length != 8))
                throw new ArgumentException();

            from = DateOnly.TryParseExact(dateRange[0], "yyyyMMdd", out var dateOnlyFrom) ? dateOnlyFrom : throw new ArgumentException();
            to = DateOnly.TryParseExact(dateRange[1], "yyyyMMdd", out var dateOnlyTo) ? dateOnlyTo : throw new ArgumentException();
        }
        else
            throw new ArgumentException();

        if(from > to)
        {
            var temp = to;
            to = from; 
            from = temp;
        }

        return new (hotelId, (from, to), roomType);
    }

    private bool RoomAvailabilityCommandValidate(string hotelId, (DateOnly from, DateOnly to) availabilityPerdiod, string roomType)
    {
        if (hotelId.Length > 10 || hotelId.Length < 2)
            throw new ArgumentException();

        //if (availabilityPerdiod.from < DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1) || 
        if (availabilityPerdiod.to < availabilityPerdiod.from)
            throw new ArgumentException();

        return false;
    }
}




public class SearchCommandHandler(DataContext dataContext) : ICommandHandler
{
    private readonly DataContext _dataContext = dataContext;

    public string CommandName => "Search";

    public async Task<CommandHandlerResult> HandleAsync(string[] parameters)
    {
        await Task.Delay(333);

        var bookings = _dataContext.Bookings.Where(h => h != null);
        var hotels = _dataContext.Hotels.Where(h => h != null);

        var sb = new StringBuilder();

        if (parameters != null)
        {
            sb.AppendLine("Parameters:");
            foreach (var parameter in parameters)
            {
                sb.AppendLine($"{parameter.Trim()}");
            }
            sb.AppendLine("");
        }

        sb.AppendLine("Hotels:");
        foreach (var h in hotels)
        {
            sb.AppendLine($"{h.Name}");
        }

        sb.AppendLine("Bookings:");
        foreach (var b in bookings)
        {
            sb.AppendLine($"{b.HotelId} {b.RoomType}");
        }

        return new CommandHandlerResult { Success = true, Message = sb.ToString() };
    }
}
