using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.input;
using Microsoft.Xna.Framework.Content;

namespace TestGame.src.graphics.ui.menu.components
{
    public class ListMenu : MenuComponent
    {        
        public string title { get; private set; }
        public List<ListMenuElement> list = new List<ListMenuElement>();
        private int listelementsize = 75;
        private int fontHeight;
    
        public ListMenu(Rectangle position, string title, Texture2D texture, SpriteFont font, List<ListMenuElement> list, string command)
        {
            this.position = position;
            this.title = title;
            this.texture = texture;
            this.font = font;
            this.list = list;
            this.command = command;
            fontHeight = (int)font.MeasureString("0").Y ;
        }    

        public override bool update(Input input)
        {
            if (input.MouseLeftButtonPressed())
            {
                Vector2 mousePos = input.MousePos;
                if (mousePos.X > position.X && mousePos.X < position.X + position.Width && mousePos.Y > position.Y && mousePos.Y < position.Y + position.Height)
                {
                   try
                    {
                        command += " " + list[(((int)mousePos.Y - position.Y-10) / listelementsize)].ip;
                        action(this, null);
                        command = "!connect";
                        return true;
                    }
                    catch (Exception)
                    {
                        
                    }
                    
                }
            }
            return false;    
        }

        public override void draw(SpriteBatch sp)
        {           
            for (int i = 0; i < list.Count; i++)
            {
                sp.Draw(texture, new Rectangle(position.X, position.Y+listelementsize*i, 400, listelementsize), Color.White);
                sp.DrawString(font, "Name: " + list[i].name, new Vector2(position.X+10, position.Y +23-fontHeight + listelementsize*(i)), Color.Black);
                sp.DrawString(font, list[i].players + " Players Online", new Vector2(position.X + 30, position.Y + 48-fontHeight + listelementsize * (i)), Color.Black);
                sp.DrawString(font, "IP: " + list[i].ip, new Vector2(position.X + 10, position.Y + 73-fontHeight + listelementsize * (i)), Color.Black);
            }
        }

      
    }

    public class ListMenuElement
    {
        public string name { get; private set; }
        public string ip { get; private set; }
        public string players { get; private set; }

        public ListMenuElement(string name, string ip, string players)
        {
            this.name = name;
            this.ip = ip;
            this.players = players;
        }
    }
}
