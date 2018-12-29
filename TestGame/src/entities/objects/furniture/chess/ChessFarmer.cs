using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.modules;
using TestGame.src.tools;

namespace TestGame.src.entities.objects.furniture.chess
{
    class ChessFarmer : ChessFigure
    {
        public ChessFarmer(int x, int y, bool white, ChessGame chessgame) : base(x, y, white, chessgame)
        {

        }

        public ChessFarmer()
        {
        }


        public override void LoadContent(ContentManager cm)
        {
            if (whitecolor) source = new Microsoft.Xna.Framework.Rectangle(2 * 16, 16 * 16, 16, 16);
            else source = new Microsoft.Xna.Framework.Rectangle(2 * 16, 17 * 16, 16, 16);
            possiblePositions = new Vector2[1] { new Vector2(0, 1) };

            base.LoadContent(cm);
        }

        public override void UpdatePossiblePositions()
        {
            List<Vector2> posPos = new List<Vector2>();
            if (whitecolor)
            {
                if (chessgame.positionFreeOffset(this, 0, -1)) posPos.Add(new Vector2(0, -1));
                if (chessgame.getArrayPos(this).Y == 6 && chessgame.positionFreeOffset(this, 0, -2)) posPos.Add(new Vector2(0, -2));
                if (chessgame.positionEnemyOffset(this, 1, -1)) posPos.Add(new Vector2(1, -1)); //SCHRÄG SCHLAGEN              
                if (chessgame.positionEnemyOffset(this, -1, -1)) posPos.Add(new Vector2(-1, -1)); //SCHRÄG SCHLAGEN

            }
            else
            {
                if (chessgame.positionFreeOffset(this, 0, 1)) posPos.Add(new Vector2(0, 1));
                if (chessgame.getArrayPos(this).Y == 1 && chessgame.positionFreeOffset(this, 0, 2)) posPos.Add(new Vector2(0, 2));
                if (chessgame.positionEnemyOffset(this, 1, 1)) posPos.Add(new Vector2(1, 1)); //SCHRÄG SCHLAGEN
                if (chessgame.positionEnemyOffset(this, -1, 1)) posPos.Add(new Vector2(-1, 1)); //SCHRÄG SCHLAGEN
            }
            possiblePositions = posPos.ToArray();
        }
    }
}
