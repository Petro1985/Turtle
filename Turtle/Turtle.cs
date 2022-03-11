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
                X = state.X,
                Y = state.Y
            };
             switch (command) {
                case MoveCommand moveCommand:
                    switch (state.Direction) {
                        case Directions.East:
                            newState.X = state.X + moveCommand.Distance;
                            break;
                        case Directions.West:
                            newState.X = state.X - moveCommand.Distance;
                            break;
                        case Directions.North:
                            newState.Y = state.Y + moveCommand.Distance;
                            break;
                        case Directions.South:
                            newState.Y = state.Y - moveCommand.Distance;
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