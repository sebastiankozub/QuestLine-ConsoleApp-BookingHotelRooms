using Microsoft.Extensions.DependencyInjection;
using ConsoleBookingApp.UserInterface;
using Microsoft.Extensions.Configuration;
using ConsoleBookingApp.Configuration;
using ConsoleBookingApp.CommandHandler;
using BookingData;
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

            // DEPENDENCY INJECTION 
            var services = new ServiceCollection()
                .AddSingleton<IConfigurationRoot>(configuration);

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

            // CONSOLE APP DOMAIN
            services
                .AddSingleton<ICommandLineParser, CommandLineParser>()
                .AddSingleton<ConsoleBookingAppArgsParser>()
                .AddSingleton(new Action<int>(ExitApplication))
                .AddSingleton<CommandLineProcessor>()
                .AddSingleton(sp => {
                    var options = new ConsoleAppArgs { args = args };
                    return options;
                });

            // DATALAYER
            services
                .AddSingleton<IDataContext>(sp => {
                    var consoleArgsParser = sp.GetRequiredService<ConsoleBookingAppArgsParser>();
                    var (hotelsFilename, bookingsFilname) = consoleArgsParser.Parse();
                    var dataContext = new DataContext(hotelsFilename, bookingsFilname);
                    return dataContext;
                });

            // CONSOLE COMMAND HANDLERS   // TODO - make real transient
            var commandLineHandlers = typeof(ConsoleBookingAppEntry).Assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsClass && x.GetInterface(nameof(ICommandHandler)) == typeof(ICommandHandler));
            foreach (var commandLineHandler in commandLineHandlers)
                services.Add(new ServiceDescriptor(typeof(ICommandHandler), commandLineHandler, ServiceLifetime.Transient));

            services
                .AddTransient(sp => sp.GetServices<ICommandHandler>().ToDictionary(h => h.DefaultCommandName))
                .AddSingleton<ConsoleAppInterface>();

            // BOOKING APP DOMAIN
            services.AddTransient<IRoomAvailabilityService, RoomAvailabilityService>();

            // BUILD & RUN
            var serviceProvider = services.BuildServiceProvider();

            // DATA LAYER INITIALIZATION                     
            var dataContext = serviceProvider.GetRequiredService<IDataContext>();  // TODO check NuGet/HostInitActions for asyncronous initialization
            await dataContext.Initialization;

            // RUN
            var consoleAppInterface = serviceProvider.GetRequiredService<ConsoleAppInterface>();
            await consoleAppInterface.RunInterfaceAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Application failed to start." + Environment.NewLine);
            Console.WriteLine($"Error message: {Environment.NewLine + ex.Message}");
            Console.WriteLine($"Application closing...");
        }
    }

    public static void ExitApplication(int code)
    {
        Environment.Exit(code);
    }
}




