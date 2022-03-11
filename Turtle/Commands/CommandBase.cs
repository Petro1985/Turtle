using TheTurtle;

namespace ConsoleApp3.Commands {

    abstract class CommandBase {
        public abstract TurtleState ApplyCommand(TurtleState ts);
    }
}
