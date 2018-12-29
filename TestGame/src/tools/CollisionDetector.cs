using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.src.tools
{
    class CollisionDetector
    {
        public static bool Collison(Rectangle rect1, Rectangle rect2)
        {
            if (rect1.Right > rect2.X && rect1.X < rect2.X + rect2.Width && rect1.Bottom > rect2.Y && rect1.Y < rect2.Y + rect2.Height) return true;
            return false;
        }

        public static bool Collison(Rectangle rect1, Vector2 pos2)
        {
            if (pos2.X > rect1.X && pos2.X < rect1.X + rect1.Width && pos2.Y > rect1.Y && pos2.Y < rect1.Y+rect1.Height) return true;
            return false;
        }

    }
}
