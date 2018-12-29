using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.entities;
using TestGame.src.graphics;
using TestGame.src.input;

namespace TestGame.src.items
{
    class SpawnItem<T> : Item
    {
        bool drawAllowed;
        int mapX = 0;
        int mapY = 0;
        Entity mob;


        public SpawnItem(int column, int row, string name)
        {
            mob = (Entity)Activator.CreateInstance(typeof(T));

            source = new Rectangle(column * 16, row * 16, 16, 16); //ItemSourceRectangle

            this.Name = name;
        }

        public override void LoadContent(ContentManager cm)
        {
            mob.LoadContent(cm);
            placeable = true;
        }

        public override void Update(Input input)
        {
            mapX = (int)input.GetMapPos(input.MousePos).X;
            mapY = (int)input.GetMapPos(input.MousePos).Y;
            mob.Position = new Vector2(mapX, mapY);

            bool draw = true;

            if (mob.Collision(mob.Position,level)) draw = false;
            
            drawAllowed = draw;
            if (input.CurrentMouseState.LeftButton == ButtonState.Released)
            {
                if (drawAllowed)
                {
                    Entity instance = (Entity)Activator.CreateInstance(typeof(T));
                    instance.mapid = level.getClientPlayer().mapid;
                    instance.Position = mob.Position;
                    if (Game1.online) netcode.NetCode.addEntity(instance);
                    else level.AddEntity(instance);
                    //Activated(this, null);                    
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

            mob.Draw(sb, renderColor);
        }
    }
}
    

