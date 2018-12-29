using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.input;
using TestGame.src.tools;

namespace TestGame.src.items
{
    [DataContract]
    public class ClothItem : Item
    {
        public Texture2D Sprite_walk { get; private set; }
        public Texture2D Sprite_run { get; private set; }
        [DataMember]
        public Color Clothcolor { get; private set; }
        readonly string path;

        public ClothItem(int column, int row, string name, string path, Color color)
        {
            this.Name = name;
            source = new Rectangle(column * 16, row * 16, 16, 16);
            Clothcolor = color;
            this.path = path;
        }

        public override void LoadContent(ContentManager cm)
        {
            Sprite_walk = cm.Load<Texture2D>(path);
            Sprite_run = cm.Load<Texture2D>(path + "_run");

            //APPLY JACKET COLOR
            //Global.replaceColor(sprite_walk, new Color(255, 255, 255), clothcolor);
            //Global.replaceColor(sprite_walk, new Color(153, 153, 153), new Color(clothcolor.R - 100, clothcolor.G - 100, clothcolor.B - 100));
            //Global.replaceColor(sprite_walk, new Color(229, 229, 229), new Color(clothcolor.R - 50, clothcolor.G - 50, clothcolor.B - 50));
            //Global.replaceColor(sprite_walk, new Color(6, 6, 8), Color.Black);
            //Global.replaceColor(sprite_run, new Color(255, 255, 255), clothcolor);
            //Global.replaceColor(sprite_run, new Color(153, 153, 153), new Color(clothcolor.R - 100, clothcolor.G - 100, clothcolor.B - 100));
            //Global.replaceColor(sprite_run, new Color(229, 229, 229), new Color(clothcolor.R - 50, clothcolor.G - 50, clothcolor.B - 50));
            //Global.replaceColor(sprite_run, new Color(6, 6, 8), Color.Black);

            base.LoadContent(cm);
        }

        public override void Update(Input input)
        {
            if (input.MouseRightButtonPressed())
            {
                level.getClientPlayer().Jacket = this;
            }
            base.Update(input);
        }
    }
}
