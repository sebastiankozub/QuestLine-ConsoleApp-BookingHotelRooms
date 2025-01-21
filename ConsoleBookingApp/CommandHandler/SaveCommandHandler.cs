using BookingApp.Service;
using BookingData;
using QuickConsole.OldCommandHandler;

namespace ConsoleBookingApp.CommandHandler;

public class SaveCommandHandler(IDataContext dataContext) : OldCommandHandler("Save")
{
    private readonly IDataContext _dataContext = dataContext;

    public async override Task<IOldCommandHandlerResult> HandleAsync(string[] parameters)
    {
        await _dataContext.SaveAsync();
        return await Task.FromResult(new OldCommandHandlerResult { Success = true, ResultData = "Saved" });
    }
}