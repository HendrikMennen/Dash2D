using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.src.entities.particles
{
    class RunParticle : Particle
    {
        public RunParticle(int x, int y, Color color)
        {
            this.x = x;
            this.y = y;

            Duration = 40;
            zz = 10;
            Particlecolor = color;
            spread = 0.05f;
            size = 2;
            rdmcolor = false;
        }
    }
}
