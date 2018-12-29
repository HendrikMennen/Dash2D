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
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.src.entities.objects.furniture
{
    [DataContract]
    class Particelspawner:Furniture
    {
               
        private SoundEffect dispense;
        
        private List<SoundEffectInstance> sounds = new List<SoundEffectInstance>();
        
        private int delay = 5;
        public Particelspawner(int x, int y)
        {
            this.x = x;
            this.y = y;
            Position = new Microsoft.Xna.Framework.Vector2(x, y);
        }

        public Particelspawner()
        {

        }

        public override void onClick(Item sender)
        {
            if (On) On = false;
            else On = true;                                  
        }

        public override void Update()
        { 
            if (On && mapid == level.mapID)
            {
                delay++;
                if (delay > 5)
                {
                    RandomParticle p = new RandomParticle(x + width / 2, y + height - 5);
                    p.mapid = mapid;
                    level.AddEntity(p);
                    Global.sound.Play(dispense, Position);

                    delay = 0;                     
                }                
            }
        }

        public override void LoadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>("textures/objects/furniture");
            dispense = cm.Load<SoundEffect>("sound/effects/mono/pop");

            source = new Microsoft.Xna.Framework.Rectangle(3 * 16, 3 * 16, 16, 16);

            width = source.Width;
            height = source.Height;

            hitbox = new Microsoft.Xna.Framework.Rectangle(0, 8, 16, 8);
            clickbox = new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16);
            base.LoadContent(cm);
        }
    }
}
