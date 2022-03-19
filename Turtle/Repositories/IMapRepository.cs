using Turtle.Maps;

namespace Turtle.Repositories;

public interface IMapRepository {
    Map? Get(int id);
    int Add(Map map);
    List<Map> GetAll();
}