using System.Diagnostics;
using ConsoleApp3.Commands;

namespace TheTurtle;

public class Turtle {
    private List<CommandBase> _log = new ();
    public void Turn(TurnDirections dir) {
        _log.Add(new TurnCommand(dir));
    }
    public void MoveForward(int distance) {
        _log.Add(new MoveCommand(distance));
    }

    public TurtleState whatIsMyState() {
        return _log.Aggregate(new TurtleState(), (state, command) => command.ApplyCommand(state));
    }
}