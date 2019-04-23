using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace BrickBreaker
{
    class PowerupSprite:IDrawable, IDestroyable
    {
        private Pong pong;
        private Rect iconSpriteSource;
        public int X;
        public int Y;
        public static int Height = 35;
        public static int Width = 35;
        private int numberType;

        public PowerupSprite(Pong pong, int number, int X, int Y)
        {
            this.pong = pong;
            this.X = X;
            this.Y = Y;
            numberType = number;

            switch (numberType)
            {
                case 1:
                    iconSpriteSource = new Rect(SpriteSheet.anotherBallLocation, SpriteSheet.iconSize);
                    break;
                case 2:
                    iconSpriteSource = new Rect(SpriteSheet.longerPaddleLocation, SpriteSheet.iconSize);
                    break;
                case 3:
                    iconSpriteSource = new Rect(SpriteSheet.fasterBallLocation, SpriteSheet.iconSize);
                    break;
                case 4:
                    iconSpriteSource = new Rect(SpriteSheet.shorterPaddleLocation, SpriteSheet.iconSize);
                    break;
                default:
                    iconSpriteSource = new Rect(0, 0, 0, 0);
                    break;
            }
        }

        public int score { get => score; set => score = 0; }
       

        public void Update()
        {
            Y += 1;
        }

        public void DrawAsync(CanvasDrawingSession canvas, CanvasSpriteBatch iconSpriteBatch)
        {
            iconSpriteBatch.DrawFromSpriteSheet(pong.SpriteSheet,
                        new Rect(X, Y, Width, Height), iconSpriteSource);
        }

        public bool CollidesLeftEdge(int x, int y)
        {
            throw new NotImplementedException();
        }

        public bool ColllidesRightEdge(int x, int y)
        {
            throw new NotImplementedException();
        }

        public bool CollidesTopEdge(int x, int y)
        {
            throw new NotImplementedException();
        }

        public bool CoolidesBottomEdge(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}