using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.input;

namespace TestGame.src.effects
{
    public abstract class PlayerEffect
    {
        public int duration { get; set; }

        public abstract void update(Input input);
    }
}
