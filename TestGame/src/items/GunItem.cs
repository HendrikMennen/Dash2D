using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.entities.projectiles;
using TestGame.src.input;
using TestGame.src.tools;

namespace TestGame.src.items
{
    class GunItem<T> : Item
    {
        Projectile projectile;
        Texture2D guns;
        Rectangle gunsource_vertical;
        Rectangle gunsource_horizontal;
        Vector2 projectilepos = new Vector2();
        Rectangle gunpos = new Rectangle();
        Rectangle gunsource;
        int ammo = 3;
        SpriteEffects se = SpriteEffects.None;

        public GunItem(int column, int row, Rectangle gunsource_v, Rectangle gunsource_h, string name)
        {
            this.name = name;
            source = new Rectangle(column * 16, row * 16, 16, 16); //ItemSourceRectangle        
            this.gunsource_vertical = gunsource_v;
            this.gunsource_horizontal = gunsource_h;
            this.gunsource = gunsource_vertical;
        }

        public override void loadContent(ContentManager cm)
        {
            base.loadContent(cm);
            guns = cm.Load<Texture2D>("textures/objects/guns");
        }

        public override void update(Input input)
        {
            int mapX = (int)input.getMapPos(input.MousePos).X;
            int mapY = (int)input.getMapPos(input.MousePos).Y;

            if(Math.Abs(mapX-level.getClientPlayer().Position.X) > Math.Abs(mapY - level.getClientPlayer().Position.Y))
            {
                if(mapX - level.getClientPlayer().Position.X > 0) level.getClientPlayer().direction = 4;
                else level.getClientPlayer().direction = 2;
            }
            else
            {
                if (mapY - level.getClientPlayer().Position.Y > 0) level.getClientPlayer().direction = 3;
                else level.getClientPlayer().direction = 1;
            }

            if (input.MouseLeftButtonPressed())
            {
                if (ammo <= 0) return;
                projectile = (Projectile)Activator.CreateInstance(typeof(T), (int)projectilepos.X , (int)projectilepos.Y, input.getMapPos(input.MousePos));
                projectile.mapid = level.getClientPlayer().mapid;
                level.AddEntity(projectile);
                ammo--;
            }

            se = SpriteEffects.None;
            gunsource = gunsource_vertical;
            if (level.getClientPlayer().direction == 2)
            {
                se = SpriteEffects.FlipHorizontally;
                gunpos = new Rectangle((int)level.getClientPlayer().Position.X + 3, (int)level.getClientPlayer().Position.Y + 16, 16, 16);
                projectilepos = level.getClientPlayer().Position + new Vector2(-8 + 16, 7 + 16);
            }
            else if (level.getClientPlayer().direction == 4)
            {
                gunpos = new Rectangle((int)level.getClientPlayer().Position.X - 1 + 16, (int)level.getClientPlayer().Position.Y - 2 + 16, 16, 16);
                projectilepos = level.getClientPlayer().Position + new Vector2(-8 + 16, 7 + 16);
            }
            else if (level.getClientPlayer().direction == 3)
            {
                gunpos = new Rectangle((int)level.getClientPlayer().Position.X - 13 + 16, (int)level.getClientPlayer().Position.Y + 16, 16, 16);
                projectilepos = level.getClientPlayer().Position + new Vector2(-8 + 16, 7 + 16);
                gunsource = gunsource_horizontal;
            }
            else
            {
                projectilepos = level.getClientPlayer().Position + new Vector2(6 + 16, 16);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
                              
            if (level.getClientPlayer().direction == 1)           
            {
                return;
            }

            sb.Draw(guns, gunpos, gunsource, Color.White, 0f, Vector2.Zero, se, 0f);
        }
    }
}
