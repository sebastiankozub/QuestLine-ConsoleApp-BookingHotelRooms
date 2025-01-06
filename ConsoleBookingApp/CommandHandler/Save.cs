using BookingApp.Service;
using BookingData;

namespace ConsoleBookingApp.CommandHandler;

public class Save(IDataContext dataContext) : CommandHandler("Save")
{
    private readonly IDataContext _dataContext = dataContext;

    public async override Task<ICommandHandlerResult> HandleAsync(string[] parameters)
    {
        await _dataContext.SaveAsync();
        return await Task.FromResult(new CommandHandlerResult { Success = true, ResultData = "Saved" });
    }
}