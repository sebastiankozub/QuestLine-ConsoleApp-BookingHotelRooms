using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using QuickConsole.Configuration;
using QuickConsole.Core;
using QuickConsole.Handler;
using QuickConsole.OldCommandHandler;
using QuickConsole.Options;
using QuickConsole.RunCommand;
using System;
using System.Runtime.CompilerServices;

namespace QuickConsole;

public static class QuickConsoleConfigureServices
{
    public static IServiceCollection AddQuickConsole(this IServiceCollection services, string[] args, ConsoleConfiguration? quickConsoleConfiguration = null)
    {
        services.AddSingleton<QuickConsoleEntryPoint>(sp =>
            {
                var argsManager = sp.GetService<RunCommandArgsManager>();

                if (argsManager != null)                
                    return new QuickConsoleEntryPoint(argsManager);                

                return new QuickConsoleEntryPoint();
            });


        // CORE
        services
            .AddSingleton<ILineCommandParser, LineCommandParser>()
            .AddSingleton<ConsoleBookingAppArgsParser>()
            .AddSingleton(new Action<int>(ExitApplication))
            .AddSingleton<CommandLineProcessor>();





        if (quickConsoleConfiguration is not null)
        {
            if (quickConsoleConfiguration.UseRunCommandArgsManager)
                services
                    .AddQuickRunCommandArgs(Environment.GetCommandLineArgs())
                    .AddSingleton(sp => {
                        var options = new ConsoleAppArgs { args = args };
                        return options;
                    });
        }




        return services;
    }

    public static IServiceCollection AddQuickHandlers(this IServiceCollection services)
    {
        // CONSOLE COMMAND HANDLERS  // Substituting with new command handlers
        var commandLineHandlers = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(x => !x.IsAbstract && x.IsClass && (x.GetInterface(nameof(IOldCommandHandler)) == typeof(IOldCommandHandler)));
        //        typeof(ConsoleBookingAppEntry).Assembly.GetTypes()
         //   .Where(x => !x.IsAbstract && x.IsClass && x.GetInterface(nameof(IOldCommandHandler)) == typeof(IOldCommandHandler));

        foreach (var commandLineHandler in commandLineHandlers)
        {
            services.Add(new ServiceDescriptor(typeof(IOldCommandHandler), commandLineHandler, ServiceLifetime.Transient));
        }
        services
            .AddTransient(sp => sp.GetServices<IOldCommandHandler>().ToDictionary(h => h.DefaultCommandName))
            .AddSingleton<ConsoleAppInterface>();

        // NEW COMMAND HANDLERS 
        var type = typeof(IHandler);

        var handlers = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(x => !x.IsAbstract && x.IsClass && (x.GetInterface(nameof(IHandler)) == typeof(IHandler)));

        //.Where(p => type.IsAssignableFrom(p)); // take also interface
        foreach (var handler in handlers)       
            services.Add(new ServiceDescriptor(typeof(IHandler), handler.Name, handler, ServiceLifetime.Transient));        

        services
            .AddTransient<Dictionary<string, string>>(sp => {
                var defautCommandNames = new Dictionary<string, string>();
                foreach (var handler in handlers)
                {
                    var h = sp.GetKeyedService<IHandler>(handler.Name);
                    var defautCommandName = h?.DefaultHandlerName;

                    if (defautCommandName is not null && h is not null)
                    {
                        var typeName = h.GetType().Name;
                        defautCommandNames.Add(defautCommandName, typeName);
                    }
                }
                return defautCommandNames;
            });

        return services;
    }

    public static IServiceCollection AddQuickRunCommandArgs(this IServiceCollection services, string[] consoleRunArgs)
    {
        services.AddSingleton(sp => {
            var options = new QuickRunCommandArgs { args = consoleRunArgs };
            return options;
        });
        services.AddSingleton<IQuickRunCommandnArgsParser>(sp => 
            new RunCommandArgsParser(sp.GetRequiredService<QuickRunCommandArgs>(), "--"));
        services.AddSingleton<RunCommandArgsManager>();

        return services;
    }

    public static IServiceCollection AddQuickRunCommandArgs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<UserInterfaceOptions>()
            .Bind(configuration.GetSection(UserInterfaceOptions.UserInterfaceSegmentName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<UserInterfaceCommandsOptions>()
            .Bind(configuration.GetSection(UserInterfaceCommandsOptions.CommandsSegmentName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }


    public static async Task RunQuickConsole(this ServiceProvider serviceProvider)
    {
        // RUN
        var consoleAppInterface = serviceProvider.GetRequiredService<ConsoleAppInterface>();
        await consoleAppInterface.RunInterfaceAsync();
    }


    public static void ExitApplication(int code)
    {
        Environment.Exit(code);
    }

}
