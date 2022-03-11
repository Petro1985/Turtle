namespace TheTurtle;

public struct TurtleState {
    public Directions Direction;
    public int X;
    public int Y;
    public Map Map;
    
    public TurtleState(Directions direction = Directions.North, int x = 0, int y = 0, Map? map = null) {
        Direction = direction;
        this.X = x;
        this.Y = y;
        this.Map = map ?? new Map();
    }

    public TurtleState(TurtleState ts) {
        Direction = ts.Direction;
        this.X = ts.X;
        this.Y = ts.Y;
        this.Map = ts.Map;
    }

    public override string ToString() {
        return $"x = {X}\ny = {Y}\nDirection: {Enum.GetName(Direction)}";
    }
}