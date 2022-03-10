using System;
using System.Collections;
using TheTurtle;
using Xunit;

namespace Tests; 

public class Movement {
    [Fact]
    public void SomeTest() {
        // Arrange
        var t = new Turtle();
        
        // Act
        t.MoveForward(10);
        t.Turn(TurnDirections.Right);
        t.MoveForward(10);
        t.Turn(TurnDirections.Right);
        t.MoveForward(20);
        t.Turn(TurnDirections.Right);
        t.MoveForward(20);
        var state = t.whatIsMyState();
        
        // Assert
        Assert.Equal(state, new TurtleState {
            Direction = Directions.East,
            x = 10,
            y = -10
        });
    }
}