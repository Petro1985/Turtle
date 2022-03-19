using Turtle;
using Turtle.Maps;
using FluentAssertions;
using Xunit;

namespace Tests; 

public class Movement {
    //       __
    //      |  |
    //      O  |
    //         |
    //  O-------
    [Fact]
    public void ClockwiseMovement() {
        // Arrange
        var map = new Map();
        var t = new Turtle(map);
        
        // Act
        t.MoveForward(10);
        t.Turn(TurnDirections.Right);
        t.MoveForward(10);
        t.Turn(TurnDirections.Right);
        t.MoveForward(20);
        t.Turn(TurnDirections.Right);
        t.MoveForward(20);
        var state = t.WhatIsMyState();
        
        // Assert
        var expectedState = new TurtleState {
            Direction = Directions.West,
            X = -10,
            Y = -10,
            Map = map,
        };
        Assert.Equal(expectedState,state );
    }
    [Fact]
    public void AllDirectionTurn() {
        // Arrange
        var t = new Turtle();
        // Act
        t.MoveForward(15);
        t.Turn(TurnDirections.Left);
        t.MoveForward(10);
        t.Turn(TurnDirections.Back);
        t.MoveForward(30);
        t.Turn(TurnDirections.Right);
        t.MoveForward(5);
        var state = t.WhatIsMyState();
        
        // Assert
        var expectedState = new TurtleState {
            Direction = Directions.South,
            X = 20,
            Y = 10,
        };
        state.Should().BeEquivalentTo(expectedState, options => options
            .Excluding(turtleState => turtleState.Map)
            .ComparingByMembers(typeof(TurtleState))
        );

    }
}