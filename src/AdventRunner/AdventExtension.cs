using Advent2025;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AdventRunner;

public static class AdventExtension
{
    public static IServiceCollection AddAdventYears(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<AdventRegistry>();
        
        serviceCollection.AddAdvent2025();
        
        serviceCollection.AddAdventLogging();
        
        return serviceCollection;
    }

    public static IServiceCollection AddAdventLogging(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddLogging(lb =>
        {
            lb.SetMinimumLevel(LogLevel.Debug);
            lb.AddSimpleConsole();
        });
        return serviceCollection;
    }
}