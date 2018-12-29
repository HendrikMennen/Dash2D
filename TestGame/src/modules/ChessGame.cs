using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TestGame.src.entities.objects.furniture;
using TestGame.src.entities.objects.furniture.chess;
using TestGame.src.level;

namespace TestGame.src.modules
{
    class ChessGame : MapModule
    {
        private ChessFigure[,] chessfield;
        private int x, y;
        private int deadyOffsetWhite = -2;
        private int deadyOffsetBlack = -10;
        

        public ChessGame(int x, int y)
        {
            chessfield = new ChessFigure[8,8];
            this.x = x;
            this.y = y;
            reset();
        }

        public void move(ChessFigure f, int x, int y)
        {
            int tempx = ((int)f.Position.X - x) / 16;
            int tempy = ((int)f.Position.Y - y) / 16;
            Vector2 oldpos = getArrayPos(f);
            Vector2 newpos = new Vector2((int)oldpos.X - tempx, (int)oldpos.Y - tempy);

            
            foreach(var posPosition in f.possiblePositions) //CHECK IF POSITION IS POSSIBLE
            {
                if(posPosition.Equals(new Vector2(tempx, tempy)*-1))
                {
                    ChessFigure newPos = getPosition((int)newpos.X, (int)newpos.Y);
                    if (newPos == null) return;
                    if (newPos is ChessPlaceholder || newPos.whitecolor!= f.whitecolor)
                    {
                        if(!(chessfield[(int)oldpos.X - tempx, (int)oldpos.Y - tempy] is ChessPlaceholder))
                        {
                            //level.remEntity(chessfield[(int)oldpos.X - tempx, (int)oldpos.Y - tempy]);
                            if (newPos.whitecolor)
                            {
                                chessfield[(int)oldpos.X - tempx, (int)oldpos.Y - tempy].tx = this.x - 18;
                                chessfield[(int)oldpos.X - tempx, (int)oldpos.Y - tempy].ty = this.y + (112 - deadyOffsetWhite);
                                deadyOffsetWhite += 8;
                            }
                            else
                            {
                                chessfield[(int)oldpos.X - tempx, (int)oldpos.Y - tempy].tx = this.x + 130;
                                chessfield[(int)oldpos.X - tempx, (int)oldpos.Y - tempy].ty = this.y + deadyOffsetBlack;
                                deadyOffsetBlack += 8;
                            }
                            chessfield[(int)oldpos.X - tempx, (int)oldpos.Y - tempy].deactivate();
                        }
                        chessfield[(int)oldpos.X - tempx, (int)oldpos.Y - tempy] = f;
                        chessfield[(int)oldpos.X, (int)oldpos.Y] = new ChessPlaceholder();

                        f.tx = x;
                        f.ty = y;

                        return;
                    }
                }
            }    
        }

        public bool positionEnemyOffset(ChessFigure f, int x, int y)
        {
            ChessFigure offset = getPositionOffset(f, x, y);
            if (offset != null && !( offset is ChessPlaceholder) && offset.whitecolor != f.whitecolor) return true;
            return false;
        }
        public bool positionFreeOffset(ChessFigure f, int x, int y) 
        {
            if (getPositionOffset(f, x, y) is ChessPlaceholder) return true;
            return false;
        }

        public ChessFigure getPositionOffset(ChessFigure f, int x, int y)
        {
            Vector2 pos = getArrayPos(f);
            x = (int)pos.X + x;
            y = (int)pos.Y + y;
            return getPosition(x, y);
        }

        public bool positionFree(int x, int y)
        {
            if (getPosition(x, y) is ChessPlaceholder) return true;
            return false;
        }

        public ChessFigure getPosition(int x, int y)
        {
            if (x < 0 || y < 0 || x >= 8 || y >= 8) return null;
            else return chessfield[x, y];
        }


