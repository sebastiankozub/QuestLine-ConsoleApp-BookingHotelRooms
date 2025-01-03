using ConsoleBookingApp.Data;
using System.Text;

namespace ConsoleBookingApp.UserInterface;

public interface ICommandLineHandler
{
    Task<CommandLineHandlerResult> HandleAsync(string[] parameters);
    string CommandName { get; }
}

public class CommandLineHandlerResult
{
    public bool Success { get; set; }
    public required string Message { get; set; }
    public Action? PostResultAction { get; set; }
}

public class AvailabilityCommandLineHandler(DataContext dataContext) : ICommandLineHandler
{
    private readonly DataContext _dataContext = dataContext;

    public string CommandName => "Availability";

    public async Task<CommandLineHandlerResult> HandleAsync(string[] parameters)
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

        return new CommandLineHandlerResult { Success = true, Message = sb.ToString() };
    }
}


public class SearchCommandLineHandler(DataContext dataContext) : ICommandLineHandler
{
    private readonly DataContext _dataContext = dataContext;

    public string CommandName => "Search";

    public async Task<CommandLineHandlerResult> HandleAsync(string[] parameters)
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

        return new CommandLineHandlerResult { Success = true, Message = sb.ToString() };
    }
}

//public class ExitCommandLineHandler : ICommandLineHandler
//{
//    public string CommandName => "Exit";

//    public async Task<CommandLineHandlerResult> HandleAsync(string[] parameters)
//    {        
//        await Task.Run(() => Environment.Exit(0));
//        return new CommandLineHandlerResult { Success = true, Message = "Exiting application..." };
//    }
//}

