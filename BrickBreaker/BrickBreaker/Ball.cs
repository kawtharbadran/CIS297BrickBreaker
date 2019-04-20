﻿using Microsoft.Graphics.Canvas;
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

        public Ball(int x, int y, Color color, int radius = 7)
        {
            X = x;
            Y = y;
            Radius = radius;
            Color = color;
        }

        public void Update()
        {
            if (TravelingDownward)
            {
                Y += 2;
            }
            else
            {
                Y -= 2;
            }
            if (TravelingLeftward)
            {
                X -= 2;
            }
            else
            {
                X += 2;
            }
        }

        public void ChangeColorRandomly()
        {
            Random random = new Random();
            Color = Color.FromArgb(255, (byte)random.Next(0, 256), (byte)random.Next(0, 256), (byte)random.Next(0, 256));
        }

        public void DrawAsync(CanvasDrawingSession canvas)
        {
            canvas.DrawEllipse(X, Y, Radius, Radius, Colors.Orchid);
            canvas.FillEllipse(X, Y, Radius, Radius, Colors.LavenderBlush);
        }
    }
}
