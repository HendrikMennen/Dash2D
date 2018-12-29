using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
