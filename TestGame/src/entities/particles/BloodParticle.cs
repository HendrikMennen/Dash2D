using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.src.entities.particles
{
    class BloodParticle : Particle
    {
        public BloodParticle(int x, int y)
        {
            this.x = x;
            this.y = y;

            duration = 60;
            particlecolor = Color.Red;
            spread = 0.05f;
            size = 2;
            rdmcolor = false;
        }

        public BloodParticle(int x, int y, Color color)
        {
            this.x = x;
            this.y = y;

            duration = 60;
            particlecolor = color;
            spread = 0.05f;
            size = 2;
            rdmcolor = false;
        }
    }
}
