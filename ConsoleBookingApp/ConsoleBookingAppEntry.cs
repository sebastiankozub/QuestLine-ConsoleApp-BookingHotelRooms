using ConsoleBookingApp.Data;
using Microsoft.Extensions.DependencyInjection;
using ConsoleBookingApp.UserInterface;
using Microsoft.Extensions.Configuration;
using ConsoleBookingApp.Configuration;

namespace ConsoleBookingApp;

internal class ConsoleBookingAppEntry
{
    public static async Task Main(string[] args)
    {
        try
        {
            // ENVIRONMENT VARIABLES
            var appEnvironment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            // JSON CONFIG FILES
            var configBuilder = new ConfigurationBuilder();
            configBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: false)
                .AddJsonFile($"config.{appEnvironment}.json", optional: false);

            var configuration = configBuilder.Build();

            // DEPENDENCY INJECTION 
            var services = new ServiceCollection()
                .AddSingleton<IConfigurationRoot>(configuration)
                .AddSingleton<ICommandLineParser, CommandLineParser>()
                .AddSingleton<CommandLineProcessor>()
                .AddSingleton(sp => { 
                    var options = new BookingAppConsoleArgsParserOptions { args = args }; 
                    return options; 
                });

            services
                .AddSingleton<BookingAppConsoleArgsParser>()
                .AddSingleton(sp => {
                    var consoleArgsParser = sp.GetRequiredService<BookingAppConsoleArgsParser>();
                    var (hotelsFilename, bookingsFilname) = consoleArgsParser.Parse();
                    var dataContext = new DataContext(hotelsFilename, bookingsFilname);
                    return dataContext;
                });

            var commandLineHandlers = typeof(ConsoleBookingAppEntry).Assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsClass && x.GetInterface(nameof(ICommandLineHandler)) == typeof(ICommandLineHandler));
            foreach (var commandLineHandler in commandLineHandlers)            
                services.Add(new ServiceDescriptor(typeof(ICommandLineHandler), commandLineHandler, ServiceLifetime.Singleton));
            
            services
                .AddSingleton(sp => sp.GetServices<ICommandLineHandler>().ToDictionary(h => h.CommandName))
                .AddSingleton<BookingAppConsoleInterface>();

            // IOptions & configuration pocos
            var myFirstClass = configuration.GetSection(MyFirstClass.MyFirstClassSegmentName).Get<MyFirstClass>();
            if (myFirstClass is not null)
                services.AddSingleton<MyFirstClass>(myFirstClass);

            var mySecondClass = configuration.GetSection(SecondOptions.SecondSegmentName).Get<SecondOptions>();
            if (mySecondClass is not null)
                services.AddSingleton<SecondOptions>(mySecondClass);

            services.AddOptions<UserInterfaceOptions>()
                .Bind(configuration.GetSection(UserInterfaceOptions.UserInterfaceSegmentName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<UserInterfaceCommandsOptions>()
                .Bind(configuration.GetSection(UserInterfaceCommandsOptions.CommandsSegmentName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var serviceProvider = services.BuildServiceProvider();

            // DATA LAYER INITIALIZATION                     
            var dataContext = serviceProvider.GetRequiredService<DataContext>();  // check -> NuGet/HostInitActions 
            await dataContext.Initialization;

            // RUN
            var consoleAppInterface = serviceProvider.GetRequiredService<BookingAppConsoleInterface>();
            await consoleAppInterface.RunInterfaceAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal or non-servicable exception during application run.\nProblem detail:\n{ex.Message}");
        }
    }

    internal static void ExitApplication(int code)
    {
        Environment.Exit(code);
    }
}




