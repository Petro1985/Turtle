﻿namespace TurtleApp.Maps;

public class Emptiness : MapObject {
    public Emptiness(int x, int y) : base(x, y) {
    }
    public override string Name { get; } = "Nothing";
}