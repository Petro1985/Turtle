namespace TurtleApp.Maps;

public class Map {
    private sealed class StructureEqualityComparer : IEqualityComparer<Map> {
        public bool Equals(Map x, Map y) {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;

            if (x.MapObjects.Count != y.MapObjects.Count) return false;
            
            foreach (var i in x.MapObjects) {
                if (!y.MapObjects.TryGetValue(i.Key, out var value)) {
                    return false;
                }
            }
            
            return x.Id == y.Id && x.Name == y.Name;
        }

        public int GetHashCode(Map obj) {
            return HashCode.Combine(obj.MapObjects, obj.Id, obj.Name);
        }
    }

    public static IEqualityComparer<Map> StructureComparer { get; } = new StructureEqualityComparer();

    public int Id { get; private init; }
    public string Name { get; init; }

    private Dictionary<(int, int), MapObject> MapObjects = new ();
    //private ICollection<MapObject> MapObjects = new List<MapObject>();

    public IEnumerable<MapObject> GetAllObjects() {
        return MapObjects.Values;
    }
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