using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TestGame.src.entities.objects.furniture;
using TestGame.src.graphics;
using TestGame.src.graphics.ui.menu;
using TestGame.src.graphics.ui.menu.components;
using TestGame.src.input;
using TestGame.src.tools;

namespace TestGame.src.level
{
    public class MapEditor
    {
        #region Attributes
        private Stack<Backup> undo = new Stack<Backup>();

        private Level level;
        private Input input;
        public Camera TilesetView { get; private set; }
        private int[] currentLayer;
        private int currentLayerID;
        private Tileset currentTileset;
        private int currentTilesetID;
        private ContentManager cm;
        private int[,] currentItemIDs = new int[0, 0];
        private Rectangle sourceRectangle;
        // ID SELECTOR
        private bool Drag = false;
        private int startID = 0;
        private int endID = 0;
        // BUTTONS

        // Object
        Rectangle objectposition = new Rectangle();
        Teleporter teleporterparams; // needed to Create teleport
        private bool objectdrag = false;
        //
        private List<MenuComponent> components = new List<MenuComponent>();
        private SpriteFont buttonFont;
        private Texture2D buttonTexture;
        //
        private Texture2D TileSelector;

        public Rectangle TileSelectorBackground;
        private Rectangle TileSelectorField; 
        private Texture2D SelectedTileTexture;
       
        private int[] Tiles;

        public enum EditorMode
        {
            Default,
            TileSelector,
            Place,
            Rubber,
            ObjectEdit,
            ObjectAdd,
            EntityAdd,
        }
        public EditorMode currentEditorMode = EditorMode.Default;

        public enum ObjectAddMode
        {
            Teleporter,
            Collision,
        }
        public ObjectAddMode currentObjectAddmode = ObjectAddMode.Collision;
        bool editorBackground = true;
        public bool tilesetScroll { get; private set; } = false;

        #endregion

        public void ChangeResolution()
        {
            TileSelectorBackground = new Rectangle(0, 0, 256, Game1.screenHeight);
            TileSelectorField = new Rectangle(0, Game1.screenHeight - 256, TileSelectorBackground.Width, Game1.screenHeight / 2);

            #region Buttons

            int xMid = TileSelectorBackground.Width / 2;       
            components.Clear();            
            components.Add(new Button(new Rectangle(xMid - 100, 10, 50, 30), "-", "layer-")); //LAYER
            components.Add(new Button(new Rectangle(xMid - 100 + 50, 10, 100, 30), "BG 0", "bfground")); //LAYER ANZEIGE
            components.Add(new Button(new Rectangle(xMid - 100 + 150, 10, 50, 30), "+", "layer+")); //LAYER
            components.Add(new Button(new Rectangle(xMid - 100, 45, 100, 30), "Remove", "removelayer")); //RemoveLayer
            components.Add(new Button(new Rectangle(xMid - 100 + 100, 45, 100, 30), "ADD", "addlayer")); //AddLayer
            components.Add(new Button(new Rectangle(xMid - 100, 80, 50, 30), "-", "tileset-")); //TileSet-
            components.Add(new Button(new Rectangle(xMid - 100 + 50, 80, 100, 30), "Tileset", "tilesetcounter")); //Tileset ANZEIGE
            components.Add(new Button(new Rectangle(xMid - 100 + 150, 80, 50, 30), "+", "tileset+")); //TileSet+
            components.Add(new Button(new Rectangle(xMid - 100, 115, 100, 30), "Remove", "removetileset")); //TileSet Remove COMING SOON
            components.Add(new Button(new Rectangle(xMid - 100+100, 115, 100, 30), "Refresh", "refreshtileset")); //TileSet Refresh
            components.Add(new Button(new Rectangle(xMid - 100 , 150, 100, 30), "ADDCollision", "addCollision")); //COLLISON ADD
            components.Add(new Button(new Rectangle(xMid - 100 + 100, 150, 100, 30), "ADDEntity", "addentity")); //Object Add
            components.Add(new Button(new Rectangle(xMid - 100, 185, 200, 30), "Remove Objects", "removeobjects")); //Remove Objects

            foreach (var button in components) button.Click += new EventHandler(this.OnClick);
            #endregion
        }

