namespace ConsoleApp3.Commands;

class TurnCommand : CommandBase {
    public TurnCommand(TurnDirections turnDirection) {
        TurnDirection = turnDirection;
    }
    public TurnDirections TurnDirection { get; }
    
    public override TurtleState ApplyCommand(TurtleState ts) {
        return ts with {Direction = ts.Direction.turn(TurnDirection)};
        //throw new NotImplementedException();
    }

}