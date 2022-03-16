using TurtleApp.Maps;

namespace TurtleApp.Handlers;

public abstract class CrushHandlerBase
{
    protected abstract bool CanHandle(TurtleState ts, MapObject obj);
    protected abstract TurtleState HandleCore(TurtleState ts, MapObject obj);

    public TurtleState Handle(TurtleState ts, MapObject obj)
    {
        if (CanHandle(ts, obj))
        {
            return HandleCore(ts, obj);
        }

        return ts;
    }
}


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