        public Vector2 getArrayPos(ChessFigure f) //TODO BETTER RUNTIME POSSIBLE
        {
            for (int xa = 0; xa < chessfield.GetLength(0); xa++)
            {
                for (int ya = 0; ya < chessfield.GetLength(0); ya++)
                {
                    if (chessfield[xa, ya] == f)
                    {
                        return new Vector2(xa, ya);
                    }
                }
            }
            return new Vector2(0, 0);
        }

        public void init(Level level)
        {
            this.level = level;
                
        }

        public override void LoadContent(ContentManager cm)
        {
            //show(true);
        }

        public void reset()
        {
            chessfield = new ChessFigure[8, 8];
 
            for (int xa = 0; xa < chessfield.GetLength(0); xa++)
            {
                for (int ya = 0; ya < chessfield.GetLength(0); ya++)
                {
                    chessfield[xa,ya] = new ChessPlaceholder();
                }
            }
           
            //BLACK
            chessfield[0, 0] = new ChessTower(x, y, false, this);
            chessfield[1, 0] = new ChessHorse(x + 16, y, false, this);
            chessfield[2, 0] = new ChessRunner(x + 32, y , false, this);
            chessfield[3, 0] = new ChessQueen(x+48, y, false, this);
            chessfield[4, 0] = new ChessKing(x+64, y, false, this);
            chessfield[5, 0] = new ChessRunner(x + 80, y, false, this);
            chessfield[6, 0] = new ChessHorse(x + 96, y, false, this);
            chessfield[7, 0] = new ChessTower(x + 112, y, false, this);

            chessfield[0, 1] = new ChessFarmer(x, y + 16, false, this);
            chessfield[1, 1] = new ChessFarmer(x + 16, y + 16, false, this);
            chessfield[2, 1] = new ChessFarmer(x + 32, y + 16, false, this);
            chessfield[3, 1] = new ChessFarmer(x + 48, y + 16, false, this);
            chessfield[4, 1] = new ChessFarmer(x + 64, y + 16, false, this);
            chessfield[5, 1] = new ChessFarmer(x + 80, y + 16, false, this);
            chessfield[6, 1] = new ChessFarmer(x + 96, y + 16, false, this);
            chessfield[7, 1] = new ChessFarmer(x + 112, y + 16, false, this);

            //WHITE
            chessfield[0, 7] = new ChessTower(x, y+ 112, true, this);
            chessfield[1, 7] = new ChessHorse(x + 16, y + 112, true, this);
            chessfield[2, 7] = new ChessRunner(x + 32, y + 112, true, this);
            chessfield[3, 7] = new ChessQueen(x + 48, y + 112, true, this);
            chessfield[4, 7] = new ChessKing(x + 64, y + 112, true, this);
            chessfield[5, 7] = new ChessRunner(x + 80, y + 112, true, this);
            chessfield[6, 7] = new ChessHorse(x + 96, y + 112, true, this);
            chessfield[7, 7] = new ChessTower(x + 112, y + 112, true, this);

            chessfield[0, 6] = new ChessFarmer(x, y + 96, true, this);
            chessfield[1, 6] = new ChessFarmer(x + 16, y + 96, true, this);
            chessfield[2, 6] = new ChessFarmer(x + 32, y + 96, true, this);
            chessfield[3, 6] = new ChessFarmer(x + 48, y + 96, true, this);
            chessfield[4, 6] = new ChessFarmer(x + 64, y + 96, true, this);
            chessfield[5, 6] = new ChessFarmer(x + 80, y + 96, true, this);
            chessfield[6, 6] = new ChessFarmer(x + 96, y + 96, true, this);
            chessfield[7, 6] = new ChessFarmer(x + 112, y + 96, true, this);
        }

        public override void Activate(bool activate, int mapID)
        {
            
            foreach (var chessfigure in chessfield)
            {
                if (chessfigure is null || chessfigure is ChessPlaceholder) continue;
                chessfigure.mapid = mapID;
                if (activate) level.AddEntity(chessfigure);
                else level.remEntity(chessfigure);
            }
            base.Activate(activate, mapID);
        }
    }
}
