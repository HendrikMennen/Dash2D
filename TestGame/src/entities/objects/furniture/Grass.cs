using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;
using TestGame.src.tools;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace TestGame.src.entities.objects.furniture
{
    class Grass : Furniture
    {
        private double wigglerot = 0d;

        private bool wiggledir = true;

        private bool wiggle = false;

        private int wigglecount = 0;

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

        public override void Update()
        {
            if (CollisionDetector.Collison(Global.addVector2(hitbox, Position), Global.addVector2(level.getClientPlayer().hitbox, level.getClientPlayer().Position)))
            {
                if (!wiggle)
                {
                    wiggle = true;
                    wigglecount++;
                    wigglerot = 0.0d;                    
                }
                
            }
            if (wiggle)
            {
                if (wigglerot > 0.04d) wiggledir = false;
                if (wigglerot < -0.04d)
                {
                    wiggledir = true;
                    wigglecount = 0;
                }
                if (wiggledir) wigglerot += (0.001d * wigglecount);
                else wigglerot -= (0.001d * wigglecount);

                if (wigglecount <= 0)
                {
                    wiggle = false;
                    wigglerot = 0.0d;
                }
            }
            base.Update();
        }

        public override void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(sprite, Position + new Vector2(width / 2, height), new Rectangle(source.X, source.Y, source.Width, source.Height), Color.White, (float)(wigglerot), new Vector2(width / 2, height), 1f, SpriteEffects.None, 0f);
            //base.Draw(sb, color);
        }
    }
}
