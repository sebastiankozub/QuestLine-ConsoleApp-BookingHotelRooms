using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBookingApp.CommandHandler;

public class TestIfAutoAddedCommandHandler(IRoomAvailabilityService roomAvailabilityService) : CommandHandler("TestIfAutoAdded")
{
    private readonly IRoomAvailabilityService _roomAvailabilityService = roomAvailabilityService;

    public async override Task<ICommandHandlerResult> HandleAsync(string[] parameters)
    {
        return await Task.FromResult(new CommandHandlerResult { Success = true, ResultData = "TestIfAutoAddedCommandHandlerResult" });
    }
}