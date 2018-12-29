using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.entities;
using TestGame.src.level;
using TestGame.src.tools;

namespace TestGame.src.graphics.ui
{
    class Healthbar
    {
        Texture2D background;
        Texture2D healthbar_full;
        Rectangle position;
        Rectangle health;
        Rectangle healthpos;
        Level level;
        
        public void loadContent(ContentManager cm)
        {
            background = cm.Load<Texture2D>("textures/ui/misc/healthbar");
            healthbar_full = cm.Load<Texture2D>("textures/ui/misc/healthbar_full");
        }

        public void Init(Level level)
        {
            this.level = level;
        }

        public void Update(GameTime gameTime)
        {
            int healthperc = (int)(((float)level.getClientPlayer().Health / (float)level.getClientPlayer().MaxHealth) * (float)healthbar_full.Width);
            health = new Rectangle(0, 0, healthperc, healthbar_full.Height);
            healthpos = new Rectangle(position.X, position.Y, healthperc, position.Height);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(background, position, Color.White);
            sb.Draw(healthbar_full, healthpos,health, Color.Red*0.9f);
            string healthtext = level.getClientPlayer().Health + "/" + level.getClientPlayer().MaxHealth;
            sb.DrawString(Global.buttonfont, healthtext, new Vector2(position.X + position.Width / 2 - Global.buttonfont.MeasureString(healthtext).X / 2, position.Y + Global.buttonfont.MeasureString(healthtext).Y/5), Color.Black);
        }

        public void updateResolution()
        {
            position = new Rectangle(Game1.screenWidth-210, 10, 200, 30);
        }
    }
}
