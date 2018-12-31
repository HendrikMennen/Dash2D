using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.src.entities.objects.furniture
{
    class Window : Lamp
    {
        public override void LoadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>("textures/objects/furniture");

            source = new Microsoft.Xna.Framework.Rectangle(0, 18 * 16, 16, 32);

            width = source.Width;
            height = source.Height;
            solid = false;

            switchable = false;
            on = true;
            lightColor = Color.White;

            hitbox = new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0);
            clickbox = new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 32);
            base.LoadContent(cm);
        }
    }
}
