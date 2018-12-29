using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TestGame.src.animations
{
    public class AnimatedMobSprite : AnimatedSprite
    {
        public SpriteEffects flip;
        public AnimatedMobSprite(Texture2D texture, int rows, int columns) : base(texture, rows, columns)
        {
        }

        public void setDirection(int dir)
        {
            if (dir == 4)
            {
                row = 1;
                flip = SpriteEffects.FlipHorizontally;
                return;
            }
            if (dir == 1) row = 0;           
            if(dir == 2) row = 1;
            if(dir == 3) row = 2;
            flip = SpriteEffects.None;
            
        }
        public void setColumn(int column)
        {
            this.column = column;
        }

        public override void nextFrame()
        {
            currentFrame++;
            column = currentFrame % Columns;
        }


    }
}
