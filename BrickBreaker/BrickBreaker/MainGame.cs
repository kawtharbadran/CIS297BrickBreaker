using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Gaming.Input;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using System.Runtime.InteropServices.WindowsRuntime;

namespace BrickBreaker
{
    public interface IDrawable
    {
        void DrawAsync(CanvasDrawingSession canvas, CanvasSpriteBatch iconSpriteSource);
    }

    public interface ICollidable
    {
        bool CollidesLeftEdge(int x, int y);
        bool ColllidesRightEdge(int x, int y);
        bool CollidesTopEdge(int x, int y);
        bool CoolidesBottomEdge(int x, int y);
    }

    public interface IDestroyable : ICollidable
    {
        int score { get; set;}
    }

    public class Pong
    {
        public static int LEFT_EDGE = 10;
        public static int TOP_EDGE = 10;
        public static int RIGHT_EDGE = 790;
        public static int BOTTOM_EDGE = 530;

        private Random random;

        private Ball ball, extraBall;
        private Paddle PlayerPaddle;
        private List<IDrawable> drawables;
        public bool gameOver;
        private Gamepad controller;

        private bool isPowerUpGoing;
        private Stopwatch powerUpTime;
        private TimeSpan currentTimeStamp;
        private int whichPowerUp;
        private PowerupSprite powerupIcon;

        public int score;
        private CanvasBitmap spriteSheet;
        public CanvasBitmap SpriteSheet { get => spriteSheet; set => spriteSheet = value; }

        public Pong(CanvasBitmap SpriteSheet)
        {
            spriteSheet = SpriteSheet;
            drawables = new List<IDrawable>();

            ball = new Ball(100, 100, Colors.Gray);
            drawables.Add(ball);

            extraBall = new Ball(100, 100, Colors.Blue);
            extraBall.TravelingDownward = true;
            extraBall.TravelingLeftward = true;

            isPowerUpGoing = false;
            powerUpTime = Stopwatch.StartNew();
            currentTimeStamp = powerUpTime.Elapsed;
            whichPowerUp = 0;

            drawBricks();

            ball.TravelingDownward = true;
            ball.TravelingLeftward = true;

            var leftWall = new Wall(LEFT_EDGE, TOP_EDGE, LEFT_EDGE, BOTTOM_EDGE, Colors.White);
            drawables.Add(leftWall);

            var rightWall = new Wall(RIGHT_EDGE, TOP_EDGE, RIGHT_EDGE, BOTTOM_EDGE, Colors.White);
            drawables.Add(rightWall);

            var topWall = new Wall(LEFT_EDGE - Wall.WIDTH-leftWall.X0, TOP_EDGE, RIGHT_EDGE + Wall.WIDTH, TOP_EDGE, Colors.White);
            drawables.Add(topWall);

            PlayerPaddle = new Paddle(LEFT_EDGE + RIGHT_EDGE / 2, BOTTOM_EDGE - 20, 60, 10, Colors.White);
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
                List<IDrawable> powerupsCaught = new List<IDrawable>();
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
                        else if (isPowerUpGoing && whichPowerUp == 1)//Collision for extra ball
                        {
                            if (ExtraBallChangeDirection(colliable)) { bounced = true; }
                        }
                        if (isPowerUpGoing && (colliable is Paddle)&& !(colliable is Brick ) && colliable.CollidesTopEdge(powerupIcon.X, powerupIcon.Y + 35))
                        {
                            powerupsCaught.Add(powerupIcon as IDrawable);
                            break;
                        }

                        if (bounced)
                        {
                            IDestroyable brick = colliable as IDestroyable;
                            if (brick != null && brick is Brick)
                            {
                                score += brick.score;
                                bricksToDestroy.Add(brick as IDrawable);
                                break;
                            }
                        }

                        //Trying to make paddle flip sides if pushed to the end of screen
                        //Logic Does not work
                        //START
                        /*
                        if (PlayerPaddle.X+PlayerPaddle.Width == RIGHT_EDGE)
                        {
                            PlayerPaddle.TravelingRightward = false;
                            PlayerPaddle.TravelingLeftward = true;
                            PlayerPaddle.X = LEFT_EDGE;
                        }
                        if (PlayerPaddle.X == LEFT_EDGE)
                        {
                           PlayerPaddle.TravelingLeftward = false;
                           PlayerPaddle.TravelingRightward = true;
                           PlayerPaddle.X = RIGHT_EDGE;
                        }
                        */
                        // END ----- 
                    }

                }
                foreach (var brick in bricksToDestroy)
                {
                    if (!isPowerUpGoing)
                    {
                        if ((brick as Brick).getExtraBall())
                        {
                            ExtraBallPowerup(false);
                            isPowerUpGoing = true;
                            currentTimeStamp = powerUpTime.Elapsed;
                            whichPowerUp = 1;
                            powerupIcon = new PowerupSprite(this, whichPowerUp, (brick as Brick).X, (brick as Brick).Y);
                            drawables.Add(powerupIcon);
                        }
                        else if ((brick as Brick).getWiderPaddle())
                        {
                            WiderPaddlePowerup(false);
                            isPowerUpGoing = true;
                            currentTimeStamp = powerUpTime.Elapsed;
                            whichPowerUp = 2;
                            powerupIcon = new PowerupSprite(this, whichPowerUp, (brick as Brick).X, (brick as Brick).Y);
                            drawables.Add(powerupIcon);
                        }
                        else if ((brick as Brick).getFasterBall())
                        {
                            isPowerUpGoing = true;
                            currentTimeStamp = powerUpTime.Elapsed;
                            whichPowerUp = 3;
                            powerupIcon = new PowerupSprite(this, whichPowerUp, (brick as Brick).X, (brick as Brick).Y);
                            drawables.Add(powerupIcon);
                        }
                        else if ((brick as Brick).getShorterPaddle())
                        {
                            ShorterPaddleDowngrade(false);
                            isPowerUpGoing = true;
                            currentTimeStamp = powerUpTime.Elapsed;
                            whichPowerUp = 4;
                            powerupIcon = new PowerupSprite(this, whichPowerUp, (brick as Brick).X, (brick as Brick).Y);
                            drawables.Add(powerupIcon);
                        }
                    }
                    drawables.Remove(brick);
                    if (drawables.Count < 5) 
                    {

                    }
                }
                foreach (var powerup in powerupsCaught)
                {
                    drawables.Remove(powerup);
                }
                if (powerUpTime.ElapsedTicks > 10000)

                
                    if (isPowerUpGoing && powerUpTime.Elapsed.TotalSeconds >= currentTimeStamp.TotalSeconds + 10)
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

