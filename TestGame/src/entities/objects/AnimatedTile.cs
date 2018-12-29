using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.graphics;

namespace TestGame.src.entities
{
    public class AnimatedTile : Entity
    {
        private int animCounter = 0;
        public int frameCounter = 0;

        private int[] framesX;
        private int[] framesY;
        private int[] duration;
        
        public AnimatedTile(Texture2D texture, int[] framesX, int[] framesY, int[] duration, int x, int y)
        {
            sprite = texture;
            this.x = x;
            this.y = y;
            this.duration = duration;
            this.framesY = framesY;
            this.framesX = framesX;
        }
        public AnimatedTile(Texture2D texture, int[] framesX, int[] framesY, int[] duration, int x, int y, int startDuration)
        {
            sprite = texture;
            this.x = x;
            this.y = y;
            this.duration = duration;
            this.framesY = framesY;
            this.framesX = framesX;

            foreach (int dur in duration)
            {                
                startDuration -= dur;
                if (startDuration < 0)
                {
                    startDuration += dur;
                    break;
                }
                frameCounter++;
            }
            animCounter = startDuration;
        }

        public override void Draw(SpriteBatch sb)
        {    
           sb.Draw(sprite, new Rectangle((int)x-Game1.SpriteWidth/2, (int)y-Game1.SpriteHeight/2, level.CurrentMap.TileWidth, level.CurrentMap.TileHeight), new Rectangle(framesX[frameCounter]* level.CurrentMap.TileWidth, framesY[frameCounter]* level.CurrentMap.TileHeight, level.CurrentMap.TileWidth, level.CurrentMap.TileHeight),Color.White);
        }

        public override void Update()
        {
            animCounter++;
            if (animCounter >= duration[frameCounter])
            {
                frameCounter++;
                
                if (frameCounter >= framesX.Length)
                {
                    frameCounter = 0;
                }                
                animCounter = 0;
            }

        }       
    }
}
