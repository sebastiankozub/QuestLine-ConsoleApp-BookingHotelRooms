using ConsoleBookingApp.Configuration;
using BookingData;
using Microsoft.Extensions.Options;

namespace ConsoleBookingApp.UserInterface;

internal class ConsoleAppInterface(CommandLineProcessor processor, DataContext dataCtx, IOptions<UserInterfaceOptions> userInterfaceOptions, IOptions<UserInterfaceCommandsOptions> userInterfaceCommandsOptions)
{
    private readonly CommandLineProcessor _cmdLineProcessor = processor;
    private readonly DataContext _dataContext = dataCtx;
    private readonly UserInterfaceOptions _userInterfaceOptions = userInterfaceOptions.Value;
    private readonly UserInterfaceCommandsOptions _userInterfaceCommandsOptions = userInterfaceCommandsOptions.Value;

    private readonly string _helpCommandName = userInterfaceCommandsOptions.Value.Help ?? nameof(UserInterfaceCommandsOptions.Help);

    public async Task RunInterfaceAsync()
    {
        while (true)
        {
            Console.WriteLine("CONSOLE BOOKING APP");
            Console.WriteLine($"Type {_helpCommandName}() for quick help or use favorite known command!");
            Console.WriteLine();
            Console.Write(_userInterfaceOptions.CommandPrompt);  // TODO abstract to different place Console class calls - ones like above and below

            var commandLine = Console.ReadLine();

            var commandLineProcessorResult = await _cmdLineProcessor.ProcessCommandAsync(string.IsNullOrEmpty(commandLine) ? "" : commandLine);

            if (commandLineProcessorResult.Success is false)
                Console.WriteLine($"Failed to service the command: {commandLine}");


            Console.WriteLine(commandLineProcessorResult.Message);


            if (commandLineProcessorResult.PostProcess is not null)
                commandLineProcessorResult.PostProcess(0);
        }
    }
}