        public void Init(Level level, Input input, Viewport viewport)
        {
            this.level = level;
            this.input = input;
            TilesetView = new Camera(viewport);   
        }

        public void loadContent(ContentManager cm)
        {
            this.cm = cm;
            TileSelector = cm.Load<Texture2D>("textures/ui/misc/Background");
            buttonTexture = cm.Load<Texture2D>("textures/ui/menu/Button");
            buttonFont = cm.Load<SpriteFont>("fonts/SegoeUIBlack");
            SelectedTileTexture = cm.Load<Texture2D>("textures/inventory/currentSlot");

            ChangeResolution();

            updateMap();
        }

        public void OnClick(object sender, EventArgs e)
        {
            currentEditorMode = EditorMode.Default;

            if(sender is Button)
            {
                Button b = (Button)sender;
                if (b.command == "layer-") ChangeLayer(currentLayerID - 1);
                if (b.command == "layer+") ChangeLayer(currentLayerID + 1);
                
                if (b.command == "removelayer") RemoveLayer();
                if (b.command == "addlayer") AddLayer();
                if (b.command == "addLayerForeGround") AddLayer(true);
                if (b.command == "addLayerBackGround") AddLayer(false);
                if (b.command == "tileset-") ChangeTileset(currentTilesetID - 1);
                if (b.command == "tileset+") ChangeTileset(currentTilesetID + 1);
                if (b.command == "newTileset") AddTileset();
                if (b.command == "addCollision")
                {
                    currentEditorMode = EditorMode.ObjectAdd;
                    currentObjectAddmode = ObjectAddMode.Collision;
                }
                if (b.command == "removeobjects") currentEditorMode = EditorMode.ObjectEdit;
                //if (b.command == "add") objectadd = true;
                if (b.command == "bfground")
                {
                    if (b.text.StartsWith("B")) editorBackground = false;                    
                    else editorBackground = true;                   
                    ChangeLayer(0);
                }
                if (b.command == "refreshtileset")
                {
                    TilesetView.Position = new Vector2(0, 0);
                    TilesetView.Zoom = 1f;
                }
                if (b.command == "addentity")
                {
                    currentEditorMode = EditorMode.EntityAdd;
                }

            }

            if (sender is Textfield)
            {
                Textfield t = (Textfield)sender;
                if (t.command == "addTileset") AddTileset(t.text);
            }  
        }

        public void updateMap()
        {
            editorBackground = true;
            if(level.CurrentMap.tilesets.Count > 0) ChangeTileset(0);
            if(level.CurrentMap.layers.Count > 0) ChangeLayer(0);
        }

