using Microsoft.Extensions.DependencyInjection;

namespace QuickConsole;

public static class QuickConsoleConfigureServicesExtension
{
    public static IServiceCollection AddQuickConsole(this IServiceCollection services)
    {
        services.AddSingleton<QuickConsoleEntryPoint>(
            sp =>
        {
            if (sp.GetService<QuickConsoleRunArgsManager>() != null)
            {
                var argsManager = sp.GetService<QuickConsoleRunArgsManager>();
                return new QuickConsoleEntryPoint(argsManager);
            }

            return new QuickConsoleEntryPoint();
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
