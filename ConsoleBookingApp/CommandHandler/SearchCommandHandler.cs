using BookingApp.Service;
using QuickConsole.OldCommandHandler;
using System.Text;

namespace ConsoleBookingApp.CommandHandler;

public class SearchCommandHandler(IRoomAvailabilityService roomAvailabilityService) : OldCommandHandler("Search")
{
    private readonly IRoomAvailabilityService _roomAvailabilityService = roomAvailabilityService;

    public async override Task<IOldCommandHandlerResult> HandleAsync(string[] parameters)
    {
        try
        {
            var (hotelId, availabitlityPeriod, roomType) = SearchCommandValidator.Parse(parameters);
            SearchCommandValidator.Validate(hotelId, availabitlityPeriod, roomType);

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
        catch (Exception ex) when (ex is SearchCommandHandlerParseException 
            || ex is SearchCommandHandlerValidateException 
            || ex is RoomAvailabilityServiceException)
        {
            return HandleExceptionMessage<SearchCommandHandlerResult>(ex);
        }
    }
}

public class SearchCommandHandlerParseException : Exception
{
    public SearchCommandHandlerParseException() 
        : base(nameof(SearchCommandHandlerParseException)) {}

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