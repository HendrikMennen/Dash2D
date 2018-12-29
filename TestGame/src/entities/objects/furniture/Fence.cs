using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.src.entities.objects.furniture
{
    [DataContract]
    public abstract class Fence : Furniture
    {
        
        protected Rectangle upSource;
        
        protected Rectangle sideSource;
        
        protected Rectangle sideEndingSource;
        
        private SpriteEffects flip = SpriteEffects.None;

        public override void Update()
        {
            bool left= false, right= false, up= false, down=false;
            foreach(var fenc in level.fences)
            {
                if (fenc.x - x == 16 && fenc.y == y)
                {
                    right = true;
                }
                if (x - fenc.x == 16 && fenc.y == y)
                {
                    left = true;
                }
                if (fenc.y - y == 16 && fenc.x == x)
                {
                    down = true;
                }
                if (y - fenc.y == 16 && fenc.x == x)
                {
                    up = true;
                }
            }

            if (left && right)
            {
                source = sideSource;
                hitbox = new Rectangle(0, 12, 16, 4);
            }
            else if (right)
            {
                source = sideEndingSource;
                flip = SpriteEffects.None;
                hitbox = new Rectangle(6, 12, 10, 4);
            }
            else if (left)
            {
                source = sideEndingSource;
                flip = SpriteEffects.FlipHorizontally;
                hitbox = new Rectangle(0, 12, 6, 4);
            }
            else if(up)
            {
                source = upSource;
                flip = SpriteEffects.None;
                hitbox = new Rectangle(6, 0, 2, 16);
                if (down) hitbox = new Rectangle(6, 0, 2, 24);
            }

            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            Rectangle draw = new Rectangle(x, y, width, height);
            sb.Draw(sprite, draw, source, Color.White, 0f, new Vector2(), flip, 0f);
        }
    }

    [DataContract]
    class Fence1: Fence
    {
        public Fence1(int x, int y)
        {
            this.x = x;
            this.y = y;
            Position = new Microsoft.Xna.Framework.Vector2(x, y);
        }

        public Fence1()
        {
        }

        public override void LoadContent(ContentManager cm)
        {
            solid = true;
            sprite = cm.Load<Texture2D>("textures/objects/furniture");

            upSource = new Microsoft.Xna.Framework.Rectangle(2 * 16, 5 * 16, 16, 16);
            sideSource = new Microsoft.Xna.Framework.Rectangle(3 * 16, 4 * 16, 16, 16);
            sideEndingSource = new Microsoft.Xna.Framework.Rectangle(2 * 16, 4 * 16, 16, 16);

            source = new Microsoft.Xna.Framework.Rectangle(2 * 16, 4*16, 16, 16);

            width = source.Width;
            height = source.Height;

            hitbox = new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16);
            clickbox = new Microsoft.Xna.Framework.Rectangle(0 , 0, 16, 16);
            base.LoadContent(cm);
        }
    }
}
