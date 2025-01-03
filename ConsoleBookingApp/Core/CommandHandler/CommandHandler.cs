// Using CQRS convention most of ICommandHandler implementations are not command but query handlers
// In the ICommandHandler interface command simply stands for user executed action
// left as it is for now - refactor when more functionalities mixing query and command handlers will be added

using ConsoleBookingApp.Data;
using System.Text;

namespace ConsoleBookingApp.Core.CommandHandler;

public interface ICommandHandler
{
    Task<CommandHandlerResult> HandleAsync(string[] parameters);
    string CommandName { get; }
}



public class AvailabilityCommandHandler(DataContext dataContext) : ICommandHandler
{
    private readonly DataContext _dataContext = dataContext;

    public string CommandName => "Availability";

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

//public class ExitCommandLineHandler : ICommandHandler
//{
//    public string CommandName => "Exit";

//    public async Task<CommandLineHandlerResult> HandleAsync(string[] parameters)
//    {        
//        await Task.Run(() => Environment.Exit(0));
//        return new CommandLineHandlerResult { Success = true, Message = "Exiting application..." };
//    }
//}

