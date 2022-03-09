using System.Diagnostics;

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
        return _log.Aggregate(new TurtleState(), (state, command) => {
            var newState = new TurtleState {
                Direction = state.Direction,
                x = state.x,
                y = state.y
            };
             switch (command) {
                case MoveCommand moveCommand:
                    switch (state.Direction) {
                        case Directions.East:
                            newState.x = state.x + moveCommand.Distance;
                            break;
                        case Directions.West:
                            newState.x = state.x - moveCommand.Distance;
                            break;
                        case Directions.North:
                            newState.y = state.y + moveCommand.Distance;
                            break;
                        case Directions.South:
                            newState.y = state.y - moveCommand.Distance;
                            break;
                    }
                    break;
                case TurnCommand turnCommand:
                    newState.Direction = state.Direction.turn(turnCommand.TurnDirection);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(command));
            }
            return newState;
        });
    }
}