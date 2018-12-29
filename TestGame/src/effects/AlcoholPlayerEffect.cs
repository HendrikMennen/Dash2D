using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.input;
using TestGame.src.tools;

namespace TestGame.src.effects
{
    class AlcoholPlayerEffect : PlayerEffect
    {
        bool dir = true;
          
        public override void update(Input input)
        {
            if(duration > 1)
            {
                if (dir)
                {
                    Global.camera.Rotation += 0.01f;
                    if (Global.camera.Rotation > 1f) dir = false;
                }
                else
                {
                    Global.camera.Rotation -= 0.01f;
                    if (Global.camera.Rotation < -1f) dir = true;
                }                           
                duration--;
            }else
            {
                duration = 0;
                if (Global.camera.Rotation > 0.1f) Global.camera.Rotation -= 0.01f;
                else if (Global.camera.Rotation < -0.1f) Global.camera.Rotation += 0.01f;
                else Global.camera.Rotation = 0;
            }
        }
    }
}
