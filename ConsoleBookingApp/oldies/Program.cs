//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.ObjectModel;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//// Define interfaces
//public interface ICommandLineParser
//{
//    (string CommandName, string[] Parameters) Parse(string command);
//}

//public interface ICommandLineHandler
//{
//    Task HandleAsync(string[] parameters);
//    string CommandName { get; }
//}

//// Implementations
//public class CommandParser : ICommandLineParser
//{
//    public (string CommandName, string[] Parameters) Parse(string command)
//    {
//        string pattern = @"^(\w+)\(([^)]*)\)$";
//        Match match = Regex.Match(command.Trim(), pattern);

//        if (match.Success)
//        {
//            string commandName = match.Groups[1].Value;
//            string parametersString = match.Groups[2].Value;
//            string[] parameters = parametersString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
//                .Select(p => p.Trim())
//                .ToArray();
//            return (commandName, parameters);
//        }
//        else
//        {
//            return (null, null); // Or throw an exception if you prefer
//        }
//    }
//}

//public class AvailabilityCommandLineHandler : ICommandLineHandler
//{
//    public string CommandName => "Availability";

//    public async Task HandleAsync(string[] parameters)
//    {
//        Console.WriteLine("Handling Availability command...");
//        if (parameters != null)
//        {
//            foreach (var parameter in parameters)
//            {
//                Console.WriteLine($"- Parameter: {parameter}");
//            }
//        }
//    }
//}

//public class ExitCommandLineHandler : ICommandLineHandler
//{
//    public string CommandName => "Exit";

//    public async Task HandleAsync(string[] parameters)
//    {
//        Environment.Exit(0);
//    }
//}

////public class HelpCommandHandler : ICommandLineHandler
////{
////    private readonly IEnumerable<ICommandLineHandler> _commandHandlers;

////    public HelpCommandHandler(IEnumerable<ICommandLineHandler> commandHandlers)
////    {
////        _commandHandlers = commandHandlers;
////    }
////    public string CommandName => "Help";

////    public async Task HandleAsync(string[] parameters)
////    {
////        Console.WriteLine("Available Commands:");
////        foreach (var handler in _commandHandlers)
////        {
////            Console.WriteLine($"- {handler.CommandName}");
////        }
////    }
////}

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

//        var handler = _commandLineHandlers.FirstOrDefault(h => h.Value.CommandName.Equals(commandName, StringComparison.OrdinalIgnoreCase)).Value;

//        if (handler != null)
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
//        // Setup Dependency Injection
//        var commandDescriptions = new Dictionary<string, string>
//        {
//            { "Availability", "Checks availability." },
//            { "Exit", "Exits the application." },
//            //{ "Help", "Displays available commands." }
//        };

//        var serviceCollection = new ServiceCollection()
//            .AddSingleton<ICommandLineParser, CommandParser>()
//            .AddSingleton(commandDescriptions)
//            .AddTransient<CommandLineProcessor>();

//        //// Register command handlers in the dictionary
//        var commandHandlers = new Dictionary<string, Type>
//        {
//            { "Availability", typeof(AvailabilityCommandLineHandler) },
//            { "Exit", typeof(ExitCommandLineHandler) },
//            //{ "Help", typeof(HelpCommandHandler) }
//        };

//        foreach (var kvp in commandHandlers)
//        {
//            serviceCollection.AddTransient(typeof(ICommandLineHandler), kvp.Value); // Register as ICommandLineHandler
//        }

//        var serviceProvider = serviceCollection.BuildServiceProvider();

//        //Resolve the command handlers and create the dictionary
//        var handlers = serviceProvider.GetServices<ICommandLineHandler>().ToDictionary(h => h.CommandName);
//        serviceCollection.AddSingleton(handlers);


//        serviceProvider = serviceCollection.BuildServiceProvider();//Rebuild service provider

//        var processor = serviceProvider.GetRequiredService<CommandLineProcessor>();

//        while (true)
//        {
//            Console.Write("> ");
//            string commandLine = Console.ReadLine();

//            if (!string.IsNullOrWhiteSpace(commandLine))
//            {
//                await processor.ProcessCommandAsync(commandLine);
//            }
//        }
//    }
//}