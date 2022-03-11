using TheTurtle;

namespace ConsoleApp3.Commands;

class MoveCommand : CommandBase {
    public MoveCommand(int distance) {
        Distance = distance;
    }
    public int Distance { get; }

    public override TurtleState ApplyCommand(TurtleState ts) {
        var newState = new TurtleState(ts);
        switch (ts.Direction) {
            case Directions.East:
                newState.X += Distance;
                break;
            case Directions.West:
                newState.X -= Distance;
                break;
            case Directions.North:
                newState.Y += Distance;
                break;
            case Directions.South:
                newState.Y -= Distance;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return newState;
    }
}