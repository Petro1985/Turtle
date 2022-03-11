﻿namespace TheTurtle;

public struct TurtleState {
    public Directions Direction;
    public int X;
    public int Y;

    public TurtleState(Directions direction = Directions.North, int x = 0, int y = 0) {
        Direction = direction;
        this.X = x;
        this.Y = y;
    }

    public TurtleState(TurtleState ts) {
        Direction = ts.Direction;
        this.X = ts.X;
        this.Y = ts.Y;
    }

    public override string ToString() {
        return $"x = {X}\ny = {Y}\nDirection: {Enum.GetName(Direction)}";
    }
}