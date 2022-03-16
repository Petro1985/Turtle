using System.ComponentModel.DataAnnotations;
using TurtleApp;
using TurtleApp.Maps;
using Xunit;

namespace Tests; 

public class CheckWithMap
{
    [Fact]
    void CrushingIntoWall() {
        var map = new Map();
        
        map.Add(new Wall(0, 5));
    
        var turtle = new Turtle(map);

        turtle.MoveForward(10);

        var expectedState = new TurtleState(Directions.North, 0, 4, SoulState.Pedestrian, map);
        
        Assert.Equal(expectedState, turtle.WhatIsMyState());
    }

    [Fact]
    void FlyOverWall () {
        var map = new Map();
        
        map.Add(new Wall(0, -5));
    
        var turtle = new Turtle(map);

        turtle.Turn(TurnDirections.Back);
        turtle.TakeOff();
        turtle.MoveForward(10);

        var expectedState = new TurtleState(Directions.South, 0, -10, SoulState.Helicopter, map);
        
        Assert.Equal(expectedState, turtle.WhatIsMyState());
        
    } 
}

