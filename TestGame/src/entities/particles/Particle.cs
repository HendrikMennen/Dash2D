using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TestGame.src.tools;

namespace TestGame.src.entities.particles
{
    public abstract class Particle : Entity
    {
        
        protected double xx, yy, zz = 20, xa, ya, za;
        protected bool rdmcolor = false;
        protected int size = 2;
        protected float spread = 0.1f;       
        public Color LightColor { get; protected set; } = Color.White;
        public Color Particlecolor { get; protected set; }
        public bool Light { get; protected set; } = false;
        public double Duration { get; protected set; }  = 60;
             
        public override void LoadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>("textures/particle/star");
            width = size;
            height = size;
            hitbox = new Rectangle(0, 0, size, size);
            xx = x;
            yy = y;

            if (Global.rdm != null) this.Duration = Global.rdm.Next((int)(Duration/2), (int)Duration);
            if (Global.rdm != null) xa = Global.rdm.Next(-10, 10) * spread;
            if (Global.rdm != null) ya = Global.rdm.Next(-10, 10) * spread-0.02;
            if (Global.rdm != null) this.zz = Global.rdm.Next((int)(zz/2), (int)zz);
            if (Global.rdm != null && rdmcolor)
            {
                Particlecolor = new Color((int)Global.rdm.Next(0, 2) * 255, (int)Global.rdm.Next(0, 2) * 255, (int)Global.rdm.Next(0, 2) * 255);
                LightColor = Particlecolor;
            }
            solid = false;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            sb.Draw(sprite, new Rectangle(x - width / 2, y - height / 2, size, size), Particlecolor);
        }

        public override void Update()
        {
            base.Update();
            if (Duration > 0) Duration--;
            za -= 0.1;

            if (zz < 0)
            {
                zz = 0;
                za *= -0.7;
                xa *= 0.4;
                ya *= 0.4;
            }

            this.xx += xa;
            this.yy += ya;
            this.zz += za;

            if (!Collision(new Vector2((float) x, (float)yy - (float)zz), level))
            {
                y = (int)yy - (int)zz;
            }else
            {
                ya *= -1;
            }
            if (!Collision(new Vector2((float)xx,  y), level))
            {
                x = (int)xx;
            }else
            {
                xa *= -1;
            }
        }
    }
}
