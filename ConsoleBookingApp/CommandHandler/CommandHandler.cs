// Using CQRS convention most of ICommandHandler implementations are query handlers
// In the ICommandHandler interface command simply stands for user executed command line action
// left as it is for now - refactor when more functionalities mixing query and command handlers will be added

using BookingApp.Service;
using BookingData;
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
        await Task.Delay(333);

        var from = new DateOnly(0, 0, 0);  // parse from parameters
        var to = DateOnly.MaxValue;
        var roomType = "SGL";

        _roomAvailabilityService.GetRoomAvailabilityByType("", (from, to), roomType);




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
