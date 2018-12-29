using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using TestGame.src.graphics;
using TestGame.src.items;
using TestGame.src.netcode;
using TestGame.src.tools;

namespace TestGame.src.entities.objects.furniture
{
    public abstract class Furniture : Entity
    {
        [ContentSerializerIgnore]
        public Rectangle clickbox;
        [ContentSerializerIgnore]
        public Rectangle source;
        [ContentSerializerIgnore]
        public virtual bool On
        {
            get
            {
                return on;
            }
            set
            {
                on = value;
            }
        }
        private bool on = false;
        public Furniture(Vector2 Position, int MapID) : base(Position, MapID)
        {

        }

        public override void ApplyPacket(EntityPacket ep)
        {
            if (ep.On.Available) On = ep.On.Value;
            base.ApplyPacket(ep);
        }

        public Furniture(EntityPacket ep) : base(ep)
        {

        }

        public Furniture() : base()
        {

        }

        public override void Draw(SpriteBatch sb, Color color)
        {
            //Rectangle draw = new Rectangle(x-width/2, y-height/2, width, height); 
            Rectangle draw = new Rectangle(x, y, width, height); 
            sb.Draw(sprite, draw, source, color);

            if (Global.showHitboxes) sb.Draw(Global.buttontexture, new Rectangle(x + hitbox.X, y + hitbox.Y, hitbox.Width, hitbox.Height), Color.Red * 0.5f);
        }
        public override void Draw(SpriteBatch sb)
        {
            Draw(sb, Color.White);
        }

            
    }
}
