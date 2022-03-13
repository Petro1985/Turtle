﻿namespace ConsoleApp3.Repositories;
using Maps;

public interface IMapRepository {
    Map? Get(int id);
    int Add(Map map);
    List<Map> GetAll();
}