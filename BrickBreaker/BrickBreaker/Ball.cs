using Microsoft.Graphics.Canvas;
using System;
using Windows.UI;

namespace BrickBreaker
{
    public class Ball : IDrawable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Radius { get; set; }

        public Color Color { get; set; }
        public bool TravelingDownward { get; set; }
        public bool TravelingLeftward { get; set; }

        public Ball(int x, int y, Color color, int radius = 8)
        {
            X = x;
            Y = y;
            Radius = radius;
            Color = color;
        }

        public void Update(int speed)
        {
            if (TravelingDownward)
            {
                Y += speed;
            }
            else
            {
                Y -= speed;
            }
            if (TravelingLeftward)
            {
                X -= speed;
            }
            else
            {
                X += speed;
            }
        }

        public void ChangeColorRandomly()
        {
            Random random = new Random();
            Color = Color.FromArgb(255, (byte)random.Next(0, 256), (byte)random.Next(0, 256), (byte)random.Next(0, 256));
        }

        public void DrawAsync(CanvasDrawingSession canvas, CanvasSpriteBatch iconSpriteBatch)
        {
            canvas.DrawEllipse(X, Y, Radius, Radius, Colors.Black);
            canvas.FillEllipse(X, Y, Radius, Radius, Color);
        }
    }
}
