namespace Turtle.Maps;

public abstract class MapObject {
    public int X { get; }
    public int Y { get; }

    public abstract string Name { get; }
    
    protected MapObject(int x, int y) {
        X = x;
        Y = y;
    }
}