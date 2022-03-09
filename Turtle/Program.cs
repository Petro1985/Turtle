using System.Collections.Immutable;
using System.Diagnostics;
using System.Security.Cryptography;

namespace TheTurtle;

static class Program {

    public static void Main() {
        var turtle = new Turtle();
        turtle.MoveForward(10);
        turtle.Turn(TurnDirections.Right);
        turtle.MoveForward(10);
        turtle.Turn(TurnDirections.Right);
        turtle.MoveForward(20);
        turtle.Turn(TurnDirections.Right);
        turtle.MoveForward(20);
            
        Console.WriteLine(turtle.whatIsMyState());

        var temp = "asdfklim ,nwer klrjewlksdnfklsdfgh wpthjklsdgn akrs gaekrbtg;rek";

        var dict = temp.Aggregate(ImmutableDictionary<char, int>.Empty, (dict, nextCh) => dict.ContainsKey(nextCh) ? dict.SetItem(nextCh, dict[nextCh] + 1) : dict.Add(nextCh, 1));
        Debugger.Break();
    }
}