using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.animations;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TestGame.src.tools;
using System.Runtime.Serialization;
using TestGame.src.netcode;

namespace TestGame.src.entities.objects.furniture
{
    [DataContract]
    public class AnimatedFurniture : Furniture
    {
        [ContentSerializerIgnore]
        
        public AnimatedSprite Animation = new AnimatedSprite(new Rectangle(), 1 ,1 );
        [ContentSerializerIgnore]
        
        public int animationspeed = 60 / 5;
        [ContentSerializerIgnore]
        
        public int animCounter = 0;

        public AnimatedFurniture(Vector2 Position, int MapID) : base(Position, MapID)
        {

        }

        public AnimatedFurniture(EntityPacket ep) : base(ep)
        {

        }

        public AnimatedFurniture() : base()
        {

        }

        public override void Update()
        {
            if(animCounter > animationspeed)
            {
                Animation.nextFrame();
                animCounter = 0;
            }            
            animCounter++;
        }

        public override void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(sprite, new Rectangle(x, y , width, height), new Rectangle(source.X + Animation.getSourceRect().X , source.Y + Animation.getSourceRect().Y , width, height) , color);
            if (Global.showHitboxes) sb.Draw(Global.buttontexture, new Rectangle(x + hitbox.X, y + hitbox.Y, hitbox.Width, hitbox.Height), Color.Red * 0.5f);
        }
    }

   
}
