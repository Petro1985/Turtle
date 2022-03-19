using Microsoft.Extensions.DependencyInjection;

namespace Turtle;

public class TurtleProvider
{
    private Dictionary<long, Turtle> _turtles;
    private IServiceProvider _serviceProvider;

    public TurtleProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _turtles = new Dictionary<long, Turtle>();
    }

    public Turtle GetTurtle(long chatId)
    {
        if (!_turtles.TryGetValue(chatId, out var turtle))
        {
            turtle = _serviceProvider.GetRequiredService<Turtle>();
            _turtles.Add(chatId, turtle);
        }
        return turtle;
    }
}