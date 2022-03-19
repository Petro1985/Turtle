using System;
using System.Collections.Generic;
using System.IO;
using Turtle.Maps;
using Turtle.Repositories;
using FluentAssertions;
using Xunit;
namespace Tests; 

public class Repository : IDisposable {
    [Theory]
    [MemberData(nameof(Data))]
    public void RepositoryTest(IMapRepository mapRepository) {
       
        var map = new Map {
            Name = "Super map!!!"
        };

        map.Add(new Wall(2, 3));
        map.Add(new Wall(5, -3));
        
        var id = mapRepository.Add(map);
        var map2 = mapRepository.Get(id);
        
        map.Should().BeEquivalentTo(map2, options => options.Using(Map.StructureComparer));
    }
    public static IEnumerable<object[]> Data()
    {
        return new List<object[]>
        {
            new object[] {new InFileMapRepository()},
            new object[] {new InMemoryMapRepository()},
        };
    }

    // public void Dispose()
    // {
    //     var files = Directory.GetFiles(".", "Map_*");
    //     foreach (var file in files)
    //     {
    //         File.Delete(file);            
    //     }
    // }
}

