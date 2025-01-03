namespace ConsoleBookingApp.Core.CommandHandler;

public class CommandHandlerData
{
    public uint RepeatWhenFailes { get; set; } = 0;
    public required string UserCommandUsed { get; set; }
    public Action? PreHandleAction { get; set; } = null;
}

public class SearchCommandHandlerData : CommandHandlerData
{
    public required string RoomId { get; set; }
}

public class AvailabilityCommandHandlerData : CommandHandlerData
{
    public required string HotelId { get; set; }
}
