using Microsoft.Extensions.DependencyInjection;
using Turtle.Handlers;
using Turtle.Maps;
using Turtle.Repositories;

namespace Turtle;

public static class ServiceCollectionExt
{
    public static IServiceCollection AddTurtle(this IServiceCollection services)
    {
        services.AddSingleton<CrushHandlerBase, CrushIntoWallHandler>()
            .AddSingleton<Map>()
            .AddSingleton<TurtleProvider>()
            .AddTransient<Turtle>()
            .AddSingleton<IMapRepository, InMemoryMapRepository>();

        return services;
    }
}