using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.tools;
using TestGame.src.graphics.ui.menu.components;
using TestGame.src.graphics.ui.menu.menus;
using TestGame.src.input;

namespace TestGame.src.graphics.ui.menu
{
    public class Interface
    {
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
                if (!value) changeMenu(0);
                else changeMenu(1);
            }
        }
        public List<Menu> menus = new List<Menu>();
        public Menu currentMenu;

        private ContentManager cm;
        private Input input;
        private Commands cmd;
        private static List<MenuComponent> activeComponents = new List<MenuComponent>();
      
        public Interface()
        {
            menus.Add(new Menu()); // void
            menus.Add(new PauseMenu());
            menus.Add(new ServerMenu());            
            currentMenu = menus[0];
        }      

        public static void addComponent(MenuComponent comp)
        {
            activeComponents.Add(comp);
        }
        public static void removeComponent(MenuComponent comp)
        {
            activeComponents.Remove(comp);
        }

        public void init(Input input, Commands cmd)
        {
            this.input = input;
            this.cmd = cmd;         
            
            foreach(var menu in menus)
            {
                menu.init(input, cmd);
            } 
        }

        public void changeMenu(int id)
        {
            //activeComponents.Clear();
            currentMenu = menus[id];            
            activeComponents = menus[id].comps;           
        }

        public void loadContent(ContentManager cm)
        {
            this.cm = cm;
            foreach (var menu in menus) menu.loadContent(cm);           
        }

        public void update()
        {    
            foreach (var com in activeComponents)
            {
                if (com.update(input)){
                    if(com is OptionPane)
                    {
                        removeComponent(com);                        
                    }
                    break;
                }
            }
        }

        public void render(SpriteBatch sb)
        {
            foreach (var com in activeComponents) com.draw(sb);
        }

        public void changeResolution()
        {
            foreach(var menu in menus)
            {
                menu.comps.Clear();
                menu.loadObjects();
            }
        }
    }
}
