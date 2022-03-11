namespace TheTurtle;

public struct TurtleState {
    public Directions Direction;
    public int X;
    public int Y;

    public TurtleState(Directions direction = Directions.North, int x = 0, int y = 0) {
        Direction = direction;
        this.X = x;
        this.Y = y;
    }

    public override string ToString() {
        return $"x = {X}\ny = {Y}\nDirection: {Enum.GetName(Direction)}";
    }
}