using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TestGame.src.entities.objects.furniture
{
    class Beachchair : Furniture
    {
        public Beachchair(int x, int y)
        {
            this.x = x;
            this.y = y;
            Position = new Microsoft.Xna.Framework.Vector2(x, y);
        }

        public Beachchair()
        {
        }

        public override void LoadContent(ContentManager cm)
        {

            source = new Microsoft.Xna.Framework.Rectangle(0, 14 * 16, 16, 32);

            width = source.Width;
            height = source.Height;

            hitbox = new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 32);
            clickbox = new Microsoft.Xna.Framework.Rectangle(0,0, 16, 32);
            base.LoadContent(cm);
        }
    }
}
