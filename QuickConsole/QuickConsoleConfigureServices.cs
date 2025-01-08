using Microsoft.Extensions.DependencyInjection;

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

        //registering handlers from by main assebly
        // var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        services.AddSingleton<IQuickCommandLineParser, QuickCommandLineParser>();
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