                            case 4:
                                {
                                    ShorterPaddleDowngrade(true);
                                }
                                break;
                        }
                        powerUpTime.Reset();

                        isPowerUpGoing = false;
                    }

                    if (isPowerUpGoing && whichPowerUp == 3)
                    {
                        ball.Update(4);
                    }
                    else { ball.Update(3); }

                    PlayerPaddle.Update();

                    if (isPowerUpGoing && whichPowerUp == 1)
                    {
                        if (extraBall.Y < BOTTOM_EDGE)
                        {
                            extraBall.Update(3);
                        }
                        else
                        {
                            drawables.Remove(extraBall);
                        }
                    }

                    if (powerupIcon != null)
                    {
                        if (powerupIcon.Y < BOTTOM_EDGE)
                        {
                            powerupIcon.Update();
                        }
                        else drawables.Remove(powerupIcon);
                    }
                
                gameOver = ball.Y > BOTTOM_EDGE;
            }
            return bounced;
        }

        public void DrawGame(CanvasDrawingSession canvas, CanvasSpriteBatch iconSpriteBatch)
        {
            foreach (var drawable in drawables)
            {
                drawable.DrawAsync(canvas, iconSpriteBatch);
            }
        }

        public Color PickRandomColor()
        {
            Random random = new Random();
            int rand = random.Next(0, 5);
            Color color = new Color();
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
                drawables.Remove(extraBall);
            }
            else
            {
                drawables.Add(extraBall);
            }
        }

        public void WiderPaddlePowerup(bool running)
        {
            if (running)
            {
                PlayerPaddle.Width = 60;
                PlayerPaddle.Color = Colors.White;
            }
            else
            {
                PlayerPaddle.Width = 100;
                PlayerPaddle.Color = Colors.LimeGreen;
            }
        }
        public void ShorterPaddleDowngrade(bool running)
        {
            if (running)
            {
                PlayerPaddle.Width = 60;
                PlayerPaddle.Color = Colors.White;
            }
            else
            {
                PlayerPaddle.Width = 30;
                PlayerPaddle.Color = Colors.Red;
            }
        }

        public bool ExtraBallChangeDirection(ICollidable colliable)
        {
            if (colliable.CoolidesBottomEdge(extraBall.X, extraBall.Y))
            {
                extraBall.TravelingDownward = !extraBall.TravelingDownward;
                extraBall.ChangeColorRandomly();
                return true;
            }
            else if (colliable.CollidesTopEdge(extraBall.X, extraBall.Y + extraBall.Radius * 2))
            {
                extraBall.TravelingDownward = !extraBall.TravelingDownward;
                extraBall.ChangeColorRandomly();
                return true;
            }
            else if (colliable.CollidesLeftEdge(extraBall.X, extraBall.Y))
            {
                extraBall.TravelingLeftward = !extraBall.TravelingLeftward;
                extraBall.ChangeColorRandomly();
                return true;
            }
            else if (colliable.ColllidesRightEdge(extraBall.X + extraBall.Radius * 2, extraBall.Y))
            {
                extraBall.TravelingLeftward = !extraBall.TravelingLeftward;
                extraBall.ChangeColorRandomly();
                return true;
     
            }
            return false;
        }

    }
}




