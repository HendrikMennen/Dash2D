using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.src.graphics.effects
{
    class Light
    {
        private Texture2D lightmask;
       
        public void loadContent(ContentManager cm)
        {
            lightmask = cm.Load<Texture2D>("effects/lighteffect/lightmask");
        }

        public void Draw(SpriteBatch sb, Vector2 position, float scale, Color color)
        {
            sb.Draw(lightmask, new Rectangle((int)(position.X-(lightmask.Width*scale)/2), (int)(position.Y-(lightmask.Height * scale) / 2), (int)(lightmask.Width * scale), (int)(lightmask.Height * scale)), color);
        }        
    }
}
