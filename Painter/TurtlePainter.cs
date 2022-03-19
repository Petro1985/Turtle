using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Intrinsics.X86;
using Turtle;
using Turtle.Maps;

namespace Turtle.TelegramBot;

public class TurtlePainter
{
    private const int CellSize = 50;
    private Graphics _graphics;
    private Bitmap _bitmap;
    private Dictionary<Type, Bitmap> _resources;

    public TurtlePainter()
    {
    }

    private void LoadImages(TurtleState ts)
    {
        _resources = ts.Map.GetAllObjects()
            .DistinctBy(item => item.GetType())
            .ToDictionary(key => key.GetType(), value =>
            {
                return value switch
                {
                    Emptiness emptiness => new Bitmap("Resources\\emptiness.bmp"),
                    Wall wall => new Bitmap("Resources\\wall.bmp"),
                    _ => throw new ArgumentOutOfRangeException(nameof(value))
                };
            });
        _resources.Add(typeof(global::Turtle.Turtle), 
            ts.Direction switch
            {
                Directions.North => new Bitmap("Resources\\turtle_up.png"),
                Directions.West  => new Bitmap("Resources\\turtle_left.png"),
                Directions.East  => new Bitmap("Resources\\turtle_right.png"),
                Directions.South => new Bitmap("Resources\\turtle_down.png"),
                _ => throw new ArgumentOutOfRangeException()
            }
            );
    }
    
    public Bitmap DrawField(TurtleState ts)
    {
        LoadImages(ts);
        _bitmap = new Bitmap(CellSize * 9, CellSize * 9);
        _graphics = Graphics.FromImage(_bitmap);

        _graphics.FillRectangle(new SolidBrush(Color.Bisque), 0, 0, CellSize * 9-1, CellSize * 9-1);

        for (var i = 0; i < 9; i++)
        {
            _graphics.DrawLine(new Pen(Color.Chocolate), i * CellSize, 0, i * CellSize, CellSize * 9);
            _graphics.DrawLine(new Pen(Color.Chocolate), 0, CellSize*i, CellSize * 9, CellSize * i);
        }

        var observedObj = ts.Map.GetAllObjects()
            .Where(item => 
                            !(item.X < ts.X - 5 
                           || item.X > ts.X + 5 
                           || item.Y < ts.Y - 5 
                           || item.Y > ts.Y + 5));
        
        foreach (var item in observedObj)
        {
            var x = (item.X - ts.X) + 4;
            var y = (ts.Y - item.Y) + 4;

            _graphics.DrawImage(_resources[item.GetType()], x * CellSize, y * CellSize, CellSize, CellSize);
        }
        
        _graphics.DrawImage(_resources[typeof(global::Turtle.Turtle)],4*CellSize, 4*CellSize, CellSize, CellSize);
        
        
        return _bitmap;
    }
    
    
}