        public void Update()
        {
            Vector2 mousePos = input.MousePos;
            Vector2 tilesetPos = TilesetView.ScreenToWorld(input.MousePos);
            Vector2 mapPos = input.getMapPos(input.MousePos);

            if (!tilesetScroll)
            {
                if (input.currentMouseState.ScrollWheelValue > input.previousMouseState.ScrollWheelValue && input.KeyDown(Keys.LeftControl)) Global.camera.AdjustZoom(0.2f);
                if (input.currentMouseState.ScrollWheelValue < input.previousMouseState.ScrollWheelValue && input.KeyDown(Keys.LeftControl)) Global.camera.AdjustZoom(-0.2f);
            }

            if (input.KeyPressed(Keys.Z) && input.KeyDown(Keys.LeftControl)) Undo();


            if (input.KeyPressed(Microsoft.Xna.Framework.Input.Keys.C) || input.MouseRightButtonPressed())
            {
                currentItemIDs = new int[0, 0];
                sourceRectangle = new Rectangle();
                currentEditorMode = EditorMode.Default;
            }

            foreach (var button in components)
            {
                button.update(input);
            }

            if (input.KeyPressed(Keys.E))
            {
                currentItemIDs = new int[1, 1] { { 0 } };
                sourceRectangle = new Rectangle();
                currentEditorMode = EditorMode.Rubber;
            }

            if (mousePos.X > TileSelectorField.X && mousePos.X < TileSelectorField.X + TileSelectorField.Width && mousePos.Y > TileSelectorField.Y && mousePos.Y < TileSelectorField.Y + TileSelectorField.Height)
            {
                if (input.currentMouseState.ScrollWheelValue > input.previousMouseState.ScrollWheelValue && input.KeyDown(Keys.LeftControl)) zoomTileset(0.2f);
                if (input.currentMouseState.ScrollWheelValue < input.previousMouseState.ScrollWheelValue && input.KeyDown(Keys.LeftControl)) zoomTileset(-0.2f);
                tilesetScroll = true;    
                if (input.ButtonDown(input.currentMouseState.LeftButton))
                {
                    currentEditorMode = EditorMode.TileSelector;
                }
            }else
            {
                tilesetScroll = false;
            }
            

            if(currentEditorMode == EditorMode.TileSelector)
            {
                if (mousePos.X > TileSelectorField.X && mousePos.X < TileSelectorField.X + TileSelectorField.Width && mousePos.Y > TileSelectorField.Y && mousePos.Y < TileSelectorField.Y + TileSelectorField.Height)
                {
                    if (input.ButtonDown(input.currentMouseState.LeftButton))
                    {
                        if (!Drag)
                        {
                            startID = ((int)tilesetPos.X / level.CurrentMap.TileWidth) + ((int)tilesetPos.Y - TileSelectorField.Y) / level.CurrentMap.TileHeight * (currentTileset.width);
                            Drag = true;
                        }
                    }
                    if (!input.ButtonDown(input.currentMouseState.LeftButton))
                    {
                        if (Drag)
                        {
                            endID = ((int)tilesetPos.X / level.CurrentMap.TileWidth) + ((int)tilesetPos.Y - TileSelectorField.Y) / level.CurrentMap.TileHeight * (currentTileset.width);


                            Rectangle endid = new Rectangle(level.getSourceRectangle(endID, currentTileset).X / level.CurrentMap.TileWidth, level.getSourceRectangle(endID, currentTileset).Y / level.CurrentMap.TileHeight, level.CurrentMap.TileHeight, level.CurrentMap.TileHeight);
                            Rectangle startid = new Rectangle(level.getSourceRectangle(startID, currentTileset).X / level.CurrentMap.TileWidth, level.getSourceRectangle(startID, currentTileset).Y / level.CurrentMap.TileHeight, level.CurrentMap.TileHeight, level.CurrentMap.TileHeight);
                            if (startID > endID || startid.X > endid.X || startid.Y > endid.Y) return;
                            currentItemIDs = new int[endid.X - startid.X + 1, endid.Y - startid.Y + 1];
                            for (int y = startid.Y; y <= endid.Y; y++)
                            {
                                for (int x = startid.X; x <= endid.X; x++)
                                {
                                    currentItemIDs[x - startid.X, y - startid.Y] = Tiles[x + y * currentTileset.width];
                                }
                            };
                            sourceRectangle = new Rectangle(startid.X * level.CurrentMap.TileWidth, startid.Y * level.CurrentMap.TileHeight, (endid.X - startid.X) * level.CurrentMap.TileWidth + level.CurrentMap.TileWidth, (endid.Y - startid.Y) * level.CurrentMap.TileHeight + level.CurrentMap.TileHeight);
                            Drag = false;
                            currentEditorMode = EditorMode.Place;
                        }
                    }
                }
            }

            if (input.MousePos.X < TileSelectorBackground.Width) return; // STOP IF CURSOR IS AT OVERLAY

            switch (currentEditorMode)
            {
                case EditorMode.Default:
                   
                    break;

                case EditorMode.Place:
                  
                    if (input.ButtonDown(input.currentMouseState.LeftButton))
                    {
                        int mapX = (int)(mapPos.X) / level.CurrentMap.TileWidth;
                        int mapY = (int)(mapPos.Y) / level.CurrentMap.TileHeight;
                        if (mapX < 0 || mapY < 0 || mapX + currentItemIDs.GetLength(0) > level.CurrentMap.MapWidth || mapY + currentItemIDs.GetLength(1) > level.CurrentMap.MapHeight) return;
                        int[,] restoreIDs = new int[currentItemIDs.GetLength(0), currentItemIDs.GetLength(1)];
                        bool change = false;

                        for (int y = mapY; y < mapY + currentItemIDs.GetLength(1); y++)
                        {
                            for (int x = mapX; x < mapX + currentItemIDs.GetLength(0); x++)
                            {
                                if (currentLayer[x + y * level.CurrentMap.MapWidth] != currentItemIDs[x - mapX, y - mapY]) change = true;
                                restoreIDs[x - mapX, y - mapY] = currentLayer[x + y * level.CurrentMap.MapWidth];
                                currentLayer[x + y * level.CurrentMap.MapWidth] = currentItemIDs[x - mapX, y - mapY];
                            }
                        }

                        if (change) undo.Push(new Backup(new Vector2(mapX, mapY), restoreIDs, currentLayerID));

                    }

                    break;

                case EditorMode.Rubber:

                    if (input.ButtonDown(input.currentMouseState.LeftButton))
                    {
                        int mapX = (int)(mapPos.X) / level.CurrentMap.TileWidth;
                        int mapY = (int)(mapPos.Y) / level.CurrentMap.TileHeight;
                        if (mapX < 0 || mapY < 0 || mapX + currentItemIDs.GetLength(0) > level.CurrentMap.MapWidth || mapY + currentItemIDs.GetLength(1) > level.CurrentMap.MapHeight) return;
                        int[,] restoreIDs = new int[1, 1];
                        bool change = false;

                        if (currentLayer[mapX + mapY * level.CurrentMap.MapWidth] != 0) change = true;
                        restoreIDs[0, 0] = currentLayer[mapX + mapY * level.CurrentMap.MapWidth];
                        currentLayer[mapX + mapY * level.CurrentMap.MapWidth] = 0;
                                             
                        if (change) undo.Push(new Backup(new Vector2(mapX, mapY), restoreIDs, currentLayerID));
                    }

                    break;
               
                case EditorMode.ObjectAdd:
                    if (input.ButtonDown(input.currentMouseState.LeftButton) && !objectdrag)
                    {
                        objectposition.X = (int)mapPos.X;
                        objectposition.Y = (int)mapPos.Y;
                        objectdrag = true;
                    }
                    if (objectdrag)
                    {
                        objectposition.Width = (int)mapPos.X - objectposition.X;
                        objectposition.Height = (int)mapPos.Y - objectposition.Y;
                    }

                    if (!input.ButtonDown(input.currentMouseState.LeftButton) && objectdrag)
                    {
                        if (objectposition.X < 0)
                        {
                            objectposition.Width += objectposition.X;
                            objectposition.X = 0;
                        }
                        if (objectposition.Y < 0)
                        {
                            objectposition.Height += objectposition.Y;
                            objectposition.Y = 0;
                        }
                        if (objectposition.Width + objectposition.X > level.CurrentMap.MapWidth * level.CurrentMap.TileWidth) objectposition.Width = level.CurrentMap.MapWidth * level.CurrentMap.TileWidth - objectposition.X;
                        if (objectposition.Height + objectposition.Y > level.CurrentMap.MapHeight * level.CurrentMap.TileHeight) objectposition.Height = level.CurrentMap.MapHeight * level.CurrentMap.TileHeight - objectposition.Y;

                        if (objectposition.Width > 0 && objectposition.Height > 0)
                        {
                            if (currentObjectAddmode == ObjectAddMode.Collision) level.CurrentMap.collisions.Add(objectposition);
                            else if (currentObjectAddmode == ObjectAddMode.Teleporter)
                            {
                                level.CurrentMap.teleporters.Add(new Teleporter(objectposition, teleporterparams.mapID, teleporterparams.tpCoordinates));
                                currentEditorMode = EditorMode.Default;
                            }

                            //ADD COllison or teleporter
                        }
                        objectposition = new Rectangle();
                        objectdrag = false;
                    }
                    break;

                case EditorMode.ObjectEdit:

                    if (input.MouseLeftButtonPressed())
                    {
                        Rectangle obj = new Rectangle();
                        foreach (var coll in level.CurrentMap.collisions)
                        {
                            if (CollisionDetector.Collison(coll, mapPos))
                            {
                                obj = coll;
                                break;
                            }
                        }
                        Furniture fur = null;
                        foreach (var coll in level.CurrentMap.entities)
                        {
                            if (CollisionDetector.Collison(new Rectangle((int)coll.Position.X + coll.clickbox.X, (int)coll.Position.Y + coll.clickbox.Y, coll.clickbox.Width, coll.clickbox.Height), mapPos))
                            {
                                fur = coll;
                                break;
                            }
                        }
                        Teleporter tp = null;
                        foreach (var coll in level.CurrentMap.teleporters)
                        {
                            if (CollisionDetector.Collison(coll.position, mapPos))
                            {
                                tp = coll;
                                break;
                            }
                        }
                        if (obj.Width != 0)
                        {
                            level.CurrentMap.collisions.Remove(obj);
                        }
                        if (fur != null)
                        {
                            level.CurrentMap.entities.Remove(fur);
                            level.entities.Remove(fur);
                        }
                        if (tp != null)
                        {
                            level.CurrentMap.teleporters.Remove(tp);
                        }
                    }
                    break;
            }       
        }

