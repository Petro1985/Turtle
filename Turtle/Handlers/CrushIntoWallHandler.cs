using Turtle.Maps;

namespace Turtle.Handlers;

public class CrushIntoWallHandler : CrushHandlerBase
{
    protected override bool CanHandle(TurtleState ts, MapObject obj)
    {
        return obj is Wall;
    }

    protected override TurtleState HandleCore (TurtleState ts, MapObject obj)
    {
        if (ts.SoulState == SoulState.Helicopter) {
            return ts;
        }
    
        return ts.MoveForward(-1);
    }
}