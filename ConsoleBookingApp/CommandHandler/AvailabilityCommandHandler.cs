using BookingApp.Service;
using System.Text;

namespace ConsoleBookingApp.CommandHandler;

public class AvailabilityCommandHandler(IRoomAvailabilityService roomAvailabilityService) : OldCommandHandler("Availability")
{
    private readonly IRoomAvailabilityService _roomAvailabilityService = roomAvailabilityService;

    public override async Task<IOldCommandHandlerResult> HandleAsync(string[] parameters)
    {
        try
        {
            var (hotelId, availabitlityPeriod, roomType) = AvailabilityCommandValidator.Parse(parameters);
            AvailabilityCommandValidator.Validate(hotelId, availabitlityPeriod, roomType);

            var roomAvailabilities = await _roomAvailabilityService
                .GetRoomAvailabilityByRoomType(hotelId, availabitlityPeriod, roomType);

            var outputBuilder = new StringBuilder();
            foreach (var roomAvailabitily in roomAvailabilities)
                outputBuilder.AppendLine(roomAvailabitily.Day.ToString("yyyyMMdd") + " - " + roomAvailabitily.RoomAvailabilityCount);

            return new AvailabilityCommandHandlerResult { Success = true, ResultData = outputBuilder.ToString() };
        }
        catch (Exception ex) when (ex is AvailabilityCommandHandlerParseException
            || ex is AvailabilityCommandHandlerValidateException
            || ex is RoomAvailabilityServiceException)
        {
            return HandleExceptionMessage<AvailabilityCommandHandlerResult>(ex);
        }
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
