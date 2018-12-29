

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TestGame
{
    class Tile
    {

        public static Tile voidTile = new BasicTile("textures/tiles/grass", true);
        public static Tile grass = new BasicTile("textures/tiles/grass", false);
        public static Tile dirt = new BasicTile("textures/tiles/dirt", false);
        public static Tile fire = new AnimatedTile("textures/animations/campfire", true, 1,5,10);
        public static Tile woodplanks = new BasicTile("textures/tiles/wood", false);

        public static Tile house1 = new ConstructionTile("textures/tiles/constructions/buildings/house1",false,3*16);
        public static Tile tree1 = new ConstructionTile("textures/tiles/constructions/vegetation/tree1", false,16);

        public string path;
        public bool solid;
        public int foreGroundDistance;
        public Texture2D sprite;
        public Tile(string path, bool solid)
        {
            this.path = path;
            this.solid = solid;
        }
        public virtual void loadContent(ContentManager cm)
        {
        }
        public virtual void unloadContent(ContentManager cm)
        {

        }
        public virtual void render(int x, int y, Screen screen)
        {

        }
        public virtual void update()
        {

        }
        public virtual bool getSolid()
        {
            return solid;
        }
    }
}
