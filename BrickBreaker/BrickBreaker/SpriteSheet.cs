using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace BrickBreaker
{
    public static class SpriteSheet
    {
        public static Size iconSize => new Size(80, 79);

        public static Point shorterPaddleLocation => new Point(0, 0);
        public static Point anotherBallLocation => new Point(80, 0);
        public static Point slowerPaddleLocation => new Point(0, 160);
        public static Point longerPaddleLocation => new Point(0, 79);
        public static Point fasterBallLocation => new Point(80, 79);
        public static Point fasterPaddleLocaiton => new Point(160, 79);
    }
}
