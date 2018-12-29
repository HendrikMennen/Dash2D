using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.input;
using TestGame.src.tools;

namespace TestGame.src.effects
{
    class PortalEffect : PlayerEffect
    {
        float rate = 0.01f;

        public override void update(Input input)
        {
            if (duration > 1)
            {
                Global.camera.Rotation += rate;
                Global.camera.Zoom += rate/10;
                rate += 0.1f;
                duration--;
            }
            else
            {
                Global.camera.Rotation = 0;
                rate = 0f;
                duration = 0;
                Global.camera.Zoom = 4f;
            }
        }
    }
}
