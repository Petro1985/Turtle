namespace ConsoleApp3;

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