using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TestGame.src.entities.objects;
using System.Runtime.Serialization;

namespace TestGame.src.entities.objects.furniture
{
    [DataContract]
    class Plant1 : Furniture
    {  

        public Plant1()
        {
        }

        public override void LoadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>("textures/objects/furniture");

            source = new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 32);

            width = source.Width;
            height = source.Height;

            hitbox = new Microsoft.Xna.Framework.Rectangle(1, 22, 14, 9);
            clickbox = new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 32);
            base.LoadContent(cm);
        }
    }
}
