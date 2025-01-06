using BookingApp.Service;

namespace ConsoleBookingApp.CommandHandler;

public class TestIfAutoAddedCommandHandler(IRoomAvailabilityService roomAvailabilityService) : CommandHandler("TestIfAutoAdded")
{
    private readonly IRoomAvailabilityService _roomAvailabilityService = roomAvailabilityService;

    public async override Task<ICommandHandlerResult> HandleAsync(string[] parameters)
    {
        if (_roomAvailabilityService is null)
            throw new ArgumentNullException(nameof(_roomAvailabilityService));

        return await Task.FromResult(new CommandHandlerResult { Success = true, ResultData = "TestIfAutoAddedCommandHandlerResult" });
    }
}