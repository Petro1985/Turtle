using ConsoleApp3.Commands;

namespace ConsoleApp3;

public class Turtle {
    private TurtleState _initialState;
    
    private List<CommandBase> _log = new ();
    public void Turn(TurnDirections dir) {
        _log.Add(new TurnCommand(dir));
    }
    public void MoveForward(int distance) {
        _log.Add(new MoveCommand(distance));
    }

    public void TakeOff() {
        _log.Add(new TakeOffCommand());
    }

    public void Land() {
        _log.Add(new LandCommand());
    }

    public TurtleState whatIsMyState() {
        return _log.Aggregate(_initialState, (state, command) => command.ApplyCommand(state));
    }

    public Turtle(TurtleState initialState) {
        _initialState = initialState;
    }
    public Turtle(Maps.Map map) : this(new TurtleState(map: map)) {}

    public Turtle() : this(new TurtleState(map: new Maps.Map())) {}
}