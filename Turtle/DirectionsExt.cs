namespace Turtle;

public static class DirectionsExt {
    public static Directions turn(this Directions turnFrom, TurnDirections t) {
        return (Directions)((4 + (int)turnFrom + (int)t) % 4);
    }
}