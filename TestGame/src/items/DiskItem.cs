using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace TestGame.src.items
{
    class DiskItem<T> : Item
    {

        public object disk { get; private set; }
        private string path;


        public DiskItem(int column, int row, string name, string path)
        {
            source = new Rectangle(column * 16, row * 16, 16, 16); //ItemSourceRectangle   
            this.path = path;
            this.name = name;
        }

        public override void loadContent(ContentManager cm)
        {
            disk = cm.Load<T>(path);
        }
    }
}
