using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TestGame.src.graphics;
using TestGame.src.graphics.effects;
using TestGame.src.entities;
using TestGame.src.entities.objects.furniture;
using System.Linq;
using Lidgren.Network;
using TestGame.src.input;
using TestGame.src.entities.particles;
using static TestGame.Game1;
using TestGame.src.tools;
using TestGame.src.entities.projectiles;
using TestGame.src.items;

namespace TestGame.src.level
{

    public class Level
    {
        public int mapWidth, mapHeight;
        public int screenWidth, screenHeight;
        public string currentMapName= "";
        public int mapID=1;
        public Map CurrentMap;
        private ContentManager cm;
        private Input input;
        private Light LightRenderer;
        public static int RenderedTiles;
        private static Generator generator;


        public List<Player> players = new List<Player>();
        public List<Sheep> sheeps = new List<Sheep>();
        public List<Mob> mobs = new List<Mob>();
        public List<Entity> entities = new List<Entity>();
        public List<Furniture> furniture = new List<Furniture>();
        public List<AnimatedTile> lights = new List<AnimatedTile>();
        public List<Lamp> lamps = new List<Lamp>();
        public List<Fence> fences = new List<Fence>();
        public List<Particle> particles = new List<Particle>();
        public List<Projectile> projectiles = new List<Projectile>();

        //NEW MAP SYSTEM <-- 
        public List<Map> maps = new List<Map>();
        //


        public Level()
        {
            LightRenderer = new Light();
            generator = new Generator();
            this.screenWidth = Game1.screenWidth;
            this.screenHeight = Game1.screenHeight;

            Game1.SpriteHeight = 16;
            Game1.SpriteWidth = 16;        
        }

        public void init(Input input)
        {
            this.input = input;
            generator.init(this);
        }

        public void loadContent(ContentManager cm)
        {
            this.cm = cm;
            LightRenderer.loadContent(cm);

            LoadMap(SaveGame.DeserializeMap<Map>("Disco"));
            LoadMap(SaveGame.DeserializeMap<Map>("christmas"));
            Global.mapModules[0].Activate(true, 1); //ACTIVATE CHESS

            //LoadMap(SaveGame.DeserializeMap<Map>("lobby"));
            LoadMap(SaveGame.DeserializeMap<Map>("testarea"));
            //LoadMap(SaveGame.DeserializeMap<Map>("beach"));
            //LoadMap(SaveGame.DeserializeMap<Map>("chess"));
            //LoadMap(SaveGame.DeserializeMap<Map>("snowhouse"));

            ChangeMap(0);
        }

        public void LoadMap(Map map)
        {
            maps.Add(map);

            foreach (var tileset in map.tilesets)
            {
                tileset.Init(cm.Load<Texture2D>(tileset.path), map.TileWidth, map.TileHeight);
            }

            foreach (var entity in map.entities)
            {
                entity.mapid = maps.Count - 1;
                AddEntity(entity);
            }
        
        }

        public virtual void AddEntity(Entity entity)
        {
            if(input!= null) entity.Init(this, input);
            if(cm != null) entity.LoadContent(cm);

            if (entity is Player) players.Add((Player)entity);
            else if (entity is Sheep) sheeps.Add((Sheep)entity);
            else if (entity is Furniture) furniture.Add((Furniture)entity);
            if (entity is Mob) mobs.Add((Mob)entity);
            if (entity is Entity) entities.Add(entity);
            if (entity is Lamp) lamps.Add((Lamp)entity);
            if (entity is Fence) fences.Add((Fence)entity);
            if (entity is Particle) particles.Add((Particle)entity);
            if (entity is Projectile) projectiles.Add((Projectile)entity);
        }

        public void remEntity(Entity entity)
        {
            entities.Remove(entity);
            if (entity is Furniture) furniture.Remove((Furniture)entity);
            if (entity is Player) players.Remove((Player)entity);
            if (entity is Sheep) sheeps.Remove((Sheep)entity);
            if (entity is Mob) mobs.Remove((Mob)entity);
            if (entity is Lamp) lamps.Remove((Lamp)entity);
            if (entity is Fence) fences.Remove((Fence)entity);
            if (entity is Particle) particles.Remove((Particle)entity);
            if (entity is Projectile) projectiles.Remove((Projectile)entity);
        }


