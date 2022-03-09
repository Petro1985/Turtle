namespace TheTurtle;

class TurnCommand : CommandBase {
    public TurnCommand(TurnDirections turnDirection) {
        TurnDirection = turnDirection;
    }
    public TurnDirections TurnDirection { get; }
}