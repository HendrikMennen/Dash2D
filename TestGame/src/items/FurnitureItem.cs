

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using TestGame.src.entities;
using TestGame.src.entities.objects;
using TestGame.src.entities.objects.furniture;
using TestGame.src.graphics;
using TestGame.src.input;
using TestGame.src.tools;
using static TestGame.Game1;

namespace TestGame.src.items
{
    
    class FurnitureItem<T> : Item
    {
        bool drawAllowed = false;
        int mapX = 0;
        int mapY= 0;
        Furniture furniture;

        public FurnitureItem(int column, int row, string name)
        {
            this.Name = name;
            furniture = (Furniture)Activator.CreateInstance(typeof(T));
            //furniture = new Sofa();
            source = new Rectangle(column * 16, row * 16, 16, 16); //ItemSourceRectangle                              
        }

        public override void LoadContent(ContentManager cm)
        {
            furniture.LoadContent(cm);
            placeable = true;
        }

        public override void Update(Input input)
        {
            base.Update(input);
            mapX = (int)input.GetMapPos(input.MousePos).X/16*16;
            mapY = (int)input.GetMapPos(input.MousePos).Y/16*16;
            furniture.Position = new Vector2(mapX, mapY);

            bool draw = true;

            if (furniture.Collision(furniture.Position, level)) draw = false;
            
            drawAllowed = draw;
            if (input.CurrentMouseState.LeftButton == ButtonState.Released) 
            {                                
                if (drawAllowed)
                {
                    //Activated(this, null);
                    Furniture instance = (Furniture)Activator.CreateInstance(typeof(T));
                    instance.Position = furniture.Position;
                    instance.mapid = level.getClientPlayer().mapid;
                    if (Game1.online) netcode.NetCode.addEntity(instance);  
                    else level.AddEntity(instance);
                    if (Game1.currentGamestate == GameState.Editor) level.CurrentMap.entities.Add(instance);
                }else
                {
                    inv.drag = false;
                }
            }

        }
        public override void Draw(SpriteBatch sb)
        {
            Color renderColor;
            if (drawAllowed) renderColor = Color.White * 0.5f;
            else renderColor = Color.Red * 0.5f;
            
            furniture.Draw(sb, renderColor);         
        }
    }
}
