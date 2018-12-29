using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.entities.particles;
using TestGame.src.level;
using TestGame.src.tools;

namespace TestGame.src.entities.projectiles
{
    public class Projectile : Entity
    {
        Vector2 start; 
        Vector2 end;
        Vector2 direction;
        Vector2 pos;
        float speed = 350;
        float elapsed = 0.01f;
        int duration = 1;
        float texturerotation = 0.0f;
        SoundEffect thro, hit;
        Color particlecolor = Color.DarkGray;
        private Color projectileColor = Color.DarkOrange;

        public double Duration
        {
            get
            {
                return duration;
            }
        }

        public Projectile(int x, int y, Vector2 aim)
        {
            this.x = x;
            this.y = y;
            start = new Vector2(x, y);
            pos = start;
            end = aim;

            direction = Vector2.Normalize(end - start);
            duration = 1;


            hitbox = new Rectangle(0, 0, 2, 2);
        }

        public override void LoadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>("textures/models/rocket");
            thro = cm.Load<SoundEffect>("sound/effects/stereo/singleshot");
            hit = cm.Load<SoundEffect>("sound/effects/mono/hit1");
            Global.sound.Play(thro, 0.5f);
            width = sprite.Width;
            height = sprite.Height;

            solid = false;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            if(duration > 0) sb.Draw(sprite, new Rectangle(x - width / 2, y - height / 2, width, height),new Rectangle(0, 0, 2, 2), projectileColor, texturerotation, new Vector2(sprite.Width/2, sprite.Height/2) , SpriteEffects.None, 0f);
        }

        public override void Update()
        {           
            base.Update();           
            if (duration <= 0) return;
            pos += direction * speed * elapsed;
            texturerotation = (float)Math.Atan2(direction.Y,direction.X);

            if(Collision(pos, level))
            {
                duration = 0;
                BloodParticle p = new BloodParticle(x, y, particlecolor);
                for (int i = 0; i < 50; i++) level.AddEntity(p);
            }else
            {
                x = (int)pos.X;
                y = (int)pos.Y;
            }
            
        }

        public override bool Collision(Vector2 position, Level level)
        {
            if(base.Collision(position, level))
            {
                if (LastCollision == level.getClientPlayer())
                {
                    return false;
                }
                if (LastCollision is Mob)
                {
                    particlecolor = Color.Red;
                    Global.sound.Play(hit, position);
                    ((Mob)LastCollision).Health -= 20;
                }
                return true;
            }          
            return false;
        }
    }
}
