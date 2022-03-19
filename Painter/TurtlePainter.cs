using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.Intrinsics.X86;
using Turtle;
using Turtle.Maps;

namespace Turtle.TelegramBot;

public class TurtlePainter
{
    private const int CellSize = 50;
    private const int MergeSize = 25;
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
        _bitmap = new Bitmap(CellSize * 9 + MergeSize * 2, CellSize * 9 + MergeSize * 2);
        _graphics = Graphics.FromImage(_bitmap);

        _graphics.FillRectangle(new SolidBrush(Color.Bisque), 0, 0, _bitmap.Size.Width, _bitmap.Size.Height);

        for (var i = 0; i < 9; i++)
        {
            var strFormatAlignmentRight = new StringFormat();
            var strFormatAlignmentCenter = new StringFormat();
            strFormatAlignmentRight.Alignment = StringAlignment.Far;
            strFormatAlignmentCenter.Alignment = StringAlignment.Center;
            
            _graphics.DrawString(
                (ts.Y + 4 - i).ToString()
                , new Font(FontFamily.GenericMonospace, 10)
                , new SolidBrush(Color.DarkOrchid)
                , new RectangleF(0,CellSize*i+MergeSize+15,25,20)
                , strFormatAlignmentRight 
                );
            
            _graphics.DrawString(
                (ts.X - 4 + i).ToString()
                , new Font(FontFamily.GenericMonospace, 10)
                , new SolidBrush(Color.DarkOrchid)
                , new PointF(CellSize*i+MergeSize+25, 5)
                , strFormatAlignmentCenter 
                );
            
            _graphics.DrawLine(new Pen(Color.Chocolate)
                , i * CellSize + MergeSize, MergeSize
                , i * CellSize + MergeSize, CellSize * 9 + MergeSize);
            _graphics.DrawLine(new Pen(Color.Chocolate)
                , MergeSize, CellSize*i + MergeSize
                , CellSize * 9 + MergeSize, CellSize * i + MergeSize);
        }

        _graphics.DrawLine(new Pen(Color.Chocolate)
            , 9 * CellSize + MergeSize, MergeSize
            , 9 * CellSize + MergeSize, CellSize * 9 + MergeSize);
        
        _graphics.DrawLine(new Pen(Color.Chocolate)
            , MergeSize, CellSize*9 + MergeSize
            , CellSize * 9 + MergeSize, CellSize * 9 + MergeSize);

        var observedObj = ts.Map.GetAllObjects()
            .Where(item => 
                            !(item.X < ts.X - 4 
                           || item.X > ts.X + 4 
                           || item.Y < ts.Y - 4 
                           || item.Y > ts.Y + 4));
        
        foreach (var item in observedObj)
        {
            var x = (item.X - ts.X) + 4;
            var y = (ts.Y - item.Y) + 4;

            _graphics.DrawImage(_resources[item.GetType()], x * CellSize + MergeSize, y * CellSize + MergeSize, CellSize, CellSize);
        }
        
        _graphics.DrawImage(_resources[typeof(global::Turtle.Turtle)],4*CellSize + MergeSize, 4*CellSize + MergeSize, CellSize, CellSize);
        
        return _bitmap;
    }
    
    
}