using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.tools;
using TestGame.src.entities;
using TestGame.src.input;
using TestGame.src.level;

namespace TestGame.src.graphics.ui
{
    public class Chat
    {
        private Level level;
        private Input input;
        private Commands cmd;
        private Texture2D bubble;
        private Texture2D chatWindow;
        private int chatWindowHeight = 100;
        private SpriteFont font;
        private Rectangle position;
        private Rectangle write_position;
        private int margin_bottom = 110;
        private Vector2 textPosition;
        private int scroll = 0;
        private string text = "";
        private bool open = false;
        public bool Open
        {
            get
            {
                return open;
            }
            set
            {
                open = value;             
                scroll = 0;
            }
        }
        private List<ChatMessage> lines = new List<ChatMessage>();
        private Queue<ChatMessage> active_lines = new Queue<ChatMessage>();


        public void init(Level level, Input input, Commands cmd)
        {
            this.level = level;
            this.input = input;
            this.cmd = cmd;

            input.TextInput += TextInp;   
        }   

        public void WriteLine(ChatMessage cl)
        {            
            lines.Add(cl);
            active_lines.Enqueue(cl);                    
        }
        
        public void loadContent(ContentManager cm)
        {
            font = cm.Load<SpriteFont>("fonts/dashFont");
            bubble = cm.Load<Texture2D>("textures/ui/chat/chatBubble");
            chatWindow = cm.Load<Texture2D>("textures/ui/misc/Background");          
        }    

        public void changeResolution()
        {
            position = new Rectangle(Game1.UI_Margin.Left, Game1.screenHeight- (int)font.MeasureString("Q").Y-margin_bottom, 500, 0);
            write_position = new Rectangle(Game1.UI_Margin.Left, Game1.screenHeight - (int)font.MeasureString("I").Y, Game1.screenWidth ,(int)font.MeasureString("I").Y);
        }
        
        public void TextInp(object sender, TextInputEventArgs args)
        {
            if (open)
            {
                bool addtext = false;
                foreach (var character in font.Characters)
                {
                    if (args.Character.Equals(character)) addtext = true ;
                }

                if (addtext) text += args.Character;
            }
        }

        public void update()
        {
            foreach(var line in active_lines)
            {
                if (line.Update())
                {
                    active_lines.Dequeue();
                    break;
                }
            }  
                     
            if(open)
            {
                if (input.KeyPressed(Keys.Up))
                {
                    if (scroll < lines.Count-4) scroll++;
                }
                if (input.KeyPressed(Keys.Down))
                {
                    if (scroll > 0) scroll--;
                }

              
                if (input.KeyPressed(Keys.Back)) if(text.Length > 0) text = text.Remove(text.Length - 1);       
                if (input.KeyPressed(Keys.Enter)){
                    Open = false;
                    if (text.StartsWith("!")) cmd.command(text);
                    else
                    {
                        if (Game1.online)
                        {
                            netcode.NetCode.Chat(text);                           
                        }
                        else
                        {
                            level.getClientPlayer().say(text);
                            cmd.command("!chat " + level.getClientPlayer().name + " " + text);
                        }
                    }
                    text = "";
                }
            }
        }

        

        public void render(SpriteBatch g)
        {
            g.Draw(chatWindow, position, Color.Black * 0.8f);
            textPosition = new Vector2(position.X + 10, position.Y + position.Height);
            if (!open)
            {
                ChatMessage[] activelines = active_lines.ToArray();
                position.Height = 0;
                for (int i = activelines.Length - 1; i >= 0; i--)
                {                  
                    float height = activelines[i].TotalHeight(font, position.Width);
                    textPosition.Y -= height;
                    position.Height += (int)height;

                    activelines[i].Draw(g, font, textPosition, position.Width);                                                        
                    if (position.Height > chatWindowHeight) active_lines.Dequeue();
                }                            
                position.Y = Game1.screenHeight - position.Height - margin_bottom;
            }
            else
            {
                position.Height = 0;
                position.Y = Game1.screenHeight - position.Height - margin_bottom;
                g.Draw(chatWindow, write_position, Color.Black * 0.8f);
                g.DrawString(font, "You: " + text, new Vector2(write_position.X + 10, write_position.Y + write_position.Height - font.MeasureString("I").Y), Color.Yellow);
                g.Draw(chatWindow, new Rectangle(position.X, (int)textPosition.Y, position.Width, 0), Color.Black);

                for (int i = lines.Count - 1 - scroll; i >= 0; i--)
                {
                    float height = lines[i].TotalHeight(font, position.Width);
                    textPosition.Y -= height;
                    position.Height += (int)height;

                    lines[i].Draw(g, font, textPosition, position.Width);                                   
                }
                position.Y = Game1.screenHeight - position.Height - margin_bottom;
            }                       
        }
    }   

    public class ChatMessage
    {
        public readonly string author;
        public readonly string message;
        public Color authorcolor { get; private set; }
        public Color messagecolor { get; private set; }
        private float charHeight = 0;
        private int duration = 400;
        

        public ChatMessage(string author, string message) : this(author, Color.AliceBlue ,message, Color.DarkGray)
        {
           
        }

        public ChatMessage(string author, Color authorcolor, string message, Color messagecolor)
        {
            this.author = author;
            this.message = message;
            this.authorcolor = authorcolor;
            this.messagecolor = messagecolor;
        }
       
        public bool Update()
        {
            if (duration > 0)
            {
                duration--;
                return false;
            }
            return true;
        }

        public void Draw(SpriteBatch sb, SpriteFont font, Vector2 pos, float maxWidth)
        {
            charHeight = font.MeasureString("I").Y;
            sb.DrawString(font, author, pos, authorcolor);
            pos.X += font.MeasureString(author).X;
            sb.DrawString(font, " -> ", pos, Color.LimeGreen);
            pos.X += font.MeasureString(" -> ").X;
            maxWidth -= (pos.X + 5);

            float messagelength = font.MeasureString(message).X;
            int lines = (int)(messagelength / maxWidth) +1;
            float charlength = messagelength / message.Length;
            int maxcharsperline = (int)(maxWidth / charlength);
            float totalHeight = charHeight * lines;

            string[] words = message.Split(' ');
            int charcounter = 0;
            int totalcharcounter = 0;

            for (int i = 0; i < words.Length; i++)
            {               
                if (maxcharsperline > words[i].Length + charcounter)
                {
                    charcounter += words[i].Length + 1;
                }
                else
                {           
                    if(words[i].Length > 15)
                    {
                        words[i] = words[i].Substring(maxcharsperline - charcounter);
                        charcounter = maxcharsperline;
                    }         

                    sb.DrawString(font, message.Substring(totalcharcounter, charcounter-1), pos, messagecolor);
                    pos.Y += charHeight;
                    totalcharcounter += (charcounter);
                    charcounter = 0;
                    i--;
                }
            }
            sb.DrawString(font, message.Substring(totalcharcounter, message.Length - totalcharcounter), pos, messagecolor);
        }

        public float TotalHeight(SpriteFont font, int maxWidth)
        {
            maxWidth -= ((int)font.MeasureString(author + " -> ").X + 15);
            charHeight = font.MeasureString("I").Y;
            float messagelength = font.MeasureString(message).X;
            int lines = (int)(messagelength / maxWidth) + 1;
            return charHeight * lines;
        }
    }
}
