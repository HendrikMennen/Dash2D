using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.input;
using System.Runtime.Serialization;

namespace TestGame.src.items
{
    class BasicItem : Item
    {       
        public readonly string command;
        public BasicItem(int column, int row, string name, string command)
        {
            this.Name = name;
            this.command = command;
            source = new Rectangle(column * 16, row * 16, 16, 16);
        }

        public BasicItem()
        {

        }

        public override void Update(Input input)
        {
            base.Update(input);
            if (input.MouseLeftButtonPressed())
            {
                //Activated(this, null);
            }
        }
    }
}