        public void cmd(string[] param)
        {
            if (param[0] == "!map") updateMap();
            if (param[0] == "!loadmap")
            {
                Map map = SaveGame.DeserializeMap<Map>(param[1]);
                level.LoadMap(map);
                ChangeLayer(0);
                ChangeTileset(0);
            }
            if (param[0] == "!addtileset")
            {
                AddTileset("maps/tilesets/" + param[1]);
            }
            if (param[0] == "!addlayer")
            {
                AddLayer(bool.Parse(param[1]));
            }
            if (param[0] == "!newmap")
            {
                NewMap(new Map(param[1], int.Parse(param[2]), int.Parse(param[3]), int.Parse(param[4]), int.Parse(param[5])));
            }
            if (param[0] == "!setlayer")
            {
                ChangeLayer(Int32.Parse(param[1]));
            }
            if (param[0] == "!savemap")
            {
                SaveGame.SerializeMap<object>(param[1], (object)level.CurrentMap);
            }
            if (param[0] == "!fill")
            {
                FillLayer(0);
            }
            if (param[0] == "!addtp")
            {
                teleporterparams = new Teleporter(new Rectangle(), int.Parse(param[1]), new Vector2(float.Parse(param[2]), float.Parse(param[3])));
                currentEditorMode = EditorMode.ObjectAdd;
                currentObjectAddmode = ObjectAddMode.Teleporter;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            
            sb.Draw(TileSelector, TileSelectorBackground, Color.Black * 0.5f);
            foreach(var button in components)
            {
                button.draw(sb);
            }

            Vector2 size = input.getMapPos(new Vector2(sourceRectangle.Width * level.CurrentMap.TileWidth , sourceRectangle.Height * level.CurrentMap.TileHeight));

            int xStart = 10 + TileSelectorBackground.Width;
            sb.DrawString(buttonFont, currentEditorMode.ToString(), new Vector2(xStart, 130), Color.Black);
            
        }

        public void DrawPreview(SpriteBatch sb)
        {
            if(currentObjectAddmode == ObjectAddMode.Teleporter)
            {
                sb.Draw(TileSelector, objectposition, Color.Blue * 0.5f);
            }
            else sb.Draw(TileSelector, objectposition, Color.Red * 0.5f);
            if (Global.showHitboxes || currentEditorMode == EditorMode.ObjectAdd || currentEditorMode == EditorMode.ObjectEdit)
            {
                foreach (var coll in level.CurrentMap.collisions) sb.Draw(TileSelector, coll, Color.Red * 0.5f);
                foreach (var tp in level.CurrentMap.teleporters) sb.Draw(TileSelector, tp.position, Color.Blue * 0.5f);
            }          
            
            if(input.MousePos.X > TileSelectorBackground.Width && currentEditorMode == EditorMode.Place) sb.Draw(currentTileset.texture, new Rectangle(((int)input.getMapPos(input.MousePos).X)/ level.CurrentMap.TileWidth * level.CurrentMap.TileWidth, ((int)input.getMapPos(input.MousePos).Y) / level.CurrentMap.TileHeight * level.CurrentMap.TileHeight, sourceRectangle.Width, sourceRectangle.Height), sourceRectangle, Color.Gray * 0.5f);
        }

        public void moveTileset(Vector2 pos)
        {
            Vector2 position = TilesetView.Position + pos; //new Vector2(TileSelectorField.X, TileSelectorField.Y);
            if (true) //TODO CLAMP MOVEMENT
            {
                TilesetView.MoveCamera(pos);
            }
        }

        public void zoomTileset(float value)
        {
            TilesetView.AdjustZoom(value);
        }

        public void DrawTileset(SpriteBatch sb)
        {                    
                for (int y = (int)TileSelectorField.Y; y < currentTileset.texture.Height + TileSelectorField.Y; y+= level.CurrentMap.TileHeight)
                {
                    for (int x = 0; x < currentTileset.texture.Width; x+= level.CurrentMap.TileWidth)
                    {
                        Rectangle source = new Rectangle(x, y-(int)TileSelectorField.Y, level.CurrentMap.TileWidth, level.CurrentMap.TileHeight);
                        if (TilesetView.WorldToScreen(new Vector2(x, y)).X+ level.CurrentMap.TileWidth > TileSelectorBackground.Width || TilesetView.WorldToScreen(new Vector2(x, y)).Y < TileSelectorField.Y) continue;
                        sb.Draw(currentTileset.texture, new Rectangle(x, y, level.CurrentMap.TileWidth, level.CurrentMap.TileHeight),source, Color.White);
                    }
                }

                foreach(var id in currentItemIDs)
                {
                    Rectangle sourceRectangle = level.getSourceRectangle(id - currentTileset.startvalue - 1, currentTileset);
                    sb.Draw(SelectedTileTexture, new Rectangle(TileSelectorField.X + sourceRectangle.X, TileSelectorField.Y + sourceRectangle.Y, level.CurrentMap.TileWidth, level.CurrentMap.TileHeight), Color.White);
                }                           
        }

        public void NewMap(Map map)
        {           
            level.maps.Add(map);
            level.LoadMap(map);
            level.ChangeMap(level.maps.Count - 1);           
            AddLayer(true);
            AddLayer(false);
            AddTileset("maps/tilesets/tilesheetBasicTiles");           
            for(int i = 0; i < map.MapWidth*map.MapHeight; i++)
            {
                level.CurrentMap.layers[0][i] = 1;
            }      
        }

        public void ChangeTileset(int id)
        {
            if (id >= level.CurrentMap.tilesets.Count || id < 0) return;
            currentTilesetID = id;
            currentTileset = level.CurrentMap.tilesets[id];
            Tiles = new int[currentTileset.tileCount];

            for (int i = 0; i < Tiles.Length; i++) Tiles[i] = currentTileset.startvalue+i+1;         
        }
        public void AddTileset(string path)
        {
            Texture2D texture = cm.Load<Texture2D>(path);
            int startid = 0;
            if(level.CurrentMap.tilesets.Count > 0) startid = level.CurrentMap.tilesets[level.CurrentMap.tilesets.Count - 1].tileCount+1;
            Tileset tileset = new Tileset(startid, path);
            tileset.Init(texture, level.CurrentMap.TileWidth, level.CurrentMap.TileHeight);
            level.CurrentMap.tilesets.Add(tileset);
            ChangeTileset(level.CurrentMap.tilesets.Count-1);
        }

        public void AddTileset()
        {
            List<MenuComponent> options = new List<MenuComponent>();
            Textfield path = new Textfield(new Rectangle(), "path", "addTileset");         
            options.Add(path);
            foreach (var textfield in options) textfield.Click += new EventHandler(this.OnClick);
            Interface.addComponent(new OptionPane("Enter the path below", options, ""));
        }

        public void Undo()
        {
            if (undo.Count != 0)
            {
                Backup b = undo.Pop();

                for (int y = (int)b.position.Y; y < (int)b.position.Y + b.previousIDs.GetLength(1); y++)
                {
                    for (int x = (int)b.position.X; x < (int)b.position.X + b.previousIDs.GetLength(0); x++)
                    {
                        level.CurrentMap.layers[b.layer][x + y * level.CurrentMap.MapWidth] = b.previousIDs[x - (int)b.position.X, y - (int)b.position.Y];
                    }
                }
            }
        }

        public void ChangeLayer(int id)
        {
            if (id < 0) return;
            if (editorBackground)
            {
                if (id > level.CurrentMap.layers.Count) return;
                currentLayer = level.CurrentMap.layers[id];
                components[1].text = "BG " + id;
            }
            else
            {
                if (id > level.CurrentMap.flayers.Count) return;
                currentLayer = level.CurrentMap.flayers[id];
                components[1].text = "FG " + id;
            }
                       
            currentLayerID = id;
            
                      
        }
        public void RemoveLayer()
        {
            if(currentLayerID > level.CurrentMap.layers.Count - 1)
            {
                level.CurrentMap.flayers.RemoveAt(currentLayerID - level.CurrentMap.layers.Count);
            }else
            {
                level.CurrentMap.layers.RemoveAt(currentLayerID);
            }
           
            ChangeLayer(currentLayerID - 1);
        }
        public void AddLayer(bool foreground)
        {
            int[] newLayer = new int[level.CurrentMap.MapWidth * level.CurrentMap.MapHeight]; //MAY Set everything to 0
            if (foreground)
            {
                editorBackground = false;
                level.CurrentMap.flayers.Add(newLayer);
                ChangeLayer(level.CurrentMap.flayers.Count - 1);
            }
            else
            {
                editorBackground = true;
                level.CurrentMap.layers.Add(newLayer);
                ChangeLayer(level.CurrentMap.layers.Count -1);
            }
            
        }
        public void AddLayer()
        {          
            List<MenuComponent> options = new List<MenuComponent>();
            Button AddLayerF = new Button(new Rectangle(), "Foregound", "addLayerForeGround");
            Button AddLayerB = new Button(new Rectangle(), "Background", "addLayerBackGround");
            options.Add(AddLayerF);
            options.Add(AddLayerB);
            foreach (var button in options) button.Click += new EventHandler(this.OnClick);
            Interface.addComponent(new OptionPane("What layer type do you want?", options, ""));
        }
        public void FillLayer(int id)
        {
            for (int i = 0; i < level.CurrentMap.MapWidth * level.CurrentMap.MapHeight; i++)
            {
                level.CurrentMap.layers[0][i] = currentItemIDs[0,0];
            }
        }
    }

    class Backup
    {
        public readonly Vector2 position;
        public readonly int[,] previousIDs;
        public readonly int layer;
        public Backup(Vector2 position, int[,] ids, int layer)
        {
            this.position = position;
            this.previousIDs = ids;
            this.layer = layer;
        }
    }
}
