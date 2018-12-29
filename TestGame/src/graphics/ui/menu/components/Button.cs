using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using TestGame.src.input;
using Microsoft.Xna.Framework.Content;
using TestGame.src.tools;

namespace TestGame.src.graphics.ui.menu.components
{
    class Button : MenuComponent
    {
        public Button(Rectangle position,string text, string command)
        {
            this.position = position;
            this.text = text;
            this.command = command;
            texture = Global.buttontexture;
            font = Global.buttonfont; 
        }

        public override bool update(Input input)
        {         
            Vector2 mousePos = input.MousePos;
            if (mousePos.X > position.X && mousePos.X < position.X + position.Width && mousePos.Y > position.Y && mousePos.Y < position.Y + position.Height) 
            {
                if (input.MouseLeftButtonPressed())
                {
                    EventArgs e = null;
                    action(this, e);
                    return true;
                }
            }
            return false;
        }

        public override void draw(SpriteBatch sp)
        {
            textPos = new Vector2((int)((position.X + position.Width / 2) - font.MeasureString(text).X / 2), (int)((position.Y + position.Height / 2) - font.MeasureString(text).Y / 2));
            sp.Draw(texture, position, Color.White);
            sp.DrawString(font, text, textPos, Color.Black);
        }       
    }
}
