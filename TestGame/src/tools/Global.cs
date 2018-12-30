using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.graphics;
using TestGame.src.graphics.effects;
using TestGame.src.graphics.ui;
using TestGame.src.modules;
using TestGame.src.sound;

namespace TestGame.src.tools
{
    public static class Global
    {
        public static List<MapModule> mapModules = new List<MapModule>();
        public static SoundEffect clicksound { get; private set; }
        public static SpriteFont buttonfont { get; private set; }
        public static SpriteFont dashfont { get; private set; }
        public static SpriteFont PixelFont { get; private set; }
        public static Texture2D buttontexture { get; private set; }

        public static bool showHitboxes = false;

        public static Time time { get; private set; }
        public static Camera camera { get; private set; }
        public static Sound sound { get; private set; }
        public static Chat chat { get; private set; }       
        public static Commands cmd { get; private set; }
        public static Random rdm { get; set; }
        public static float UIScale { get; set; }

        

        public static void loadContent(ContentManager cm)
        {
            clicksound = cm.Load<SoundEffect>("sound/effects/stereo/oldlamp_switch");
            buttonfont = cm.Load<SpriteFont>("fonts/SegoeUIBlack");
            dashfont = cm.Load<SpriteFont>("fonts/DashFont");
            PixelFont = cm.Load<SpriteFont>("fonts/PixelFont");
            buttontexture = cm.Load<Texture2D>("textures/ui/menu/Button");

            UIScale = 1f;
        }

        public static void Init(Camera pCamera, Time pTime, Sound pSound, Chat pChat, Commands pCmd, Random pRdm)
        {
            camera = pCamera;
            time = pTime;
            sound = pSound;
            chat = pChat;
            cmd = pCmd;
            rdm = pRdm;
        }

        public static IEnumerable<Color> GetGradients(Color start, Color end, int steps)
        {
            double stepA = ((end.A - start.A) / (double)(steps - 1));
            double stepR = ((end.R - start.R) / (double)(steps - 1));
            double stepG = ((end.G - start.G) / (double)(steps - 1));
            double stepB = ((end.B - start.B) / (double)(steps - 1));

            for (int i = 0; i < steps; i++)
            {
                yield return new Color((int)start.R + (int)(stepR * i),
                                       (int)start.G + (int)(stepG * i),
                                       (int)start.B + (int)(stepB * i),
                                       (int)start.A + (int)(stepA * i));
            }
        }     
        
        public static void replaceColor(Texture2D texture ,Color color, Color replaceColor)
        {
            Color[] tcolor = new Color[texture.Width * texture.Height];
            texture.GetData(tcolor);
            for(int i = 0; i < tcolor.Length; i++)
            {
                if (tcolor[i] == color) tcolor[i] = replaceColor;
            }
            texture.SetData<Color>(tcolor);
        }

        public static string CheckChars(SpriteFont font,string text)
        {
            string newstring = "";
            for(int i = 0; i < text.Length; i++)
            {
                if (font.Characters.Contains(text[i]))
                {
                    newstring += text[i];
                }
            }
            return newstring;
        }
    }
}
