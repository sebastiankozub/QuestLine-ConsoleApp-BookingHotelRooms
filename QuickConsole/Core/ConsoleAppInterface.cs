using Microsoft.Extensions.Options;
using QuickConsole;
using QuickConsole.Options;

namespace QuickConsole.Core;

internal class ConsoleAppInterface(QuickConsoleEntryPoint c, CommandLineProcessor processor, IOptions<UserInterfaceOptions> userInterfaceOptions, IOptions<UserInterfaceCommandsOptions> userInterfaceCommandsOptions)
{
    private readonly CommandLineProcessor _cmdLineProcessor = processor;
    private readonly UserInterfaceOptions _userInterfaceOptions = userInterfaceOptions.Value;

    private readonly QuickConsoleEntryPoint _c = c;

    private readonly string _helpCommandName = userInterfaceCommandsOptions.Value.Help ?? nameof(UserInterfaceCommandsOptions.Help);

    public async Task RunInterfaceAsync()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("CONSOLE BOOKING APP");
                Console.WriteLine("-------------------");

                // TODO abstract to different place Console class calls - ones like below and above
                // TODO create interface Display and Read, create wrapper Console Read and Write be used as an interface to be mocked and test ui logic

                Console.WriteLine($"Type {_helpCommandName}() for quick help or use favorite known command!"); //TODO AliasResolver
                Console.WriteLine();
                Console.Write(_userInterfaceOptions.CommandPrompt);  

                var commandLine = Console.ReadLine();

                var commandLineProcessorResult = string.IsNullOrEmpty(commandLine) 
                    ? await _cmdLineProcessor.ProcessCommandAsync("")
                    : await _cmdLineProcessor.ProcessCommandAsync(commandLine);

                if (commandLineProcessorResult.Success is false)
                {
                    Console.WriteLine($"Failed to execute given command  [{commandLine}]");
                    Console.WriteLine($"Error message: {commandLineProcessorResult.ExceptionMessage}");
                }
                else
                {
                    Console.WriteLine($"Command: [{commandLine}] executed succesfully." + Environment.NewLine);
                    
                    if (!string.IsNullOrEmpty(commandLineProcessorResult.Message))
                    {
                        Console.WriteLine("Message: " + Environment.NewLine);
                        Console.WriteLine(commandLineProcessorResult.Message + Environment.NewLine);
                    }

                    if (!string.IsNullOrEmpty(commandLineProcessorResult.Result))
                    {
                        Console.WriteLine("Command result: " + Environment.NewLine);
                        Console.WriteLine(commandLineProcessorResult.Result + Environment.NewLine);
                    }
                }

                if (commandLineProcessorResult.PostProcess is not null)
                    commandLineProcessorResult.PostProcess(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Application failed to service your command." + Environment.NewLine);
                Console.WriteLine($"Error message: {Environment.NewLine + ex.Message}");
            }
        }
    }
}