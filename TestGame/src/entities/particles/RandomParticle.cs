using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.src.entities.particles
{
    class RandomParticle : Particle
    {
        public RandomParticle(int x, int y) : this(x,y,4)
        {
            
        }

        public RandomParticle(int x, int y, int size)
        {
            this.x = x;
            this.y = y;
            Position = new Microsoft.Xna.Framework.Vector2(x, y);

            Duration = 120;
            spread = 0.1f;
            this.size = size;
            rdmcolor = true;
            Light = true;          
        }
    }
}
