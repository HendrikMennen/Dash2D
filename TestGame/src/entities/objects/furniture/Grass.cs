using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.src.entities.objects.furniture
{
    [DataContract]
    class Grass : Furniture
    {
        public Grass(int x, int y)
        {
            this.x = x;
            this.y = y;
            Position = new Microsoft.Xna.Framework.Vector2(x, y);
        }

        public Grass()
        {
        }

        public override void LoadContent(ContentManager cm)
        {
            solid = false;
            sprite = cm.Load<Texture2D>("textures/objects/furniture");

            source = new Microsoft.Xna.Framework.Rectangle(6 * 16, 0, 16, 16);

            width = source.Width;
            height = source.Height;

            hitbox = new Microsoft.Xna.Framework.Rectangle(0, 14, 16, 2);
            clickbox = new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16);
            base.LoadContent(cm);
        }
    }
}
