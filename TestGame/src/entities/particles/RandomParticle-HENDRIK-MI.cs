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

            duration = 60;
            spread = 0.5f;
            this.size = size;
            rdmcolor = true;
            Light = true;
        }
    }
}
