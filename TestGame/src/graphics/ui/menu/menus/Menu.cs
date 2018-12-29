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
using TestGame.src.graphics.ui.menu.components;
using TestGame.src.input;

namespace TestGame.src.graphics.ui.menu
{
    public class Menu
    {
        protected Input input;
        protected Commands cmd;

        public List<MenuComponent> comps { get; private set; } = new List<MenuComponent>();
        private List<Button> buttons = new List<Button>();
        private List<Textfield> textfields = new List<Textfield>();
        public List<ListMenu> lists = new List<ListMenu>();
       
        public virtual void loadContent(ContentManager cm)
        {           
            loadObjects();
        }

        public void addObject(MenuComponent comp)
        {
            comp.Click += new EventHandler(ComponentFired);
            comps.Add(comp);
        }

        public virtual void loadObjects()
        {

        }

              
        public virtual void ComponentFired(object sender, EventArgs e)
        {
            if(sender is Button) cmd.command(((Button)sender).command);
            if(sender is ListMenu) cmd.command(((ListMenu)sender).command);

        }

        public void init(Input input, Commands cmd)
        {
            this.input = input;
            this.cmd = cmd;
        }

        public virtual void update()
        {
            foreach(var comp in comps)
            {
                if (comp.update(input))
                {
                    if (comp is OptionPane) Interface.removeComponent(comp);
                    break;
                }
            }          
        }

        public virtual void render(SpriteBatch sp)
        {
            foreach (var comp in comps)
            {
                comp.draw(sp);
            }
        }
    }
}
