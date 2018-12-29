using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TestGame.src.input;
using TestGame.src.level;
using TestGame.src.graphics.ui;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.Serialization;

namespace TestGame.src.items
{
    //MORE...
    public abstract class Item
    {
        public int ID;
        public Rectangle source;          
        public Inventory inv;       
        public Level level;
        
        public bool placeable = false;
        public string Name { get; set; } = "";
        //public virtual event System.EventHandler Activated;
        public delegate void EventHandler(object sender, EventArgs args);
    

        //Chess
        //public static Item chessking = new FurnitureItem<ChessKing>(5, 4, "Chess King");

        public Item()
        {

        }
        public void Init(Level level, Inventory inv)
        {
            this.level = level;
            this.inv = inv;
        }

        public virtual void LoadContent(ContentManager cm)
        {

        }       

        public virtual void Update(Input input)
        {
            
        }

        public virtual void Draw(SpriteBatch sb)
        {

        }
    }
}
