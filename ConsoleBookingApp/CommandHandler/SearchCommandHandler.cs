using BookingApp.Service;
using System.Text;

namespace ConsoleBookingApp.CommandHandler;

public class SearchCommandHandler(IRoomAvailabilityService roomAvailabilityService) : CommandHandler("Search")
{
    private readonly IRoomAvailabilityService _roomAvailabilityService = roomAvailabilityService;

    public async override Task<CommandHandlerResult> HandleAsync(string[] parameters)
    {
        var parsedParameters = SearchCommandParse(parameters);
        SearchCommandValidate(parsedParameters.hotelId, parsedParameters.availabitlityPeriod, parsedParameters.roomType);

        try
        { 
            var roomAvailabilities = await _roomAvailabilityService
                .GetRoomAvailabilityByRoomType(parsedParameters.hotelId, parsedParameters.availabitlityPeriod, parsedParameters.roomType);

            var outputBuilder = new StringBuilder();
            foreach (var roomAvailabitily in roomAvailabilities)
            {
                if (roomAvailabitily.AvailabilityCount > 0)
                {
                    outputBuilder.Append(
                    "(" + $"{roomAvailabitily.Day.ToString("yyyyMMdd")}" + "," + $"{roomAvailabitily.AvailabilityCount}" + "),");
                }
                else
                {
                    outputBuilder.AppendLine("");
                }
            }               

            return new SearchCommandHandlerResult { Success = true, ResultData = outputBuilder.ToString() };
        }
        catch(RoomAvailabilityServiceException ex)
        {
            return new CommandHandlerResult
            {
                Success = false,
                Message = $"Executing user command [{DefaultCommandName}] finieshed with error." + Environment.NewLine +
                $"Error message: {ex.Message}"
            };
        }
    }

    private static bool SearchCommandValidate(string hotelId, (DateOnly from, DateOnly to) availabilityPerdiod, object roomType)
    {
        if (hotelId.Length > 10 || hotelId.Length < 2)
            throw new SearchCommandHandlerValidateException();

        if (availabilityPerdiod.to < availabilityPerdiod.from)
            throw new SearchCommandHandlerValidateException();

        if (availabilityPerdiod.from < GetTommorowUtcDateOnly())
            throw new SearchCommandHandlerValidateException();

        return true;
    }

    private static (string hotelId, (DateOnly from, DateOnly to) availabitlityPeriod, string roomType) SearchCommandParse(string[] parameters)
    {
        parameters = parameters.Select(p => p.Trim()).ToArray();

        if (parameters.Any(p => string.IsNullOrEmpty(p)))
            throw new SearchCommandHandlerParseException();

        if (parameters.Length != 3)
            throw new SearchCommandHandlerParseException();

        var hotelId = parameters[0];
        var roomType = parameters[2];

        var days = parameters[1];

        if (days.Length <= 0)        
            throw new SearchCommandHandlerParseException();
 
        if (days.Length >= 4)
            throw new SearchCommandHandlerParseException();

        var numberOfDaysAhead = int.TryParse(days, out var n) ? n : throw new SearchCommandHandlerParseException();

        DateOnly from = GetTommorowUtcDateOnly();
        DateOnly to = from.AddDays(numberOfDaysAhead - 1);

        if (from > to)
            (from, to) = (to, from);

        return new(hotelId, (from, to), roomType);
    }

    private static DateOnly GetTommorowUtcDateOnly()
    {
        return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
    }
}


public class SearchCommandHandlerParseException : Exception
{
    public SearchCommandHandlerParseException()
        : base(nameof(SearchCommandHandlerParseException))
    {
    }

    public SearchCommandHandlerParseException(string? message)
          : base(message == null ? nameof(SearchCommandHandlerParseException) : message)
    {


    }
}

public class SearchCommandHandlerValidateException : Exception
{
    public SearchCommandHandlerValidateException(string? message)
        : base(message == null ? nameof(SearchCommandHandlerValidateException) : message)
    {


    }
    public SearchCommandHandlerValidateException()
    : base(nameof(SearchCommandHandlerValidateException))
    {
    }
}


