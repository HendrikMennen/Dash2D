using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using TestGame.src.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TestGame.src.tools;

namespace TestGame.src.graphics.ui.menu.components
{
    class OptionPane : MenuComponent
    {
        public string title { get; private set; }
        public List<MenuComponent> options;

        public OptionPane(string title, List<MenuComponent> options, string command)
        {
            position = new Rectangle(Game1.screenWidth / 2 - (int)font.MeasureString(title).X - 10, Game1.screenHeight / 2, (int)font.MeasureString(title).X + 20, (int)font.MeasureString(title).Y * 3);
            this.title = title;
            this.options = options;
            this.command = command;

            font = Global.buttonfont;
            texture = Global.buttontexture;

            int width = (int)font.MeasureString(title).X + 20;
            int height = (int)font.MeasureString(title).Y * 3;
            int xPos = position.X;
            int yPos = position.Y + position.Height / 2;
            for (int i = 0; i < options.Count; i++)
            {
                options[i].position = new Rectangle(xPos + width * i, yPos, width, height);
            }
        }

        public override void draw(SpriteBatch sp)
        {
            position = new Rectangle(Game1.screenWidth / 2 - (int)font.MeasureString(title).X - 10, Game1.screenHeight / 2, (int)font.MeasureString(title).X + 20, (int)font.MeasureString(title).Y * 3);

            foreach(var option in options)
            {
                option.draw(sp);
            }
        }

        public override bool update(Input input)
        {
            foreach(var option in options)
            {
                if (option.update(input))
                {
                    action(this, null);
                    return true;
                }
            }
            return false;
        }

       
    }
}
