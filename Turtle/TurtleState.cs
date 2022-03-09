namespace TheTurtle;

public struct TurtleState {
    public Directions Direction = Directions.North;
    public int x = 0;
    public int y = 0;
    public override string ToString() {
        return $"x = {x}\ny = {y}\nDirection: {Enum.GetName(Direction)}";
    }
}