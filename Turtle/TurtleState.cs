namespace Turtle;

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

    public override string ToString()
    {
        return $"X = {X}; Y = {Y}\nDirection = {Direction}\nCurrent \"SoulState\" is {SoulState}";
    }
};