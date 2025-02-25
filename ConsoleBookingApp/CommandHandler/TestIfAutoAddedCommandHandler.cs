﻿using BookingApp.Service;
using QuickConsole.OldCommandHandler;

namespace ConsoleBookingApp.CommandHandler;

public class TestIfAutoAddedCommandHandler(IRoomAvailabilityService roomAvailabilityService) : OldCommandHandler("TestIfAutoAdded")
{
    private readonly IRoomAvailabilityService _roomAvailabilityService = roomAvailabilityService;

    public async override Task<IOldCommandHandlerResult> HandleAsync(string[] parameters)
    {
        if (_roomAvailabilityService is null)
            throw new ArgumentNullException(nameof(TestIfAutoAddedCommandHandler));

        return await Task.FromResult(new OldCommandHandlerResult { Success = true, ResultData = "TestIfAutoAddedCommandHandlerResult" });
    }
}