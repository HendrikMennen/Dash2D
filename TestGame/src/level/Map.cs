using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.entities;
using TestGame.src.entities.objects.furniture;

namespace TestGame.src.level
{
    public class Map
    {
        [ContentSerializer]
        public readonly string MapName;
        public readonly int MapWidth, MapHeight, TileWidth, TileHeight;
        public List<Tileset> tilesets = new List<Tileset>();
        public List<int[]> layers = new List<int[]>();
        public List<int[]> flayers = new List<int[]>();       
        public List<Rectangle> collisions = new List<Rectangle>();
        public List<Furniture> entities = new List<Furniture>();
        public List<Teleporter> teleporters = new List<Teleporter>();

        public Map(string MapName, int MapWidth, int MapHeight, int TileWidth, int TileHeight)
        {
            this.MapName = MapName;
            this.MapWidth = MapWidth;
            this.MapHeight = MapHeight;
            this.TileWidth = TileWidth;
            this.TileHeight = TileHeight;
        }
        private Map() //NEEDED FOR SERIALISATION
        {         
        }

        public int getTile(int layer, Vector2 Position)
        {
            int x = (int)(Position.X / TileWidth);
            int y = (int)(Position.Y / TileHeight);
            if (layer >= 0 && layer < layers.Count)
            {
                if (x >= 0 && x < MapWidth && y >= 0 && y < MapHeight)
                {
                    return layers[layer][x + y * MapWidth];                  
                }
            }
            return 0;
        }

        public Tileset GetTileset(int gid)
        {
            foreach (var tileset in tilesets)
            {
                if (gid > tileset.startvalue && gid < tileset.startvalue + tileset.tileCount) return tileset;
            }
            return tilesets[0];
        }

        public Rectangle getSourceRectangle(int id, Tileset tileset)
        {

            int column = id % tileset.width;

            bool done = false;
            int c = 0;
            while (!done)
            {
                c++;
                if (id < tileset.width * c) done = true;
            }
            int row = c - 1;

            return new Rectangle((column - 1) * Game1.SpriteWidth, row * Game1.SpriteHeight, Game1.SpriteWidth, Game1.SpriteHeight);
        }

        public Color[] getTileTex(int tileid)
        {            
            Tileset ts = GetTileset(tileid);
            Rectangle sr = getSourceRectangle(tileid - ts.startvalue, ts);
            Color[] cc = new Color[TileWidth * TileHeight];
            ts.texture.GetData(0, sr, cc, 0, TileWidth * TileHeight);
            return cc;
        }
    }

    public class Teleporter
    {
        public readonly Rectangle position;
        public readonly int mapID;
        public readonly Vector2 tpCoordinates;

        public Teleporter(Rectangle position, int mapID, Vector2 tpCoordinates)
        {
            this.position = position;
            this.mapID = mapID;
            this.tpCoordinates = tpCoordinates;
        }

        public Teleporter()
        {
        }
    }
    public class Tileset
    {
        [ContentSerializerIgnore]
        public Texture2D texture;
        [ContentSerializer]
        public int startvalue;
        public int tileCount;
        public int width;
        public int height;
        public string path;
        public Tileset(int startvalue,string path)
        {
            this.startvalue = startvalue;
            this.path = path;                
        }

        public void Init(Texture2D texture, int TileWidth, int TileHeight)
        {
            this.texture = texture;
            width = texture.Width / TileWidth;
            height = texture.Height / TileHeight;
            tileCount = (texture.Width / TileWidth) * (texture.Height / TileHeight);
        }
        private Tileset() //NEEDED FOR SERIALISATION
        {
        }

    }  
}
