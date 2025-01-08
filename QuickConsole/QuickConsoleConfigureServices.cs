using Microsoft.Extensions.DependencyInjection;
using QuickConsole.Handler;

namespace QuickConsole;

public static class QuickConsoleConfigureServices
{
    public static IServiceCollection AddQuickConsole(this IServiceCollection services)
    {
        services.AddSingleton<QuickConsoleEntryPoint>(sp =>
            {
                var argsManager = sp.GetService<QuickConsoleRunArgsManager>();

                if (argsManager != null)                
                    return new QuickConsoleEntryPoint(argsManager);                

                return new QuickConsoleEntryPoint();
            });

        services.AddSingleton<IQuickCommandLineParser, QuickCommandLineParser>();
        return services;
    }

    public static IServiceCollection AddQuickHandlers(this IServiceCollection services)
    {
        // NEW COMMAND HANDLERS 
        var type = typeof(IHandler);

        var handlers = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(x => !x.IsAbstract && x.IsClass && (x.GetInterface(nameof(IHandler)) == typeof(IHandler)));

        //.Where(p => type.IsAssignableFrom(p)); // take also interface
        //x => !x.IsAbstract && 
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

    public static IServiceCollection AddQuickCommandLineArguments(this IServiceCollection services, string[] consoleRunArgs)
    {
        services.AddSingleton(sp => {
            var options = new QuickConsoleRunArgs { args = consoleRunArgs };
            return options;
        });
        
        services.AddSingleton<QuickConsoleRunArgsManager>();

        return services;
    }
}
