using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.entities.objects.furniture;
using TestGame.src.tools;

namespace TestGame.src.level
{
    class Generator
    {
        Level level;
        Random rdm;
        List<List<Rectangle>> generatedMaps = new List<List<Rectangle>>();

        public Generator()
        {

        }

        public void init(Level level)
        {
            this.level = level;
            this.rdm = Global.rdm;
        }

        public void generate(Rectangle area)
        {
            while (level.mapID+1 > generatedMaps.Count) generatedMaps.Add(new List<Rectangle>());
            foreach(var rect in generatedMaps[level.mapID])
            {
                if (rect.X == area.X) return;
            }
            generatedMaps[level.mapID].Add(area);
            //OBJECTS
            Tree1 tree = new Tree1(0, 0);
            Grass grass = new Grass(0, 0);
            //
            for (int y = (int)((area.Y / level.CurrentMap.TileHeight) * level.CurrentMap.TileHeight); y < area.Y + area.Height; y += level.CurrentMap.TileHeight)
            {
                for (int x = (int)((area.X / level.CurrentMap.TileWidth) * level.CurrentMap.TileWidth); x < area.X + area.Width; x += level.CurrentMap.TileWidth)
                {
                    if (!tree.Collision(new Vector2(x, y), level)) if (rdm.Next(0, 20) == 1) level.AddEntity(new Tree1(x, y));
                }
            }
            for (int y = (int)((area.Y / level.CurrentMap.TileHeight) * level.CurrentMap.TileHeight); y < area.Y + area.Height; y+= level.CurrentMap.TileHeight)
            {
                for (int x = (int)((area.X / level.CurrentMap.TileWidth) * level.CurrentMap.TileWidth); x < area.X + area.Width; x+= level.CurrentMap.TileWidth)
                {
                    if(!grass.Collision(new Vector2(x,y),level)) if(rdm.Next(0,2) == 1) level.AddEntity(new Grass(x, y));
                }
            }
           
        }

        public void generateOn(int id)
        {
            //OBJECTS DUMMY
            Tree tree = new Tree1(0, 0);
            Grass grass = new Grass(0, 0);
            //
            for (int y = 0; y < level.CurrentMap.MapHeight; y++)
            {
                for(int x = 0; x < level.CurrentMap.MapWidth; x++)
                {
                    if (level.CurrentMap.layers[0][x+y*level.CurrentMap.MapWidth] == id)
                    {
                            if (!grass.Collision(new Vector2(x* level.CurrentMap.TileWidth + 8, y* level.CurrentMap.TileHeight + 8), level)) if (rdm.Next(0, 5) == 1) level.AddEntity(new Grass(x* level.CurrentMap.TileWidth + 8, y* level.CurrentMap.TileHeight + 8));
                    }
                }               
            }
        }
    }
}
