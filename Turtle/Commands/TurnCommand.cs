using TheTurtle;

namespace ConsoleApp3.Commands;

class TurnCommand : CommandBase {
    public TurnCommand(TurnDirections turnDirection) {
        TurnDirection = turnDirection;
    }
    public TurnDirections TurnDirection { get; }
    
    public override TurtleState ApplyCommand(TurtleState ts) {
        var newState = new TurtleState(ts);
        newState.Direction = ts.Direction.turn(TurnDirection);
        return newState;
        //throw new NotImplementedException();
    }

}