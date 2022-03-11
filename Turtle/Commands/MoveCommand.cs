using System.Runtime.CompilerServices;
using TheTurtle;

namespace ConsoleApp3.Commands;

class MoveCommand : CommandBase {
    static MoveCommand() {
        CrushHandlers.Add(typeof(Wall), CrushIntoWall);
        CrushHandlers.Add(typeof(Emptiness), CrushIntoEmptiness);
    }

    private static TurtleState CrushIntoEmptiness(TurtleState ts) {
        return ts;
        //throw new NotImplementedException();
    }

    private static TurtleState CrushIntoWall(TurtleState ts) {
        return MoveForward(ts, -1);
        //throw new NotImplementedException();
    }


    public MoveCommand(int distance) {
        Distance = distance;
    }
    public int Distance { get; }

    private static Dictionary<Type, Func<TurtleState, TurtleState>> CrushHandlers = new ();

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
            var crushHandler = CrushHandlers[whatIsThere.GetType()];
            newState = crushHandler(newState);
        }
        return newState;
    }
}