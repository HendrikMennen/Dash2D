using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TestGame.src.modules;
using TestGame.src.tools;

namespace TestGame.src.entities.objects.furniture
{
    class ChessKing : ChessFigure
    {
        private Vector2[] possiblePositionsTemplate;
        public ChessKing(int x, int y, bool white, ChessGame chessgame) : base(x, y, white, chessgame)
        {
                  
        }

        public ChessKing()
        {
        }
  

        public override void LoadContent(ContentManager cm)
        {       
            if(whitecolor) source = new Microsoft.Xna.Framework.Rectangle(4 * 16, 16 * 16, 16, 16);
            else source = new Microsoft.Xna.Framework.Rectangle(4*16, 17 * 16, 16, 16);

            possiblePositions = new Vector2[0] { };
            possiblePositionsTemplate = new Vector2[8]{ new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(0, -1), new Vector2(0, 1), new Vector2(1, -1), new Vector2(1, 0), new Vector2(1, 1) };

            base.LoadContent(cm);
        }

        public override void UpdatePossiblePositions()
        {
            List<Vector2> posPos = new List<Vector2>();
            foreach (var temp in possiblePositionsTemplate)
            {
                if (chessgame.positionFreeOffset(this, (int)temp.X, (int)temp.Y) || chessgame.positionEnemyOffset(this, (int)temp.X, (int)temp.Y)) posPos.Add(temp);
            }
            possiblePositions = posPos.ToArray();
        }
    }
}
