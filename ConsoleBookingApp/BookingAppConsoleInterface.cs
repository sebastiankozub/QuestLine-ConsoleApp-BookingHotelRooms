using ConsoleBookingApp.Data;
using ConsoleBookingApp.UserInterface;
using Microsoft.Extensions.Options;

namespace ConsoleBookingApp;

internal class BookingAppConsoleInterface(CommandLineProcessor processor, DataContext dataCtx, IOptions<UserInterfaceOptions> userInterfaceOptions)
{
    private readonly CommandLineProcessor _cmdLineProcessor = processor;
    private readonly DataContext _dataContext = dataCtx;
    private readonly UserInterfaceOptions _userInterfaceOptions = userInterfaceOptions.Value;

    public async Task RunInterfaceAsync()
    {
        while (true)
        {
            Console.WriteLine("CONSOLE BOOKING APP");
            Console.WriteLine($"Type {_userInterfaceOptions.HelpCommand}() for quick help or use favorite known command!");  
            Console.WriteLine();
            Console.Write(_userInterfaceOptions.CommandPrompt);

            var commandLine = Console.ReadLine();

            var commandLineProcessorResult = await _cmdLineProcessor.ProcessCommandAsync(string.IsNullOrEmpty(commandLine) ? "" : commandLine);

            if (commandLineProcessorResult.Success is false)
                Console.WriteLine($"Failed to service the command: {commandLine}");

            Console.WriteLine(commandLineProcessorResult.Message);

            if (commandLineProcessorResult.PostResultAction is not null)
                commandLineProcessorResult.PostResultAction();        
        }
    }
}