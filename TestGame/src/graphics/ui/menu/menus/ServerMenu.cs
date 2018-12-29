using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.graphics.ui.menu.components;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace TestGame.src.graphics.ui.menu
{
    class ServerMenu : Menu
    {
        public override void loadContent(ContentManager cm)
        {           
            base.loadContent(cm);
        }

        public override void update()
        {
            base.update();
        }

        public override void ComponentFired(object comp, EventArgs e)
        {
#if true
            if(comp is Textfield)
            {
                cmd.command(((Textfield)comp).command + " " + ((Textfield)comp).text);
                return;
            }
            if(comp == comps[2])
                {
                cmd.command("!addserver " + comps[1].text);
                comps[1].text = "";
                    return;
                }
            base.ComponentFired(comp, e);
#endif
        }

        public override void loadObjects()
        {
            addObject(new Button(new Rectangle(Game1.screenWidth / 2 - 450, Game1.screenHeight / 2 - 90, 400, 80), "Search Local Games", "!searchlocalservers"));
            addObject(new Textfield(new Rectangle(Game1.screenWidth / 2 - 450, Game1.screenHeight / 2, 300, 80), "Type IP", "!addserver"));
            addObject(new Button(new Rectangle(Game1.screenWidth / 2 - 450 + 300, Game1.screenHeight / 2, 100, 80), "ADD", "!addserver"));
            addObject(new Button(new Rectangle(Game1.screenWidth / 2 - 450, Game1.screenHeight / 2 + 90, 400, 80), "Back", "!changemenu 1"));
            //addObject(new ListMenu(new Rectangle(Game1.screenWidth / 2 + 50, Game1.screenHeight / 2 - 90, 400, 270), "Local Servers", serverList, serverfont, Network.Network.servers, "!connect"));
        }
    }
}
//GEIER
