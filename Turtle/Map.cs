namespace TheTurtle;

public abstract class MapObject {
    public int X { get; }
    public int Y { get; }

    protected MapObject(int x, int y) {
        X = x;
        Y = y;
    }
}

public class Wall : MapObject {
    public Wall(int x, int y) : base(x, y) {
    }
}

public class Emptiness : MapObject {
    public Emptiness(int x, int y) : base(x, y) {
    }
}

public class Map {
    private Dictionary<(int, int), MapObject> MapObjects = new ();
    //private ICollection<MapObject> MapObjects = new List<MapObject>();

    public MapObject WhatIsThere(int x, int y) {
        return MapObjects.TryGetValue((x, y), out var mapObject) 
            ? mapObject 
            : new Emptiness(x, y);
    }

    public bool Add(MapObject newObj) {
        if (newObj is Emptiness) return true;
        return MapObjects.TryAdd((newObj.X, newObj.Y), newObj);
    }
}
