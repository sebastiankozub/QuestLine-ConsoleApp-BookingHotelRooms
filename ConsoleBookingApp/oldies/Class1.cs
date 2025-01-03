//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//// Interfaces (same as before)

//// Implementations (same as before)

//public class CommandLineProcessor
//{
//    private readonly ICommandLineParser _parser;
//    private readonly Dictionary<string, ICommandLineHandler> _commandLineHandlers;

//    public CommandLineProcessor(ICommandLineParser parser, Dictionary<string, ICommandLineHandler> handlers)
//    {
//        _parser = parser;
//        _commandLineHandlers = handlers;
//    }

//    public async Task ProcessCommandAsync(string commandLine)
//    {
//        var (commandName, parameters) = _parser.Parse(commandLine);

//        if (commandName == null)
//        {
//            Console.WriteLine("Invalid command format.");
//            return;
//        }

//        if (_commandLineHandlers.TryGetValue(commandName, out var handler))
//        {
//            await handler.HandleAsync(parameters);
//        }
//        else
//        {
//            Console.WriteLine($"Command '{commandName}' not found.");
//        }
//    }
//}

//public class Program
//{
//    public static async Task Main(string[] args)
//    {
//        var commandDescriptions = new Dictionary<string, string>
//        {
//            { "Availability", "Checks availability." },
//            { "Exit", "Exits the application." },
//            { "Help", "Displays available commands." }
//        };

//        var serviceCollection = new ServiceCollection()
//            .AddSingleton<ICommandLineParser, CommandParser>()
//            .AddSingleton(commandDescriptions)
//            .AddTransient<CommandLineProcessor>();

//        // Register command handlers in the dictionary
//        var commandHandlers = new Dictionary<string, Type>
//        {
//            { "Availability", typeof(AvailabilityCommandLineHandler) },
//            { "Exit", typeof(ExitCommandLineHandler) },
//            { "Help", typeof(HelpCommandHandler) }
//        };

//        foreach (var kvp in commandHandlers)
//        {
//            serviceCollection.AddTransient(typeof(ICommandLineHandler), kvp.Value); // Register as ICommandLineHandler
//        }

//        var serviceProvider = serviceCollection.BuildServiceProvider();

//        //Resolve the command handlers and create the dictionary
//        var handlers = serviceProvider.GetServices<ICommandLineHandler>().ToDictionary(h => h.CommandName);

//        //Override the dictionary of handlers in the service provider
//        serviceCollection.AddSingleton(handlers);
//        serviceProvider = serviceCollection.BuildServiceProvider();//Rebuild service provider

//        var processor = serviceProvider.GetRequiredService<CommandLineProcessor>();

//        while (true)
//        {
//            // ... (same as before)
//        }
//    }
//}