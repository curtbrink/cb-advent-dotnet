using Advent2016.Solutions;
using AdventBase;
using Microsoft.Extensions.DependencyInjection;

namespace Advent2016;

public static class Advent2016Extension
{
    public static IServiceCollection AddAdvent2016(this IServiceCollection services)
    {
        services.AddSingleton<AdventYear, Advent2016>();
        services.AddTransient<Solution, Solution01>();
        services.AddTransient<Solution, Solution02>();
        
        return services;
    }
}