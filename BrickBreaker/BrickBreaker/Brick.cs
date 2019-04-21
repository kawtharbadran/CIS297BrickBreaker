using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System;
using Windows.UI;

namespace BrickBreaker
{
    public class Brick : Paddle, IDestroyable
    {
        //Add bools for power-ups and downgrades
        //Also add getters and setters, and set up in constructor random generator for the bools

        private bool extraBall, widerPaddle;//Power-Ups
        private bool fasterBall, shorterPaddle;//downgrades
        private Random random;
        int scoreValue;
        public int score { get => scoreValue; set => score = scoreValue; }

        public Brick(int x, int y, int width, int height, Color color) : base(x, y, width, height, color)
        {
            random = new Random();
            int rand = random.Next(0, 20);
            Color = color;
            scoreValue = 0;

            switch (rand)
            {
                case 0://extra ball upgrade
                    {
                        extraBall = true;
                        widerPaddle = false;
                        fasterBall = false;
                        shorterPaddle = false;
                    }
                    break;

                case 4://faster ball downgrade
                    {
                        extraBall = false;
                        widerPaddle = false;
                        fasterBall = true;
                        shorterPaddle = false;
                    }
                    break;
                case 9://wider paddle upgrade
                    {
                        extraBall = false;
                        widerPaddle = true;
                        fasterBall = false;
                        shorterPaddle = false;
                    }
                    break;
                case 14://shorter paddle downgrade
                    {
                        extraBall = false;
                        widerPaddle = false;
                        fasterBall = false;
                        shorterPaddle = true;
                    }
                    break;
            }
             
        }
        public bool getExtraBall()
        {
            return extraBall;
        }

        public bool getWiderPaddle()
        {
            return widerPaddle;
        }

        public bool getFasterBall()
        {
            return fasterBall;
        }

        public bool getShorterPaddle()
        {
            return shorterPaddle;
        }
        public override void DrawAsync(CanvasDrawingSession canvas)
        {
            canvas.DrawRectangle(X, Y, Width, Height, Color);
            Color secondColor;
            if (Color == Colors.Red)
            {
                secondColor = Colors.LightPink;
                scoreValue = 1;
            }
            else if (Color == Colors.DodgerBlue)
            {
                secondColor = Colors.LightBlue;
                scoreValue = 2;
            }
            else if (Color == Colors.Yellow)
            {
                secondColor = Colors.LightYellow;
                scoreValue = 3;
            }
            else if (Color == Colors.LimeGreen)
            {
                secondColor = Colors.LightGreen;
                scoreValue = 4;
            }
            else if (Color == Colors.Purple)
            {
                secondColor = Colors.Orchid;
                scoreValue = 5;
            }
            CanvasLinearGradientBrush gradientBrush = new CanvasLinearGradientBrush(canvas, secondColor, Color)
            {
                StartPoint = new System.Numerics.Vector2(X, Y),
                EndPoint = new System.Numerics.Vector2(X, Y + (Height / 2)),
            };
            canvas.FillRectangle(X, Y, Width, Height, gradientBrush);
        }
    }
}
