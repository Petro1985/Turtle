namespace TheTurtle;

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
    Map Map
) {
    public TurtleState(Map? map = null) : this(
        Directions.North
        , 0
        , 0
        , SoulState.Pedestrian
        , map ?? new Map()
    ) {
    }
};