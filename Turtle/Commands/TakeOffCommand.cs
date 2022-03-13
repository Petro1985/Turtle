namespace ConsoleApp3.Commands; 

class TakeOffCommand : CommandBase {
    public override TurtleState ApplyCommand(TurtleState ts) {
        return ts with { SoulState = SoulState.Helicopter };
    }
}