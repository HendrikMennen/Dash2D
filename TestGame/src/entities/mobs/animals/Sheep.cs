using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.entities.particles;
using Microsoft.Xna.Framework.Graphics;
using TestGame.src.input;
using TestGame.src.level;
using Microsoft.Xna.Framework.Content;
using TestGame.src.tools;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace TestGame.src.entities
{
    [DataContract]
    public class Sheep : Npc
    {
        
        private Texture2D sprite_walk;
        
        public Color sheepcolor = Color.White;
        
        private int count = 0, countmax = 20;
        public Sheep(int x, int y)
        {
            this.x = x;
            this.y = y;
            Position = new Vector2(x, y);            
        }

        public Sheep()
        {

        }

        public override void LoadContent(ContentManager cm)
        {
            animationSpeed = 9;
            rows = 3;
            columns = 4;
            speed = 1f;

            hitbox = new Rectangle(13, 26, 13, 12);

            sprite_walk = cm.Load<Texture2D>("textures/models/sheep/sheep_walk");
            sprite = sprite_walk;
            base.LoadContent(cm);
        }

        public override void Draw(SpriteBatch sb)
        {
            Draw(sb, Color.White);
        }

        public override void Update() //CLIENT SIDE ONLY
        {          
            if (direction == 1 || direction == 3) hitbox = new Rectangle(13, 26, 13, 12);
            else hitbox = new Rectangle(6, 34, 29 ,6);

            Moveto();
            base.Update();                        
        }

        public override void FixedUpdate()
        {
            moveRandom();          
            base.FixedUpdate();
        }
    }
}
