using Microsoft.Graphics.Canvas;
using Windows.UI;

namespace BrickBreaker
{
    public class Wall : IDrawable, ICollidable
    {
        public static int WIDTH = 3;
        public int X0 { get; set; }
        public int Y0 { get; set; }
        public int X1 { get; set; }

        public int Y1 { get; set; }

        public Color Color { get; set; }

        public Wall(int x0, int y0, int x1, int y1, Color color)
        {
            X0 = x0;
            Y0 = y0;
            X1 = x1;
            Y1 = y1;
            Color = color;
        }

        public void DrawAsync(CanvasDrawingSession canvas, CanvasSpriteBatch batch)
        {
            canvas.DrawLine(X0, Y0, X1, Y1, Color, WIDTH);
        }

        public bool CollidesLeftEdge(int x, int y)
        {
            return x == X0 && y >= Y0 && y <= Y1;
        }

        public bool ColllidesRightEdge(int x, int y)
        {
            return x == X1 + WIDTH && y >= Y0 && y <= Y1;
        }

        public bool CollidesTopEdge(int x, int y)
        {
            return x >= X0 && x <= X1 && y == Y1;
        }

        public bool CoolidesBottomEdge(int x, int y)
        {
            return x >= X0 && x <= X1 && y + WIDTH == Y0;
        }
    }
}
