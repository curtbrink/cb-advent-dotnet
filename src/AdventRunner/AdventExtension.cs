using Advent2016;
using Advent2025;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AdventRunner;

public static class AdventExtension
{
    public static IServiceCollection AddAdventYears(this IServiceCollection serviceCollection, bool debug)
    {
        serviceCollection.AddSingleton<AdventRegistry>();

        serviceCollection.AddAdvent2016();
        serviceCollection.AddAdvent2025();
        
        serviceCollection.AddAdventLogging(debug);
        
        return serviceCollection;
    }

    public static IServiceCollection AddAdventLogging(this IServiceCollection serviceCollection, bool debug)
    {
        serviceCollection.AddLogging(lb =>
        {
            lb.SetMinimumLevel(debug ? LogLevel.Debug : LogLevel.Information);
            lb.AddSimpleConsole(f =>
            {
                f.SingleLine = true;
                f.IncludeScopes = false;
            });
        });
        return serviceCollection;
    }
}