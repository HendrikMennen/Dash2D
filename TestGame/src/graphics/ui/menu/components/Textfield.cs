using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.src.input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using TestGame.src.tools;

namespace TestGame.src.graphics.ui.menu.components
{
    class Textfield : MenuComponent
    {      
        private bool activated = false;
   
        public Textfield(Rectangle position, string text, string command)
        {
            this.position = position;
            this.text = text;          
            this.command = command;

            texture = Global.buttontexture;
            font = Global.buttonfont;
        }     

        public override void draw(SpriteBatch sp)
        {
            sp.Draw(texture, position, Color.White);
            Vector2 textPos = new Vector2((position.X + position.Width / 2) - font.MeasureString(text).X / 2, (position.Y + position.Height / 2) - font.MeasureString(text).Y / 2);
            sp.DrawString(font, text, textPos , Color.Black);
        }

       

        public override bool update(Input input)
        {
            if (input.MouseLeftButtonPressed())
            {
                Vector2 mousePos = input.MousePos;
                if (mousePos.X > position.X && mousePos.X < position.X + position.Width && mousePos.Y > position.Y && mousePos.Y < position.Y + position.Height)
                {
                    activated = true;
                }else
                {
                    activated = false;
                }             
            }
            if (activated)
            {
                if (input.KeyPressed(Keys.Back)) if (text.Length > 0) text = text.Remove(text.Length - 1);
                text += input.getInput();
                if (input.KeyPressed(Keys.Enter))
                {              
                    EventArgs e = null;
                    action(this, e);
                    text = "";
                    activated = false;
                    return true;
                }
            }
            return false;
        }
    }
}
