using System.ComponentModel.DataAnnotations;
using TheTurtle;
using Xunit;

namespace Tests; 

public class CheckWithMap
{
    [Fact]
    void FirstTest() {
        var map = new Map();
        
        map.Add(new Wall(0, 5));
    
        var turtle = new Turtle(map);

        turtle.MoveForward(10000);

        var expectedState = new TurtleState(Directions.North, 0, 4, map);
        
        Assert.Equal(expectedState, turtle.whatIsMyState());
    }
    
}

