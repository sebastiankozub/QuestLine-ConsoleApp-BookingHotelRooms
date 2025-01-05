using BookingApp.Service;
using System.Text;

namespace ConsoleBookingApp.CommandHandler;

public class SearchCommandHandler(IRoomAvailabilityService roomAvailabilityService) : CommandHandler("Search")
{
    private readonly IRoomAvailabilityService _roomAvailabilityService = roomAvailabilityService;

    public async override Task<CommandHandlerResult> HandleAsync(string[] parameters)
    {
        try
        {
            var (hotelId, availabitlityPeriod, roomType) = SearchCommandParse(parameters);
            SearchCommandValidate(hotelId, availabitlityPeriod, roomType);

            var roomAvailabilities = await _roomAvailabilityService
                .GetRoomAvailabilityByRoomType(hotelId, availabitlityPeriod, roomType, true);

            var outputBuilder = new StringBuilder();
            var roomAvailabilitiesCount = roomAvailabilities.Count();
            var emptyLineAdded = false;

            for (int i = 0; i < roomAvailabilitiesCount;  )
            {
                var roomAvailability = roomAvailabilities.ElementAt(i);

                if (roomAvailability.RoomAvailabilityCount > 0)
                {
                    if (roomAvailability.SameCountPeriod == 1)
                    {
                        outputBuilder.AppendLine(
                            "(" + $"{roomAvailability.Day:yyyyMMdd}" + ","
                            + $"{roomAvailability.RoomAvailabilityCount}" + "),");
                        i += 1;
                    }
                    else
                    {
                        outputBuilder.AppendLine(
                            "(" + $"{roomAvailability.Day:yyyyMMdd} - {roomAvailability.Day.AddDays((int)roomAvailability.SameCountPeriod - 1):yyyyMMdd}"
                            + "," + $"{roomAvailability.RoomAvailabilityCount}" + "),");
                        i += (int)roomAvailability.SameCountPeriod;
                    }
                    emptyLineAdded = false;
                }
                else 
                { 
                    if (!emptyLineAdded) 
                    { 
                        outputBuilder.AppendLine("");
                        emptyLineAdded = true; 
                    }
                    i += 1;
                }
            }

            return new SearchCommandHandlerResult { Success = true, ResultData = outputBuilder.ToString() };
        }
        catch (SearchCommandHandlerParseException ex)
        {
            return new CommandHandlerResult
            {
                Success = false,
                Message = $"Executing user command [{DefaultCommandName}] finieshed with error." + Environment.NewLine +
                $"Error message: {ex.Message}"
            };
        }
        catch (SearchCommandHandlerValidateException ex)
        {
            return new CommandHandlerResult
            {
                Success = false,
                Message = $"Executing user command [{DefaultCommandName}] finieshed with error." + Environment.NewLine +
                $"Error message: {ex.Message}"
            };
        }
        catch (RoomAvailabilityServiceException ex)
        {
            return new CommandHandlerResult
            {
                Success = false,
                Message = $"Executing user command [{DefaultCommandName}] finieshed with error." + Environment.NewLine +
                            $"Error message: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            return new CommandHandlerResult
            {
                Success = false,
                Message = $"Executing user command [{DefaultCommandName}] finieshed with error." + Environment.NewLine +
                            $"Error message: {ex.Message}"
            };
        }
    }

    private static bool SearchCommandValidate(string hotelId, (DateOnly from, DateOnly to) availabilityPerdiod, string roomType)
    {
        if (hotelId.Length > 10 || hotelId.Length < 2)
            throw new SearchCommandHandlerValidateException();

        if (roomType.Length > 10 || roomType.Length < 2)
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

        if (string.IsNullOrEmpty(days))  
            throw new SearchCommandHandlerParseException();
 
        if (days.Length >= 5)
            throw new SearchCommandHandlerParseException();

        var numberOfDaysAhead = int.TryParse(days, out var n) ? n : throw new SearchCommandHandlerParseException();

        if (numberOfDaysAhead < 1)
            throw new SearchCommandHandlerParseException("You cannot search for a room availability in the past. Use positive number to number of days ahead.");

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
    public SearchCommandHandlerParseException() : base(nameof(SearchCommandHandlerParseException)) {}

    public SearchCommandHandlerParseException(string? message)
          : base(message ?? nameof(SearchCommandHandlerParseException)) {}
}

public class SearchCommandHandlerValidateException : Exception
{
    public SearchCommandHandlerValidateException(string? message)
        : base(message ?? nameof(SearchCommandHandlerValidateException)) {}

    public SearchCommandHandlerValidateException()
    : base(nameof(SearchCommandHandlerValidateException)) {}
}