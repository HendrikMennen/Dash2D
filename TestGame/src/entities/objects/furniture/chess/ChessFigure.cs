using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TestGame.src.entities.objects.furniture.chess;
using TestGame.src.items;
using TestGame.src.modules;
using TestGame.src.tools;
using System.Runtime.Serialization;

namespace TestGame.src.entities.objects.furniture
{
    [DataContract]
    abstract class ChessFigure : Furniture
    {
        
        public bool whitecolor { get; protected set; } = true;
        public bool moving { get; protected set; } = false;
        private int tempX = 0;
        private int tempY = 0;
        private bool deactivated = false;
        public int tx { get; set; } = 0;
        public int ty { get; set; } = 0;
        protected ChessGame chessgame;
        public Vector2[] possiblePositions { get; protected set; }
        private Texture2D possibleFieldTexture;
        public ChessFigure(int x, int y, bool white, ChessGame chessgame)
        {
            this.x = x;
            this.y = y;
            this.tx = x;
            this.ty = y;
            whitecolor = white;
            this.chessgame = chessgame;
            Position = new Microsoft.Xna.Framework.Vector2(x, y);
        }

        public ChessFigure() //for serialisation
        {
        }

        public override void LoadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>("textures/objects/furniture");

            width = source.Width;
            height = source.Height;

            hitbox = new Microsoft.Xna.Framework.Rectangle(0, 10, 16, 6);
            clickbox = new Microsoft.Xna.Framework.Rectangle(0,0, 16, 16);
            possibleFieldTexture = cm.Load<Texture2D>("textures/ui/misc/Background");
            base.LoadContent(cm);
        }

        protected bool checkPos(List<Vector2> posPos, int xa, int ya)
        {
            if (chessgame.positionFreeOffset(this, xa, ya))
            {
                posPos.Add(new Vector2(xa, ya));
                return false;
            }
            else if (chessgame.positionEnemyOffset(this, xa, ya))
            {
                posPos.Add(new Vector2(xa, ya));
                return true;
            }
            else return true; //BREAK 
        }

        public override void Update()
        {
            if(drag && !dragstart && !moving && !deactivated)
            {
                dragstart = true;
                UpdatePossiblePositions();
            }
            if (drag && dragstart)
            {
                tempX = (int)input.getMapPos(input.MousePos).X;
                tempY = (int)input.getMapPos(input.MousePos).Y;

                if(input.currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {   
                    chessgame.move(this, tempX / 16 * 16, tempY / 16 * 16);
                    drag = false;
                    dragstart = false;
                }
            }
            else
            {
                int xa = 0, ya = 0;
                if (x < tx) xa++;
                if (x > tx) xa--;
                if (y < ty) ya++;
                if (y > ty) ya--;

                if (xa == 0 && ya == 0) moving = false;
                else moving = true;
                if (!Collision(new Vector2(x + xa, y + ya), level))
                {
                    Position = new Vector2(xa + x, ya + y);
                }
            }
            base.Update();
        }

        public virtual void UpdatePossiblePositions()
        {

        }

        public void deactivate()
        {
            deactivated = true;
        }

        public override void Draw(SpriteBatch sb)
        {         
            if (level.getClientPlayer().selectedEntity == this && !moving && !deactivated)
            {
                bool validpos = false;
                int temptx = tempX / 16 * 16;
                int tempty = tempY / 16 * 16;
                foreach (var possibleplace in possiblePositions)
                {                  
                    if (temptx == x + (int)possibleplace.X * 16 && tempty == y + (int)possibleplace.Y * 16)
                    {                      
                        sb.Draw(possibleFieldTexture, new Rectangle(temptx, tempty, 16, 16), Color.Green * 0.5f); // DRAW POSSIBLE CELLS IF SELECTED
                        validpos = true;
                    }
                    else sb.Draw(possibleFieldTexture, new Rectangle(x + (int)possibleplace.X * 16, y + (int)possibleplace.Y * 16, 16, 16), Color.Blue * 0.5f);
                }

                if (!validpos)
                {
                    sb.Draw(possibleFieldTexture, new Rectangle(temptx, tempty, 16, 16), Color.Red * 0.5f);
                }
                
            }

            if (drag && !moving && !deactivated)
            {
                Rectangle draw = new Rectangle(tempX-8, tempY-8, width, height);
                sb.Draw(sprite, draw, source, Color.White * 0.4f);
            }
         
            base.Draw(sb);                      
        }    
          
    }
}
