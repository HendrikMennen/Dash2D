using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.src.entities
{
    class Joseph : Npc
    {
        private Texture2D sprite_walk;
            public Joseph()
            {
                

                animationSpeed = 12;
                rows = 3;
                columns = 4;

                hitbox = new Rectangle(13, 29, 12, 12);


            }

        public override void LoadContent(ContentManager cm)
        {
            sprite_walk = cm.Load<Texture2D>("textures/models/bot/bot_walk");
            sprite = sprite_walk;
            base.LoadContent(cm);
        }

        public override void Update()
            {
                base.Update();
                moveRandom();
            }
        
    }
}
