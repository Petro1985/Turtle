namespace Turtle.Commands {

    abstract class CommandBase {
        public abstract TurtleState ApplyCommand(TurtleState ts);
    }
}
