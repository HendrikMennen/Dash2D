using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.input;
using TestGame.src.tools;

namespace TestGame.src.graphics.ui.menu.components
{
    public abstract class MenuComponent
    {
        public Rectangle position { get; set; }
        protected Vector2 textPos { get; set; }
        protected Texture2D texture;
        protected SpriteFont font;
        public string text { get; set; }
        public string command { get; set; }

        public event System.EventHandler Click;
        public delegate void EventHandler(object b, EventArgs e);

        public void action(object sender, EventArgs e)
        {
            try
            {
                Global.sound.Play(Global.clicksound);
                Click(sender, e);
            }catch
            {

            }
        }

        public abstract bool update(Input input);
        public abstract void draw(SpriteBatch sp);        
    }
}
