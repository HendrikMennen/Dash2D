using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.src.ui
{
    class MousePointer
    {
        public Texture2D Texture { get; set; }
        public MousePointer(int x, int y, Texture2D tex)
        {
            this.Texture = tex;
        }
    }
}
