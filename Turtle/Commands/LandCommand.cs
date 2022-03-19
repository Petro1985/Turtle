namespace Turtle.Commands; 

class LandCommand : CommandBase {
    public override TurtleState ApplyCommand(TurtleState ts) {
        return ts with {SoulState = SoulState.Pedestrian};
    }
}