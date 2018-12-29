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
    class Cabinet1 : Furniture
    {
        public Cabinet1(int x, int y)
        {
            this.x = x;
            this.y = y;
            Position = new Microsoft.Xna.Framework.Vector2(x, y);
        }

        public Cabinet1()
        {
        }

        public override void LoadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>("textures/objects/furniture");

            source = new Microsoft.Xna.Framework.Rectangle(0, 4 * 16, 32, 48);

            width = source.Width;
            height = source.Height;

            hitbox = new Microsoft.Xna.Framework.Rectangle(1, 32, 28, 15);
            clickbox = new Microsoft.Xna.Framework.Rectangle(1, 5, 31, 43);
            base.LoadContent(cm);
        }
    }
}
