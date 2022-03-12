using FluentAssertions;
using Microsoft.VisualBasic;
using TheTurtle;
using Xunit;
namespace Tests; 

public class Repository {
    [Fact]
    public void RepositoryTest() {

        var MapRepository = new InMemoryMapRepository();
        
        var map = new Map {
            Name = "Super map!!!"
        };

        var id = MapRepository.Add(map);
        var map2 = MapRepository.Get(id);

        map.Should().BeEquivalentTo(map2, options => options.Using(Map.StructureComparer));
    }

}