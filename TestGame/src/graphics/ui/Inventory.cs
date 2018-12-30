using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using TestGame.src.entities;
using TestGame.src.entities.objects.furniture;
using TestGame.src.entities.projectiles;
using TestGame.src.input;
using TestGame.src.items;
using TestGame.src.level;
using TestGame.src.tools;
using static TestGame.Game1;

namespace TestGame.src.graphics.ui
{
    public class Inventory
    {
        Texture2D invBar, invOpen;
        Texture2D items;
        Texture2D currentSlotTexture;
        SpriteFont invnumberfont, itemnamefont;
        Rectangle barPos, invPos;
        ContentManager cm;
        Level level;
        Commands cmd;
        readonly StaticItems s = new StaticItems();
        public bool drag = false;
        public bool dragEntity = false;
        private int barPosX, barPosY;
        private int scale = 3;
        private int width = Game1.screenWidth;
        private int height = Game1.screenHeight;
        private int invWidth = 10, invHeight=10;
        private readonly bool open = false;
        private int scroll = 0;
        private int Scroll
        {
            get
            {
                return scroll;
            }
            set
            {
                if (value >= 0) scroll = value;
            }
        }

        public static int currentSlot=1;
       

        protected static Item[] inventory = new Item[9 * 4];
        protected int[] itemAnzahl = new int[9 * 4];
        private Item currentItem;
        public Item CurrentItem
        {
            get
            {
                return currentItem;
            }     
            private set
            {                
                if (currentItem != null) currentItem.Activated -= new EventHandler(OnActivate); //EVENT LÖSEN
                level.getClientPlayer().currentItem = value;
                currentItem = value;
                currentItem.Activated += new EventHandler(OnActivate); //NEUES EVENT VERKNÜPFEN
            }       
        }

        public void LoadContent(ContentManager cm)
        {
            this.cm = cm;
            s.LoadContent(cm);
            for (int i = 0; i < inventory.Length; i++)
            {
                inventory[i] = StaticItems.transparent;
                itemAnzahl[i] = 0;
            }

            itemnamefont = cm.Load<SpriteFont>("fonts/SegoeUIBlack");
            invnumberfont = cm.Load<SpriteFont>("fonts/InvNumber");      
            invBar = cm.Load<Texture2D>("textures/inventory/inventory");
            invOpen = cm.Load<Texture2D>("textures/inventory/inventoryOpen");
            currentSlotTexture = cm.Load<Texture2D>("textures/inventory/currentSlot");          
            items = cm.Load<Texture2D>("textures/inventory/items");

            invWidth = invBar.Width;
            invHeight = invBar.Height;
            UpdateResolution();
            #if true

            AddItem(StaticItems.particlespawner, 10);
            AddItem(StaticItems.sofa, 10);
            AddItem(StaticItems.tv, 10);
            AddItem(StaticItems.speaker, 4);
            AddItem(StaticItems.discolight, 20);
            AddItem(StaticItems.lamp1, 10);
            AddItem(StaticItems.torch, 100);
            AddItem(StaticItems.magicglass, 10);
            AddItem(StaticItems.cabinet1, 5);
            AddItem(StaticItems.plant1, 10);
            AddItem(StaticItems.fence, 50);
            AddItem(StaticItems.redjacket, 1);
            AddItem(StaticItems.yellowjacket, 1);
            AddItem(StaticItems.blackjacket, 1);          
            AddItem(StaticItems.whitejacket, 1);
            //addItem(Item.book1, 1);
            AddItem(StaticItems.beer, 2);
            //addItem(Item.disc_GhostBusters, 1);
            //addItem(Item.disc_Night_Drive, 1);

            AddItem(StaticItems.sheep, 30);
            //addItem(Item.joseph, 10);

            AddItem(StaticItems.grass, 10);
            AddItem(StaticItems.tree, 10);
            AddItem(StaticItems.palm, 5);
            AddItem(StaticItems.fir, 100);

            //addItem(Item.cristmasStar, 3);

            //addItem(Item.chessking, 2);
#endif
            CurrentSlot = 1;

        }

