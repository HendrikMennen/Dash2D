using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.graphics.ui.menu.components;

namespace TestGame.src.graphics.ui.menu.menus
{
    class PauseMenu : Menu
    {
      
        public override void loadObjects()
        {
            addObject(new Button(new Rectangle(Game1.screenWidth / 2 - 200, Game1.screenHeight / 2 - 180, 400, 80), "Multiplayer", "!changemenu 2"));
            addObject(new Button(new Rectangle(Game1.screenWidth / 2 - 200, Game1.screenHeight / 2 - 90, 400, 80), "Settings", "!changemenu 1"));
            addObject(new Button(new Rectangle(Game1.screenWidth / 2 - 200, Game1.screenHeight / 2, 400, 80), "Exit", "!exit"));
            addObject(new Button(new Rectangle(Game1.screenWidth / 2 - 200, Game1.screenHeight / 2 + 90, 400, 80), "Back", "!close"));
        }
    }
}
