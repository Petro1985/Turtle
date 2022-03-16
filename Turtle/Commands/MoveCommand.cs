using System.Runtime.CompilerServices;
using TurtleApp.Maps;
using TurtleApp.Handlers;

namespace TurtleApp.Commands;

class MoveCommand : CommandBase {
    public MoveCommand(int distance, IEnumerable<CrushHandlerBase> crushHandlers) {
        Distance = distance;
        CrushHandlers = crushHandlers;
    }
    public int Distance { get; }

    private IEnumerable<CrushHandlerBase> CrushHandlers;
    //private static Dictionary<Type, Func<TurtleState, TurtleState>> CrushHandlers = new ();

    public override TurtleState ApplyCommand(TurtleState ts) {
        for (int i = 1; i <= Distance; i++) {
            ts = ts.MoveForward();
            var whatIsThere = ts.Map.WhatIsThere(ts.X, ts.Y);
            ts = CrushHandlers.Aggregate(ts, 
                (current, crushHandler) => crushHandler.Handle(current, whatIsThere)
            );
        }
        return ts;
    }
}