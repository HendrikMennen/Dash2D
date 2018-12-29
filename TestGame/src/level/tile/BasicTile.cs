using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TestGame
{
    class BasicTile : Tile
    {
        public BasicTile(string path, bool solid) : base(path, solid)
        {
        }

        public override void loadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>(path);
        }

        public override void render(int x, int y, Screen screen)
        {
            screen.renderSprite(x*16, y*16, sprite);
        }
    }
}
