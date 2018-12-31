using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using TestGame.src.entities.objects;
using TestGame.src.graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace TestGame.src.entities.objects.furniture
{
    class TV : Furniture
    {     
        //Texture2D screenTexture;

        public TV(int x, int y)
        {
            this.x = x;
            this.y = y;
            Position = new Microsoft.Xna.Framework.Vector2(x, y);

        }

        public TV()
        {
        }
         
        public override void LoadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>("textures/objects/furniture");
            source = new Microsoft.Xna.Framework.Rectangle(16, 0, 48, 32);

            width = source.Width;
            height = source.Height;

            hitbox = new Microsoft.Xna.Framework.Rectangle(2, 25, 44, 7);
            clickbox = new Microsoft.Xna.Framework.Rectangle(0,0, 16, 32);
            base.LoadContent(cm);
            //screenTexture = cm.Load<Texture2D>("textures/screenTexture");                  
        }

        public override void Update()
        {
            
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            //sb.Draw(screenTexture, new Rectangle(x - width/2+3, y-height/2+2, width-6, height-11), new Rectangle(0, 0, screenTexture.Width, screenTexture.Height), Color.White);                           
        }
    }
}
