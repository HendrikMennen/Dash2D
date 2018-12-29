using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.entities.particles;
using TestGame.src.items;
using TestGame.src.tools;

namespace TestGame.src.entities.objects.furniture
{
    class Particelspawner:Furniture
    {
        private bool on = false;
        private SoundEffect dispense;
        private List<SoundEffectInstance> sounds = new List<SoundEffectInstance>();
        
        private int delay = 3;
        public Particelspawner(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Particelspawner()
        {

        }

        public override void onClick(Item sender)
        {
            if (on) on = false;
            else on = true;                                  
        }

        public override void Update()
        {
            
            if (on && mapid == level.mapID)
            {
                delay++;
                if (delay > 5)
                {                  
                    level.addEntity(new RandomParticle(x, y), mapid);
                    Global.sound.Play(dispense, Position);

                    delay = 3;                     
                }                
            }
        }

        public override void LoadContent(ContentManager cm)
        {
            path = "textures/objects/furniture";
            dispense = cm.Load<SoundEffect>("sound/effects/mono/pop");

            source = new Microsoft.Xna.Framework.Rectangle(3 * 16, 3 * 16, 16, 16);

            width = source.Width;
            height = source.Height;

            hitbox = new Microsoft.Xna.Framework.Rectangle(0, 8, 16, 8);
            clickbox = new Microsoft.Xna.Framework.Rectangle((int)LUCorn().X, (int)LUCorn().Y, 16, 16);
            base.LoadContent(cm);
        }
    }
}
