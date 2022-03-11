using System;
using System.Collections;
using TheTurtle;
using Xunit;

namespace Tests; 

public class Movement {
    //       __
    //      |  |
    //      O  |
    //         |
    //  O-------
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
        var expectedState = new TurtleState {
            Direction = Directions.West,
            X = -10,
            Y = -10
        };
        Assert.Equal(expectedState,state );
    }
}