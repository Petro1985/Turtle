using System.Diagnostics.Metrics;
using System.Runtime.Serialization;
using System.Text;

namespace TheTurtle;


public abstract class MapObject {
    public int X { get; }
    public int Y { get; }

    public abstract string Name { get; }
    
    protected MapObject(int x, int y) {
        X = x;
        Y = y;
    }
}

public class Wall : MapObject {
    public Wall(int x, int y) : base(x, y) {
    }

    public override string Name { get; } = "Wall";
}

public class Emptiness : MapObject {
    public Emptiness(int x, int y) : base(x, y) {
    }
    public override string Name { get; } = "Nothing";
}
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

public interface IMapRepository {
    Map? Get(int id);
    int Add(Map map);
    List<Map> GetAll();
}

public class InFileMapRepository : IMapRepository {
    //private  Storage 
    public Map? Get(int id) {
        return null;
        //throw new NotImplementedException();
    }

    private string CreateString(Map map) {
        var str = new StringBuilder();

        str.AppendLine(map.Id.ToString());
        str.AppendLine(map.Name);
        var obj = map.GetAllObjects();
        
        foreach (var i in obj) {
            str.Append(i.Name);
            str.Append(';');
            str.Append(i.X);
            str.Append(';');
            str.Append(i.Y);
        }

        return str.ToString();
    }
    
    private Map MapFromString(string input) {
        var map = new Map();

        var str = input.Split('\n');

        typeof(Map).GetProperty(nameof(Map.Id))!.SetValue(map, int.Parse(str[0]));

        for (var i = 2; i < str.Length; i++) {
            MapObject mapObj;
            var tmpStr = str[i].Split(';');
            mapObj = tmpStr[0] switch {
                "Wall" => new Wall(int.Parse(tmpStr[1]), int.Parse(tmpStr[2])),
                _ => throw new Exception("Something went wrong! Hands are up!")
            };

            map.Add(mapObj);

        }

        return map;
    }
    
    public int Add(Map map) {
        return 0;
        //throw new NotImplementedException();
    }

    public List<Map> GetAll() {
        throw new NotImplementedException();
    }
}


public class InMemoryMapRepository : IMapRepository {
    private Dictionary<int, Map> Storage = new();
    private int Count = 0;
    public Map? Get(int id) {
        if (Storage.TryGetValue(id, out var map)) {
            return map;
        }
        return null;
    }

    public int Add(Map map) {
        var id = Count;
        typeof(Map).GetProperty(nameof(Map.Id))!.SetValue(map, id);
        Storage.Add(id, map);
        Count++;
        return id;
    }

    public List<Map> GetAll() {
        return Storage.Values.ToList();
    }
}