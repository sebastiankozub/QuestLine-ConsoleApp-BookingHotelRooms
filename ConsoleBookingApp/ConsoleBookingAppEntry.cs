using Microsoft.Extensions.DependencyInjection;
using ConsoleBookingApp.UserInterface;
using Microsoft.Extensions.Configuration;
using ConsoleBookingApp.Configuration;
using ConsoleBookingApp.CommandHandler;
using BookingData;
using System.Xml.Serialization;
using BookingApp.Service;

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

            var services = new ServiceCollection();

            // OPTIONS PATTERN & CONFIGURATION POCOS
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

            // DATA CONTEXT
            services
                .AddSingleton<ConsoleAppArgsParser>()
                .AddSingleton(sp => {
                    var consoleArgsParser = sp.GetRequiredService<ConsoleAppArgsParser>();
                    var (hotelsFilename, bookingsFilname) = consoleArgsParser.Parse();
                    var dataContext = new DataContext(hotelsFilename, bookingsFilname);
                    return dataContext;
                });

            // ALL HANDLERS IN ASSEMBLY
            var commandLineHandlers = typeof(ConsoleBookingAppEntry).Assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsClass && x.GetInterface(nameof(ICommandHandler)) == typeof(ICommandHandler));
            foreach (var commandLineHandler in commandLineHandlers)
                services.Add<ICommandHandler>(new ServiceDescriptor(typeof(ICommandHandler), commandLineHandler, ServiceLifetime.Singleton));

            // DEPENDENCY INJECTION 
            services
                .AddSingleton<IConfigurationRoot>(configuration)
                .AddSingleton<ICommandLineParser, CommandLineParser>()
                .AddSingleton(new Action<int>(ExitApplication))
                .AddSingleton<CommandLineProcessor>()
                .AddSingleton(sp => {
                    var options = new ConsoleAppArgs { args = args };
                    return options;
                });

            services
                .AddSingleton(sp => sp.GetServices<ICommandHandler>().ToDictionary(h => h.CommandName))
                .AddSingleton<ConsoleAppInterface>();

            // BOOKINGAPP SERVICES
            services.AddTransient<IRoomAvailabilityService, RoomAvailabilityService>();

            var serviceProvider = services.BuildServiceProvider();

            // DATA LAYER INITIALIZATION
            var dataContext = serviceProvider.GetRequiredService<DataContext>();  // check NuGet/HostInitActions for asyncronous initialization
            await dataContext.Initialization;

            // RUN
            var consoleAppInterface = serviceProvider.GetRequiredService<ConsoleAppInterface>();
            await consoleAppInterface.RunInterfaceAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal or non-servicable exception during application run.\nProblem detail:\n{ex.Message}");
        }
    }

    public static void ExitApplication(int code)
    {
        Environment.Exit(code);
    }
}




