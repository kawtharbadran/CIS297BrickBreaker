using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Gaming.Input;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace BrickBreaker
{
    public interface IDrawable
    {
        void DrawAsync(CanvasDrawingSession canvas);
    }

    public interface ICollidable
    {
        bool CollidesLeftEdge(int x, int y);
        bool ColllidesRightEdge(int x, int y);
        bool CollidesTopEdge(int x, int y);
        bool CoolidesBottomEdge(int x, int y);
    }

    public interface IDestroyable : ICollidable
    { }

    public class Pong
    {
        public static int LEFT_EDGE = 10;
        public static int TOP_EDGE = 5;
        public static int RIGHT_EDGE = 790;
        public static int BOTTOM_EDGE = 450;

        private Random random;

        private Ball ball;
        private Paddle PlayerPaddle;
        private List<IDrawable> drawables;
        public bool gameOver;
        private Gamepad controller;

        private bool isPowerUpGoing;
        private Stopwatch powerUpTime;
        private int whichPowerUp;

        public int score;

        public Pong()
        {
            drawables = new List<IDrawable>();

            ball = new Ball(100, 100, Colors.Gray);
            drawables.Add(ball);

            isPowerUpGoing = false;
            powerUpTime = new Stopwatch();
            whichPowerUp = 0;

            drawBricks();

            ball.TravelingDownward = true;
            ball.TravelingLeftward = true;

            var leftWall = new Wall(LEFT_EDGE, TOP_EDGE, LEFT_EDGE, BOTTOM_EDGE, Colors.White);
            drawables.Add(leftWall);

            var rightWall = new Wall(RIGHT_EDGE, TOP_EDGE, RIGHT_EDGE, BOTTOM_EDGE, Colors.White);
            drawables.Add(rightWall);

            var topWall = new Wall(LEFT_EDGE - Wall.WIDTH, TOP_EDGE, RIGHT_EDGE + Wall.WIDTH, TOP_EDGE, Colors.Black);
            drawables.Add(topWall);

            PlayerPaddle = new Paddle(LEFT_EDGE + RIGHT_EDGE / 2, BOTTOM_EDGE, 60, 10, Colors.White);
            drawables.Add(PlayerPaddle);

            score = 0;
            gameOver = false;

            random = new Random();
        }

        public void drawBricks()
        {
            Stack<Color> colorStack = new Stack<Color>();
            List<Color> ourColors = new List<Color> {
                Colors.Red,
                Colors.Yellow,
                Colors.LimeGreen,
                Colors.DodgerBlue,
                Colors.Purple };

            int brickWidth = 40;
            int brickHeight = 20;
            int leftPoint = LEFT_EDGE + 10;
            int topPoint = TOP_EDGE + 10;
            int colorIndex = 0;
            for (int bricksInRow = 19; bricksInRow >= 0; bricksInRow -= 2)
            {
                leftPoint = LEFT_EDGE + 10 + (brickWidth * ((19 - bricksInRow) / 2));
                colorIndex = 0;

                for (int brick = 0; brick < bricksInRow; brick++)
                {
                    if (brick > (bricksInRow / 2))
                    {
                        if (colorIndex == 0) { colorIndex = ourColors.Count() - 1; }
                        var obstacle = new Brick(leftPoint, topPoint, brickWidth, brickHeight, colorStack.Pop());
                        drawables.Add(obstacle);
                    }
                    else
                    {
                        if (colorIndex == ourColors.Count()) { colorIndex = 0; }
                        var obstacle = new Brick(leftPoint, topPoint, brickWidth, brickHeight, ourColors[colorIndex]);
                        colorStack.Push(ourColors[colorIndex]);
                        colorIndex++;
                        if (brick == (bricksInRow / 2))
                        {
                            colorStack.Pop();
                        }
                        drawables.Add(obstacle);

                    }
                    leftPoint += brickWidth;
                }
                topPoint += brickHeight;
            }
        }

        public void SetPaddleTravelingLeftward(bool travelingLeftward)
        {
            PlayerPaddle.TravelingLeftward = travelingLeftward;
        }
        public void SetPaddleTravelingRightward(bool travelingRightward)
        {
            PlayerPaddle.TravelingRightward = travelingRightward;
        }

        public bool Update()
        {
            bool bounced = false;

            if (Gamepad.Gamepads.Count > 0)
            {
                controller = Gamepad.Gamepads.First();
                var reading = controller.GetCurrentReading();
                PlayerPaddle.X += (int)(reading.LeftThumbstickX * 5);
                PlayerPaddle.Y += (int)(reading.LeftThumbstickY * -5);
            }

            if (!gameOver)
            {
                List<IDrawable> bricksToDestroy = new List<IDrawable>();
                foreach (var drawable in drawables)
                {
                    ICollidable colliable = drawable as ICollidable;
                    if (colliable != null)
                    {
                        if (colliable.CoolidesBottomEdge(ball.X, ball.Y))
                        {
                            ball.TravelingDownward = !ball.TravelingDownward;
                            ball.ChangeColorRandomly();
                            bounced = true;
                        }
                        else if (colliable.CollidesTopEdge(ball.X, ball.Y + ball.Radius * 2))
                        {
                            ball.TravelingDownward = !ball.TravelingDownward;
                            ball.ChangeColorRandomly();
                            bounced = true;
                        }
                        else if (colliable.CollidesLeftEdge(ball.X, ball.Y))
                        {
                            ball.TravelingLeftward = !ball.TravelingLeftward;
                            ball.ChangeColorRandomly();
                            bounced = true;
                        }
                        else if (colliable.ColllidesRightEdge(ball.X + ball.Radius * 2, ball.Y))
                        {
                            ball.TravelingLeftward = !ball.TravelingLeftward;
                            ball.ChangeColorRandomly();
                            bounced = true;
                        }

                        if (bounced)
                        {
                            IDestroyable brick = colliable as IDestroyable;

                            if (brick != null)
                            {
                                bricksToDestroy.Add(brick as IDrawable);
                                break;
                            }
                        }
                    }
                }

                 foreach (var brick in bricksToDestroy)
                {
                    score += 1;

                 if (isPowerUpGoing == false)
                    {
                       
                        if ((brick as Brick).getExtraBall())
                        {
                            ExtraBallPowerup(false);
                            isPowerUpGoing = true;
                            powerUpTime.Restart();
                            whichPowerUp = 1;

                        }
                        else if ((brick as Brick).getWiderPaddle())
                        {
                            WiderPaddlePowerup(false);
                            isPowerUpGoing = true;
                            powerUpTime.Restart();
                            whichPowerUp = 2;
                        }
                        else if ((brick as Brick).getFasterBall())
                        {
                            FasterBallDowngrade(false);
                            isPowerUpGoing = true;
                            powerUpTime.Restart();
                            whichPowerUp = 3;
                        }
                        else if ((brick as Brick).getShorterPaddle())
                        {
                            ShorterPaddleDowngrade(false);
                            isPowerUpGoing = true;
                            powerUpTime.Restart();
                            whichPowerUp = 4;
                        }
                    }
                  
                    drawables.Remove(brick);
                   
                }
                if (powerUpTime.ElapsedTicks > 10000)
                {
                    switch (whichPowerUp)
                    {
                        case 1:
                            {
                                ExtraBallPowerup(true);
                            }
                            break;
                        case 2:
                            {
                                WiderPaddlePowerup(true);
                            }
                            break;
                        case 3:
                            {
                                FasterBallDowngrade(true);
                            }
                            break;
                        case 4:
                            {
                                ShorterPaddleDowngrade(true);
                            }
                            break;
                    }

                    powerUpTime.Reset();
                }

                
                ball.Update();
                PlayerPaddle.Update();
                gameOver = ball.Y < TOP_EDGE || ball.Y > BOTTOM_EDGE;
                }
            
            else
            {
                //game over here
            }

            return bounced;
        }

        public void DrawGame(CanvasDrawingSession canvas)
        {
            foreach (var drawable in drawables)
            {
                drawable.DrawAsync(canvas);
            }
        }

        public Color PickRandomColor()
        {
            Random random = new Random();
            int rand = random.Next(0, 5);
            Color color = new Color();
            //List<Colors> colorList = new List<Colors>;
            switch (rand)
            {
                case 0:
                    {
                        color = Colors.DodgerBlue;
                    }
                    break;
                case 1:
                    {
                        color = Colors.Purple;
                    }
                    break;
                case 2:
                    {
                        color = Colors.LimeGreen;
                    }
                    break;
                case 3:
                    {
                        color = Colors.Red;
                    }
                    break;
                case 4:
                    {
                        color = Colors.Yellow;
                    }
                    break;
            }
            return color;
        }

        public void ExtraBallPowerup(bool running)
        {
            if (running)
            {
                //end
            }
            else
            {
                //start
                var newBall = new Ball(100, 100, Colors.Gray);
                drawables.Add(newBall);
                newBall.TravelingDownward = true;
                newBall.TravelingLeftward = false;
            }
        }

        public void WiderPaddlePowerup(bool running)
        {
            if (running)
            {
                //end
            }
            else
            {
                //start
            }
        }

        public void FasterBallDowngrade(bool running)
        {
            if (running)
            {
                //end
            }
            else
            {
                //start
            }
        }
        public void ShorterPaddleDowngrade(bool running)
        {
            if (running)
            {
                //end
            }
            else
            {
                //start
            }
        }

    }
}




