using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.src.entities.objects.furniture
{
    [DataContract]
    public class Sofa : Furniture
    {
        public Sofa (int x, int y)
        {
            this.x = x;
            this.y = y;           
        }
      
        public Sofa()
        {
        }

        public override void LoadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>("textures/objects/furniture");

            source = new Microsoft.Xna.Framework.Rectangle(5*16,6 * 16, 48, 32);

            width = source.Width;
            height = source.Height;

            hitbox = new Microsoft.Xna.Framework.Rectangle(0, 17, 48, 15);
            clickbox = new Microsoft.Xna.Framework.Rectangle(0, 5, 48, 27);
            Position = new Microsoft.Xna.Framework.Vector2(x, y);
            base.LoadContent(cm);
        }
    }
}
