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
        
        if (ts.SoulState == SoulState.Helicopter) {
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
        ts = ts.Direction switch {
            Directions.North => ts with { Y = ts.Y + steps },
            Directions.East => ts with { X = ts.X + steps },
            Directions.South => ts with { Y = ts.Y - steps },
            Directions.West => ts with { X = ts.X - steps },
            _ => throw new ArgumentOutOfRangeException()
        };
        return ts;
    }
    
    public override TurtleState ApplyCommand(TurtleState ts) {
        for (int i = 1; i <= Distance; i++) {
            ts = MoveForward(ts);
            var whatIsThere = ts.Map.WhatIsThere(ts.X, ts.Y);
            ts = CrushHandlers.Aggregate(ts, 
                (current, crushHandler) => crushHandler(current, whatIsThere)
            );
        }
        return ts;
    }
}