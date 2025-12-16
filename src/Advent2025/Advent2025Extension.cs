using Advent2025.Solutions;
using AdventBase;
using Microsoft.Extensions.DependencyInjection;

namespace Advent2025;

public static class Advent2025Extension
{
    public static IServiceCollection AddAdvent2025(this IServiceCollection services)
    {
        services.AddSingleton<AdventYear, Advent2025>();
        services.AddTransient<Solution, Solution01>();
        services.AddTransient<Solution, Solution02>();
        services.AddTransient<Solution, Solution03>();
        services.AddTransient<Solution, Solution04>();
        services.AddTransient<Solution, Solution05>();
        services.AddTransient<Solution, Solution06>();
        services.AddTransient<Solution, Solution07>();
        services.AddTransient<Solution, Solution08>();
        services.AddTransient<Solution, Solution09>();
        services.AddTransient<Solution, Solution10>();
        services.AddTransient<Solution, Solution10Z3>();
        services.AddTransient<Solution, Solution11>();
        // services.AddTransient<Solution, Solution12>();
        return services;
    }
}