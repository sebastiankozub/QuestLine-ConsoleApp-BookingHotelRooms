using BookingApp.Service;
using System.Text;

namespace ConsoleBookingApp.CommandHandler;

public class AvailabilityCommandHandler(IRoomAvailabilityService roomAvailabilityService) : CommandHandler("Availability")
{
    private readonly IRoomAvailabilityService _roomAvailabilityService = roomAvailabilityService;

    public override async Task<ICommandHandlerResult> HandleAsync(string[] parameters)
    {
        try
        {
            var (hotelId, availabitlityPeriod, roomType) = AvailabilityCommandParse(parameters);
            AvailabilityCommandValidate(hotelId, availabitlityPeriod, roomType);

            var roomAvailabilities = await _roomAvailabilityService
                .GetRoomAvailabilityByRoomType(hotelId, availabitlityPeriod, roomType);

            var outputBuilder = new StringBuilder();
            foreach (var roomAvailabitily in roomAvailabilities)
                outputBuilder.AppendLine(roomAvailabitily.Day.ToString("yyyyMMdd") + " - " + roomAvailabitily.RoomAvailabilityCount);

            return new AvailabilityCommandHandlerResult { Success = true, ResultData = outputBuilder.ToString() };
        }
        catch (AvailabilityCommandHandlerParseException ex)
        {
            return new AvailabilityCommandHandlerResult
            {
                Success = false,
                Message = $"Executing user command [{DefaultCommandName}] finieshed with error." + Environment.NewLine +
                $"Error message: {ex.Message}"
            };
        }
        catch (AvailabilityCommandHandlerValidateException ex)
        {
            return new AvailabilityCommandHandlerResult
            {
                Success = false,
                Message = $"Executing user command [{DefaultCommandName}] finieshed with error." + Environment.NewLine +
                $"Error message: {ex.Message}"
            };
        }
        catch (RoomAvailabilityServiceException ex)
        {
            return new AvailabilityCommandHandlerResult
            {
                Success = false,
                Message = $"Executing user command [{DefaultCommandName}] finieshed with error." + Environment.NewLine +
                $"Error message: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            return new AvailabilityCommandHandlerResult
            {
                Success = false,
                Message = $"Executing user command [{DefaultCommandName}] finieshed with error." + Environment.NewLine +
                            $"Error message: {ex.Message}"
            };
        }
    }

    private static (string hotelId, (DateOnly from, DateOnly to) availabitlityPeriod, string roomType) AvailabilityCommandParse(string[] parameters)
    {
        parameters = parameters.Select(p => p.Trim()).ToArray();

        if (parameters.Any(p => string.IsNullOrEmpty(p)))
            throw new AvailabilityCommandHandlerParseException();

        if (parameters.Length != 3)
            throw new AvailabilityCommandHandlerParseException();

        var hotelId = parameters[0];
        var roomType = parameters[2];

        var date = parameters[1];

        DateOnly from;
        DateOnly to;

        if (date.Length == 8)
        {
            from = to = DateOnly.TryParseExact(date, "yyyyMMdd", out var dateOnly) ? dateOnly : throw new AvailabilityCommandHandlerParseException();
        }
        if (date.Length >= 17)
        {
            var dateRange = date.Split('-').Select(x => x.Trim()).ToArray();

            if (dateRange.Length != 2 || dateRange.Any(date => date.Length != 8))
                throw new AvailabilityCommandHandlerParseException();

            from = DateOnly.TryParseExact(dateRange[0], "yyyyMMdd", out var dateOnlyFrom) ? dateOnlyFrom : throw new AvailabilityCommandHandlerParseException();
            to = DateOnly.TryParseExact(dateRange[1], "yyyyMMdd", out var dateOnlyTo) ? dateOnlyTo : throw new AvailabilityCommandHandlerParseException();
        }
        else
            throw new AvailabilityCommandHandlerParseException();

        if (from > to)        
            (from, to) = (to, from);        

        return new(hotelId, (from, to), roomType);
    }

    private static bool AvailabilityCommandValidate(string hotelId, (DateOnly from, DateOnly to) availabilityPerdiod, string roomType)
    {
        if (hotelId.Length > 10 || hotelId.Length < 2)
            throw new AvailabilityCommandHandlerValidateException();

        if (roomType.Length > 10 || roomType.Length < 2)
            throw new AvailabilityCommandHandlerValidateException();

        //if (availabilityPerdiod.from < DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1) || let search for historical data
        if (availabilityPerdiod.to < availabilityPerdiod.from)
            throw new AvailabilityCommandHandlerValidateException();

        return true;
    }
}

public class AvailabilityCommandHandlerParseException : Exception
{
    public AvailabilityCommandHandlerParseException()
        : base(nameof(AvailabilityCommandHandlerParseException)) {}

    public AvailabilityCommandHandlerParseException(string? message)
          : base(message ?? nameof(AvailabilityCommandHandlerParseException)) {}
}

public class AvailabilityCommandHandlerValidateException : Exception
{
    public AvailabilityCommandHandlerValidateException(string? message)
        : base(message ?? nameof(AvailabilityCommandHandlerValidateException)) {}

    public AvailabilityCommandHandlerValidateException()
    : base(nameof(AvailabilityCommandHandlerValidateException)) {}
}
