using Turtle.Commands;
using Turtle.Handlers;

namespace Turtle;

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