using Microsoft.Extensions.DependencyInjection;
using TurtleApp.Commands;
using TurtleApp.Handlers;
using TurtleApp.Maps;
using TurtleApp.Repositories;
using System.Drawing;

namespace TurtleApp;

public class Turtle {
    private TurtleState _initialState;
    private IEnumerable<CrushHandlerBase> _crushHandlers;
    
    private List<CommandBase> _log = new ();
    public void Turn(TurnDirections dir) {
        _log.Add(new TurnCommand(dir));
    }
    public void MoveForward(int distance) {
        _log.Add(new MoveCommand(distance, _crushHandlers));
    }

    public void TakeOff() {
        _log.Add(new TakeOffCommand());
    }

    public void Land() {
        _log.Add(new LandCommand());
    }

    public TurtleState WhatIsMyState() {
        return _log.Aggregate(_initialState, (state, command) => command.ApplyCommand(state));
    }

    public Turtle(TurtleState initialState, IEnumerable<CrushHandlerBase> crushHandlers)
    {
        _initialState = initialState;
        _crushHandlers = crushHandlers;
    }
    public Turtle(Maps.Map map, IEnumerable<CrushHandlerBase> crushHandlers) : this(new TurtleState(map: map), crushHandlers) {}

    public Turtle(IEnumerable<CrushHandlerBase> crushHandlers) : this(new TurtleState(map: new Maps.Map()), crushHandlers) {}
}


public static class ServiceCollectionExt
{
    public static IServiceCollection AddTurtle(this IServiceCollection services)
    {
        services.AddSingleton<CrushHandlerBase, CrushIntoWallHandler>()
            .AddSingleton<Map>()
            .AddSingleton<Turtle>()
            .AddSingleton<IMapRepository, InMemoryMapRepository>();
        //services.AddSingleton<IMapRepository, InFileMapRepository>();

        return services;
    }
}
