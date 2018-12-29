using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.animations;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.src.entities.objects.furniture
{
    [DataContract]
    class Water : AnimatedFurniture
    {
        bool animforeward = true;

        public Water(int x, int y)
        {
            this.x = x;
            this.y = y;
            Position = new Microsoft.Xna.Framework.Vector2(x, y);
        }

        public Water()
        {
        }

        public override void Update()
        {
            if(animCounter > animationspeed)
            {
                if (animforeward)
                {
                    Animation.column += 1;
                    if (Animation.column >= Animation.Columns) animforeward = false;
                }
                else
                {
                    Animation.column -= 1;
                    if (Animation.column <= 0) animforeward = true;
                }
                animCounter = 0;
                
            }
            animCounter++;
        }
        public override void LoadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>("textures/objects/furniture");
            source = new Rectangle(0, 11 * 16, 3 * 16, 16);

            width = 16;
            height = 16;

            Animation = new AnimatedSprite(new Rectangle(0, 0, 48, 16), 1, 3);

            solid = false;

            hitbox = new Rectangle(0, 0, 16, 16);
            clickbox = new Rectangle(0,0, 16, 16);
            base.LoadContent(cm);
        }
    }
}
