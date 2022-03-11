using System.Runtime.CompilerServices;
using TheTurtle;

namespace ConsoleApp3.Commands;

class MoveCommand : CommandBase {
    static MoveCommand() {
        CrushHandlers = new List<Func<TurtleState, MapObject, TurtleState>> {
            CrushIntoWallHandler
        };
    }

    private static TurtleState CrushIntoWallHandler(TurtleState ts, MapObject obj) {
        if (obj is not Wall) {
            return ts;
        }
        return MoveForward(ts, -1);
    }


    public MoveCommand(int distance) {
        Distance = distance;
    }
    public int Distance { get; }

    private static IEnumerable<Func<TurtleState, MapObject, TurtleState>> CrushHandlers;
    //private static Dictionary<Type, Func<TurtleState, TurtleState>> CrushHandlers = new ();

    private static TurtleState MoveForward(TurtleState ts, int steps = 1) {
        var newState = new TurtleState(ts);
        switch (ts.Direction) {
            case Directions.North:
                newState.Y = newState.Y + steps;
                break;
            case Directions.East:
                newState.X = newState.X + steps;
                break;
            case Directions.South:
                newState.Y = newState.Y - steps;
                break;
            case Directions.West:
                newState.X = newState.X - steps;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return newState;
    }
    
    public override TurtleState ApplyCommand(TurtleState ts) {
        var newState = new TurtleState(ts);
        for (int i = 1; i <= Distance; i++) {
            newState = MoveForward(newState);
            var whatIsThere = newState.Map.WhatIsThere(newState.X, newState.Y);
            newState = CrushHandlers.Aggregate(newState, 
                (current, crushHandler) => crushHandler(current, whatIsThere)
            );
        }
        return newState;
    }
}