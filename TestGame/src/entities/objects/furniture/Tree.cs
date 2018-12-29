using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TestGame.src.items;
using TestGame.src.tools;

namespace TestGame.src.entities.objects.furniture
{
    [DataContract]
    abstract class Tree : Furniture
    {
        
        private double wigglerot = 0d;
        
        private bool wiggledir = true;
        
        private bool wiggle = false;
        
        private int wigglecount = 0;
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, Position + new Vector2(width / 2, height-16), new Rectangle(source.X, source.Y, source.Width, source.Height - 16), Color.White, (float)(wigglerot), new Vector2(width / 2, height-16), 1f, SpriteEffects.None, 0f);
            sb.Draw(sprite, Position + new Vector2(width / 2, height+48), new Rectangle(source.X, source.Y+48, source.Width, source.Height-48), Color.White, 0f, new Vector2(width / 2, height), 1f, SpriteEffects.None, 0f);
            if (Global.showHitboxes) sb.Draw(Global.buttontexture, new Rectangle(x + hitbox.X, y + hitbox.Y, hitbox.Width, hitbox.Height), Color.Red * 0.5f);
        }

        public override void Update()
        {
            if (wiggle)
            {
                if (wigglerot > 0.02d) wiggledir = false;
                if (wigglerot < -0.02d)
                {
                    wiggledir = true;
                    wigglecount = 0;
                    
                }
                if (wiggledir) wigglerot += (0.001d * wigglecount);
                else wigglerot -= (0.001d*wigglecount);

                if (wigglerot.Equals(0) && wigglecount <= 0)
                {
                    wiggle = false;
                }
            }
            

            base.Update();
        }

        public override void onClick(Item sender)
        {
            wiggle = true;
            wigglecount++;
            wigglerot = 0.0d;
            base.onClick(sender);           
        }
    }

    [DataContract]
    class Palm : Tree
    {
        public Palm(int x, int y)
        {
            this.x = x;
            this.y = y;
            Position = new Microsoft.Xna.Framework.Vector2(x, y);
        }

        public Palm()
        {
        }

        public override void LoadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>("textures/objects/furniture");

            source = new Microsoft.Xna.Framework.Rectangle(11 * 16, 0, 32, 64);

            width = source.Width;
            height = source.Height;

            hitbox = new Microsoft.Xna.Framework.Rectangle(12, 55, 8, 8);
            clickbox = new Microsoft.Xna.Framework.Rectangle(0,0, 32, 64);
            base.LoadContent(cm);
        }

        
    }

    [DataContract]
    class Fir : Tree
    {
        public Fir(int x, int y)
        {
            this.x = x;
            this.y = y;
            Position = new Microsoft.Xna.Framework.Vector2(x, y);
        }

        public Fir()
        {
        }

        public override void LoadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>("textures/objects/furniture");

            source = new Microsoft.Xna.Framework.Rectangle(13 * 16, 0, 32, 64);

            width = source.Width;
            height = source.Height;

            hitbox = new Microsoft.Xna.Framework.Rectangle(12, 55, 8, 8);
            clickbox = new Microsoft.Xna.Framework.Rectangle(0,0, 32, 64);
            base.LoadContent(cm);
        }
    }

    [DataContract]
    class Tree1 : Tree
    {
        public Tree1(int x, int y)
        {
            this.x = x;
            this.y = y;
            Position = new Microsoft.Xna.Framework.Vector2(x, y);
        }

        public Tree1()
        {
        }

        public override void LoadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>("textures/objects/furniture");
            source = new Microsoft.Xna.Framework.Rectangle(9 * 16, 4*16, 32, 64);

            width = source.Width;
            height = source.Height;

            hitbox = new Microsoft.Xna.Framework.Rectangle(12, 55, 8, 8);
            clickbox = new Microsoft.Xna.Framework.Rectangle(0, 0, 32, 64);
            base.LoadContent(cm);
        }
    }
}
