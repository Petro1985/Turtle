using Microsoft.Extensions.DependencyInjection;
using Turtle.Handlers;
using Turtle.Maps;
using Turtle.Repositories;

namespace Turtle;

public static class ServiceCollectionExt
{
    public static IServiceCollection AddTurtle(this IServiceCollection services, string pathToMap)
    {
        services.AddSingleton<CrushHandlerBase, CrushIntoWallHandler>()
            .AddSingleton<Map>(provider =>
            {
                var rep = provider.GetRequiredService<IMapRepository>();
                return rep.Get(1) ?? new Map();
            })
            .AddSingleton<TurtleProvider>()
            .AddTransient<Turtle>()
            .AddSingleton<IMapRepository, InFileMapRepository>(provider => new InFileMapRepository(pathToMap));
            //.AddSingleton<IMapRepository, InMemoryMapRepository>();
        return services;
    }
}