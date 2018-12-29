using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TestGame.src.graphics.effects
{
    public class Time
    {
        private float ambient = 0f;
       
        public float CurrentTime
        {
            get
            {
                return ambient;
            }
            set
            {
                if (value >= 0 && value <= 1f) ambient = value;
            }
        }
       
        public void forwardTime(float value)
        {
            CurrentTime += value;
        }
      
    }

   
}
