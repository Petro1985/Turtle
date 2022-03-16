namespace TurtleApp;

public enum SoulState {
    Pedestrian,
    CarDriver,
    Helicopter,
}

public record struct TurtleState(
    Directions Direction,
    int X,
    int Y,
    SoulState SoulState,
    Maps.Map Map
) {
    public TurtleState(Maps.Map? map = null) : this(
        Directions.North
        , 0
        , 0
        , SoulState.Pedestrian
        , map ?? new Maps.Map()
    ) {
    }
};


public static class TurtleStateExt
{
    public static TurtleState MoveForward(this TurtleState ts, int steps = 1) {
        ts = ts.Direction switch {
            Directions.North => ts with { Y = ts.Y + steps },
            Directions.East => ts with { X = ts.X + steps },
            Directions.South => ts with { Y = ts.Y - steps },
            Directions.West => ts with { X = ts.X - steps },
            _ => throw new ArgumentOutOfRangeException()
        };
        return ts;
    }
}

