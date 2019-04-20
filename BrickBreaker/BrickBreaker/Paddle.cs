using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Windows.UI;

namespace BrickBreaker
{
    public class Paddle : IDrawable, ICollidable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Color Color { get; set; }

        public bool TravelingLeftward { get; set; }
        public bool TravelingRightward { get; set; }

        public Paddle(int x, int y, int width, int height, Color color)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Color = color;
            TravelingLeftward = false;
            TravelingRightward = false;
        }

        public void Update()
        {
            if (TravelingRightward)
            {
                X += 3;
            }
            else if (TravelingLeftward)
            {
                X -= 3;
            }
        }

        public virtual void DrawAsync(CanvasDrawingSession canvas)
        {
            canvas.DrawRoundedRectangle(X, Y, Width, Height,3,3, Color);
            Color secondColor;
            if (Color == Colors.Red)
            {
                secondColor = Colors.LightPink;
            }
            else if (Color == Colors.DodgerBlue)
            {
                secondColor = Colors.LightBlue;
            }
            else if (Color == Colors.Yellow)
            {
                secondColor = Colors.LightYellow;
            }
            else if (Color == Colors.LimeGreen)
            {
                secondColor = Colors.LightGreen;
            }
            else if (Color == Colors.Purple)
            {
                secondColor = Colors.Orchid;
            }
            CanvasLinearGradientBrush gradientBrush = new CanvasLinearGradientBrush(canvas, secondColor, Color)
            {
                StartPoint = new System.Numerics.Vector2(X, Y),
                EndPoint = new System.Numerics.Vector2(X, Y + (Height / 2)),
            };
            canvas.FillRoundedRectangle(X, Y, Width, Height,3,3, gradientBrush);
        }

        public bool CollidesLeftEdge(int x, int y)
        {
            return x >= X && x <= X + Width && y >= Y && y <= Y + Height;
        }

        public bool ColllidesRightEdge(int x, int y)
        {
            return x >= X && x <= X + Width && y >= Y && y <= Y + Height;
        }

        public bool CollidesTopEdge(int x, int y)
        {
            return x >= X && x <= X + Width && y >= Y && y <= Y + Height;
        }

        public bool CoolidesBottomEdge(int x, int y)
        {
            return x >= X && x <= X + Width && y >= Y && y <= Y + Height;
        }
    }
}