        public bool ChangeMap(int id)
        {
            if (id == mapID || id > maps.Count-1 || id < 0) return false;
            mapID = id;
            if (players.Count > 0) players[0].mapid = id;
            CurrentMap = maps[id];
            currentMapName = CurrentMap.MapName;
            mapWidth = CurrentMap.MapWidth;
            mapHeight = CurrentMap.MapHeight;
            Game1.MapWidth = mapWidth;
            Game1.MapHeight = mapHeight;
            Game1.SpriteWidth = CurrentMap.TileWidth;
            Game1.SpriteHeight = CurrentMap.TileHeight;
            return true;
        }
                                              
        public void Draw(SpriteBatch sb)
        {
            if (CurrentMap == null) return;
            Draw(sb, CurrentMap.layers);
            if (Game1.currentGamestate == GameState.Editor)
            {
                CurrentMap.entities = CurrentMap.entities.OrderBy(o => o.Position.Y + o.height).ToList(); //SORT ALL ENTITIES -> TODO Filter out other map ids
                foreach (var entitie in CurrentMap.entities)
                {
                    entitie.Draw(sb);
                }
            }else
            {
                entities = entities.OrderBy(o => o.Position.Y + o.height).ToList(); //SORT ALL ENTITIES -> TODO Filter out other map ids
                foreach (var entitie in entities)
                {
                    if (entitie.mapid == mapID && entitie.Flat) entitie.Draw(sb);
                }
                foreach (var entitie in entities)
                {
                    if (entitie.mapid == mapID && !entitie.Flat) entitie.Draw(sb);
                }
            }
           
            Draw(sb, CurrentMap.flayers);
        }

        public void Draw(SpriteBatch sb, List<int[]> layerlist)
        {
            int y0 = (int)((Global.camera.Position.Y - (Global.camera.ViewportHeight / 2 / Global.camera.Zoom)) / CurrentMap.TileHeight);
            int x0 = (int)((Global.camera.Position.X - (Global.camera.ViewportWidth / 2 / Global.camera.Zoom)) / CurrentMap.TileWidth);
            int y1 = y0 + (int)((Global.camera.ViewportHeight / CurrentMap.TileHeight) / Global.camera.Zoom + 2);
            int x1 = x0 + (int)((Global.camera.ViewportWidth / CurrentMap.TileWidth) / Global.camera.Zoom + 2);

            if(Global.camera.Rotation != 0)
            {
                y0 -= 10;
                x0 -= 10;
                y1 += 20;
                x1 += 20;
            }

            RenderedTiles = (y1 - y0) * (x1 - x0);

            foreach (var layer in layerlist)
            {
                for(int y = y0; y < y1; y++)
                {
                    for(int x = x0; x < x1; x++)
                    {
                        if (x < 0 || y < 0 || x >= CurrentMap.MapWidth || y >= CurrentMap.MapHeight)
                        {
                            continue;
                        }

                        int gid = layer[x + y * CurrentMap.MapWidth];
                        Tileset tileset = CurrentMap.GetTileset(gid);
                        gid -= tileset.startvalue;
                        if (gid <= 0) continue;
                                               
                        Texture2D texture = tileset.texture;
                        sb.Draw(texture, new Rectangle(x*Game1.SpriteWidth, y* Game1.SpriteHeight, Game1.SpriteWidth, Game1.SpriteHeight), CurrentMap.getSourceRectangle(gid, tileset), Color.White);
                    }
                }
            }
        }
        
        public void DrawLight(SpriteBatch sb)
        {
            if(Game1.playerlight) LightRenderer.Draw(sb, getClientPlayer().CenterPosition, 1f, Color.White);
            foreach (var light in lamps) if (light.mapid == mapID && light.On) LightRenderer.Draw(sb, light.CenterPosition, light.scale, light.lightColor);
            foreach (var light in sheeps) if (light.mapid == mapID) LightRenderer.Draw(sb, light.CenterPosition, 1f, light.sheepcolor);
            foreach (var light in particles) if (light.mapid == mapID && light.Light) LightRenderer.Draw(sb, light.CenterPosition, 0.1f, light.LightColor);
        }
        
        public void update()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update();
            }
            for (int i = 0; i < particles.Count; i++)
            {
                if(particles[i].Duration <= 0)
                {
                    entities.Remove(particles[i]);
                    particles.Remove(particles[i]);
                }
            }
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i].Duration <= 0)
                {
                    entities.Remove(projectiles[i]);
                    projectiles.Remove(projectiles[i]);
                }
            }
         
        }     

        public Player getClientPlayer()
        {
            return players[0];
        }

        

        public void clearEntities()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i] == players[0] || entities[i] == null)
                {
                   
                }
                else
                {
                    remEntity(entities[i]);
                    i--;
                }

            }
        }
    }
}
