namespace TurtleApp.Maps;

public class Wall : MapObject {
    public Wall(int x, int y) : base(x, y) {
    }

    public override string Name { get; } = "Wall";
}