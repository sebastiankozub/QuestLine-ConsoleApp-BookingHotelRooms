using ConsoleBookingApp.Data;
using Microsoft.Extensions.DependencyInjection;
using ConsoleBookingApp.UserInterface;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace ConsoleBookingApp;

internal class ConsoleBookingAppEntry
{
    public static async Task Main(string[] args)
    {
        try
        {
            // ENVIRONMENT VARIABLES
            var appEnvironment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            // JSON FILES APP CONFIGURATION
            var configBuilder = new ConfigurationBuilder();

            configBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: false)
                .AddJsonFile($"config.{appEnvironment}.json", optional: true);

            var configuration = configBuilder.Build();

            // DEPENDENCY INJECTION 
            var services = new ServiceCollection()
                .AddSingleton<IConfigurationRoot>(configuration)
                .AddSingleton<ICommandLineParser, CommandLineParser>()
                .AddSingleton<CommandLineProcessor>();

            services.AddSingleton(sp => { 
                var options = new BookingAppConsoleArgsParserOptions { args = args }; 
                return options; 
            });

            services.AddSingleton<BookingAppConsoleArgsParser>();

            services.AddSingleton(sp => {
                var consoleArgsParser = sp.GetRequiredService<BookingAppConsoleArgsParser>();
                var (hotelsFilename, bookingsFilname) = consoleArgsParser.Parse();
                var dc = new DataContext(hotelsFilename, bookingsFilname);
                return dc;
            });

            var comandLineHandlers = typeof(ConsoleBookingAppEntry).Assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsClass && x.GetInterface(nameof(ICommandLineHandler)) == typeof(ICommandLineHandler));

            foreach (var commandLineHandler in comandLineHandlers)
            {
                services.Add(new ServiceDescriptor(typeof(ICommandLineHandler), commandLineHandler, ServiceLifetime.Singleton));
            }

            services.AddSingleton(sp => sp.GetServices<ICommandLineHandler>().ToDictionary(h => h.CommandName));
            services.AddSingleton<BookingAppConsoleInterface>();


            // REGISTER IOptions
            services.AddOptions();

            var myFirstClass = configuration.GetSection(MyFirstClass.MyFirstClassOptionsSegmentName).Get<MyFirstClass>();
            if (myFirstClass is not null)
                services.AddSingleton < MyFirstClass>(myFirstClass);
            var mySecondClass = configuration.GetSection(SecondOptions.SecondOptionsSegmentName).Get<SecondOptions>();
            if (mySecondClass is not null)
                services.AddSingleton<SecondOptions>(mySecondClass);

            services.AddOptions<UserInterfaceOptions>()
                .Bind(configuration.GetSection("MyOptions"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            UserInterfaceOptions userInterfaceOptions = new();
            configuration.GetSection(UserInterfaceOptions.UserInterfaceSegmentName).Bind(userInterfaceOptions);
            services.ConfigureOptions<UserInterfaceOptions>(configuration.GetSection(UserInterfaceOptions.UserInterfaceSegmentName));
            services.Configure<UserInterfaceOptions>(configuration.GetSection(UserInterfaceOptions.UserInterfaceSegmentName));

            services.AddOptionsWithValidateOnStart<UserInterfaceOptions>(UserInterfaceOptions.UserInterfaceSegmentName);

            //

            var section = configuration.GetSection(MyFirstClass.MyFirstClassOptionsSegmentName);
            services.Configure<MyFirstClass>(section);



            var serviceProvider = services.BuildServiceProvider();



            // DATA LAYER INITIALIZATION                     
            var dataContext = serviceProvider.GetRequiredService<DataContext>();
            await dataContext.Initialization;

            // RUN
            var consoleAppInterface = serviceProvider.GetRequiredService<BookingAppConsoleInterface>();
            await consoleAppInterface.RunInterfaceAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal error during application run.\nProblem detail:\n{ex.Message}");
        }
    }

    internal static void ExitApplication(int code)
    {
        Environment.Exit(code);
    }
}

public class MyFirstClass
{
    public static string MyFirstClassOptionsSegmentName = "FirstOptions";

    public string Option1 { get; set; }
    public int Option2 { get; set; }
}

public class SecondOptions
{
    public static string SecondOptionsSegmentName = "SecondOptions";

    public string SettingOne { get; set; }
    public int SettingTwo { get; set; }
}

public class UserInterfaceOptions
{
    public static string UserInterfaceSegmentName = "UserInterface";

    [Required]
    [MinLength(5)]
    public string HelpCommand { get; set; }

    [Required]
    [MinLength(5)]
    public string ExitCommand { get; set; }

    [Required]
    [MinLength(5)]
    public string SearchCommand { get; set; }

    [Required]
    [MinLength(5)]
    public string AvailabilityCommand { get; set; }

    [Required]
    [MinLength(5)]
    public string CommandPrompt { get; set; }
}




