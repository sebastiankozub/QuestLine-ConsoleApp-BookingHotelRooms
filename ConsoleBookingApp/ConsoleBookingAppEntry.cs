using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ConsoleBookingApp.Configuration;
using BookingData;
using BookingApp.Service;
using QuickConsole;
using QuickConsole.Configuration;

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
            var services = new ServiceCollection();




            services
                .AddSingleton<IConfigurationRoot>(configuration);








            // OPTIONS PATTERN & CONFIGURATION POCOS
            var myFirstClass = configuration.GetSection(MyFirstClass.MyFirstClassSegmentName).Get<MyFirstClass>();
            if (myFirstClass is not null)
                services.AddSingleton<MyFirstClass>(myFirstClass);

            var mySecondClass = configuration.GetSection(SecondOptions.SecondSegmentName).Get<SecondOptions>();
            if (mySecondClass is not null)
                services.AddSingleton<SecondOptions>(mySecondClass);



            // BOOKING APP DOMAIN
            // DATALAYER
            services
                .AddSingleton<IDataContext>(sp => {
                    var consoleArgsParser = sp.GetRequiredService<ConsoleBookingAppArgsParser>(); // TODO change into QuickConsole/RunCommandArgsManager one 
                    var (hotelsFilename, bookingsFilname) = consoleArgsParser.Parse();
                    var dataContext = new DataContext(hotelsFilename, bookingsFilname);
                    return dataContext;
                });

            // SERVICES
            services.AddTransient<IRoomAvailabilityService, RoomAvailabilityService>();




            services
                .AddQuickConsole(ExitApplication, args, new ConsoleConfiguration { UseRunCommandArgsManager = true })
                .AddQuickOptions(configuration)
                .AddQuickHandlers()
                .AddQuickRunCommandArgs(args);

            // BUILD & RUN
            var serviceProvider = services.BuildServiceProvider();

            // DATA LAYER INITIALIZATION                     
            var dataContext = serviceProvider.GetRequiredService<IDataContext>();  
            // TODO check NuGet/HostInitActions for asyncronous initialization
            await dataContext.Initialization;

            // Run QuickConsole
            await serviceProvider.RunQuickConsole();
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