        public void OnActivate(object sender, EventArgs arg)
        {
            RemItem(currentSlot);
            drag = false;
            if (sender is BasicItem) {
                cmd.command(((BasicItem)sender).command);
            }            
        }

        public void AddItem(Item item, int anzahl)
        {
            item.Init(level, this);
            item.LoadContent(cm);

            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == item)
                {
                    itemAnzahl[i] += anzahl;
                    return;
                }
            }
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == StaticItems.transparent)
                {
                    inventory[i] = item;
                    itemAnzahl[i] += anzahl;

                    return;
                }
            }
        }
        public void AddItem(int id, int anzahl)
        {
            
        }

        public void RemItem(int n)
        {
            if (itemAnzahl[n] > 0)
            {
                itemAnzahl[n] -= 1;
                if (itemAnzahl[n] < 1) inventory[n] = StaticItems.transparent;
            }
            else inventory[n] = StaticItems.transparent;
        }

        public void Init(Level level, Commands cmd)
        {
            this.level = level;
            this.cmd = cmd;
            s.Init(level, this);
        }

        public int CurrentSlot
        {
            get
            {
                return currentSlot;
            }
            set
            {
                currentSlot = value;
                if (currentSlot > 8) currentSlot = 0;
                else if (currentSlot < 0) currentSlot = 8;
            }
        }

        public void Update(Input input)
        {
            UpdateSlot(input);
            CurrentItem = inventory[currentSlot+Scroll];

            
            if (CurrentItem is GunItem<Projectile>)
            {
                CurrentItem.Update(input);
            }
            else
            {
                if (!drag)
                {
                    if (input.CurrentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (input.MousePos.X > barPos.X && input.MousePos.X < barPos.X + barPos.Width && input.MousePos.Y > barPos.Y && input.MousePos.Y < barPos.Y + barPos.Height)
                        {
                            CurrentSlot = (int)((input.MousePos.X - barPos.X) / (barPos.Width / 9));

                            if (CurrentItem.placeable) drag = true;
                        }
                    }
                    else
                    {
                       
                        if (!CurrentItem.placeable) CurrentItem.Update(input);
                    }
                }
                else
                {
                    CurrentItem.Update(input);
                }
            }

            if (input.MouseLeftButtonPressed())
            {
                // ONCLICK Furniture
                bool unselect = true;
                foreach (var furn in level.furniture)
                {
                    if (furn.mapid == level.mapID && CollisionDetector.Collison(new Rectangle(furn.clickbox.X + (int)furn.Position.X, furn.clickbox.Y + (int)furn.Position.Y, furn.clickbox.Width, furn.clickbox.Height), input.GetMapPos(input.MousePos)))
                    {
                        if (Game1.online)
                        {
                            netcode.NetCode.OnClick(furn, (Item)StaticItems.transparent);
                        }
                        else
                        {
                            furn.OnClick(CurrentItem);
                            level.getClientPlayer().SelectedEntity = furn;
                            unselect = false;
                        }                      
                    }
                }
                if (unselect)
                {
                    level.getClientPlayer().SelectedEntity = null;                  
                }
            }

            if (input.CurrentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (!dragEntity) //SELECT NEW ENTITY
                {
                    foreach (var furn in level.furniture)
                    {
                        if (furn.mapid == level.mapID && CollisionDetector.Collison(new Rectangle(furn.clickbox.X+ (int)furn.Position.X, furn.clickbox.Y+ (int)furn.Position.Y, furn.clickbox.Width, furn.clickbox.Height), input.GetMapPos(input.MousePos)))
                        {
                            level.getClientPlayer().SelectedEntity = furn;
                            dragEntity = true;
                        }
                    }
                }
                else
                {
                    ((Furniture)level.getClientPlayer().SelectedEntity).Drag = true;
                }
            }
            else
            {
                dragEntity = false;
                level.getClientPlayer().SelectedEntity = null;
            }
                   
        }

        public void Draw(SpriteBatch g)
        {
            g.Draw(invBar, barPos, Color.White); // Draw Item bar
            for(int i = 0; i < 9; i++)
            {
                if(inventory[i + Scroll] is ClothItem) g.Draw(items, new Rectangle((barPosX + i * 19 * scale + 2 * scale), (barPosY + 2 * scale), 16 * scale, 16 * scale), inventory[i + Scroll].source, ((ClothItem)inventory[i + Scroll]).Clothcolor);
                else g.Draw(items, new Rectangle((barPosX+i*19*scale+ 2*scale) ,(barPosY+2*scale), 16*scale, 16*scale), inventory[i+Scroll].source, Color.White);
                if(inventory[i+Scroll]!= StaticItems.transparent) g.DrawString(invnumberfont, itemAnzahl[i+Scroll].ToString(), new Vector2(barPosX+i*19*scale+18*scale-itemnamefont.MeasureString(itemAnzahl[i+Scroll].ToString()).X, barPosY+12*scale), Color.Azure);
            }
            g.Draw(currentSlotTexture, new Rectangle(barPosX + currentSlot * 19 * scale, barPosY, 20 * scale, 20 * scale), Color.White);
            g.DrawString(itemnamefont, CurrentItem.Name, new Vector2((barPosX + (invWidth*scale)/2)-itemnamefont.MeasureString(CurrentItem.Name).X/2, barPosY - itemnamefont.MeasureString(CurrentItem.Name).Y), Color.DarkBlue);

            if (open)
            {
                g.Draw(invOpen, invPos, Color.White);
            }            
        }
        public void DrawPreview(SpriteBatch sb)
        {
            if(drag) CurrentItem.Draw(sb);
        }

        public void UpdateResolution()
        {
            scale = 3;
            this.width = Game1.screenWidth;
            this.height = Game1.screenHeight;
            if (Game1.currentGamestate == GameState.Editor) barPosX = (width+256) / 2 + - invWidth * scale / 2;
            else barPosX = width / 2 - invWidth * scale / 2;
            barPosY = height - invHeight*scale - height / 30;
            barPos = new Rectangle(barPosX, barPosY, invWidth*scale, invHeight*scale);
            invPos = new Rectangle(width / 2 - invOpen.Width*scale / 2, height / 2 - invOpen.Height*scale / 2, invOpen.Width*scale, invOpen.Height*scale);
        }
        public void UpdateSlot(Input input)
        {
            if (input.CurrentMouseState.ScrollWheelValue > input.PreviousMouseState.ScrollWheelValue) CurrentSlot--;
            if (input.CurrentMouseState.ScrollWheelValue < input.PreviousMouseState.ScrollWheelValue) CurrentSlot++;
            //TODO FIND A BETTER SOLUTION :D
            if (input.KeyDown(Microsoft.Xna.Framework.Input.Keys.D1)) CurrentSlot = 0 + Scroll;
            if (input.KeyDown(Microsoft.Xna.Framework.Input.Keys.D2)) CurrentSlot = 1 + Scroll;
            if (input.KeyDown(Microsoft.Xna.Framework.Input.Keys.D3)) CurrentSlot = 2 + Scroll;
            if (input.KeyDown(Microsoft.Xna.Framework.Input.Keys.D4)) CurrentSlot = 3 + Scroll;
            if (input.KeyDown(Microsoft.Xna.Framework.Input.Keys.D5)) CurrentSlot = 4 + Scroll;
            if (input.KeyDown(Microsoft.Xna.Framework.Input.Keys.D6)) CurrentSlot = 5 + Scroll;
            if (input.KeyDown(Microsoft.Xna.Framework.Input.Keys.D7)) CurrentSlot = 6 + Scroll;
            if (input.KeyDown(Microsoft.Xna.Framework.Input.Keys.D8)) CurrentSlot = 7 + Scroll;
            if (input.KeyDown(Microsoft.Xna.Framework.Input.Keys.D9)) CurrentSlot = 8 + Scroll;

            if (input.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                if(Scroll > 0) Scroll -= 1;
            }
            if (input.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                Scroll += 1;
            }

            

        }
    }
}
