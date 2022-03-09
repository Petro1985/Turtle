namespace TheTurtle;

class MoveCommand : CommandBase {
    public MoveCommand(int distance) {
        Distance = distance;
    }
    public int Distance { get; }
}