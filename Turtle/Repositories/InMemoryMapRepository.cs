namespace TurtleApp.Repositories;

public class InMemoryMapRepository : IMapRepository {
    private Dictionary<int, Maps.Map> Storage = new();
    private int Count = 0;
    public Maps.Map? Get(int id) {
        if (Storage.TryGetValue(id, out var map)) {
            return map;
        }
        return null;
    }

    public int Add(Maps.Map map) {
        var id = Count;
        typeof(Maps.Map).GetProperty(nameof(Maps.Map.Id))!.SetValue(map, id);
        Storage.Add(id, map);
        Count++;
        return id;
    }

    public List<Maps.Map> GetAll() {
        return Storage.Values.ToList();
    }
}