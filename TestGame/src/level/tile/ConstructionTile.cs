using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame
{
    class ConstructionTile : Tile
    {
        public ConstructionTile(string path, bool solid, int foreGroundDistance) : base(path, solid)
        {
            this.foreGroundDistance = foreGroundDistance;           
        }
        public override void loadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>(path);
        }

        public override void render(int x, int y, Screen screen)
        {
            screen.renderSprite(x * 16, y * 16, sprite);
        }
    }
}
