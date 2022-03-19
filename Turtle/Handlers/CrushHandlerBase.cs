using Turtle.Maps;

namespace Turtle.Handlers;

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