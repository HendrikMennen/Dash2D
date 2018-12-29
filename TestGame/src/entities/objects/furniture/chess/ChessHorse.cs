using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.modules;
using TestGame.src.tools;

namespace TestGame.src.entities.objects.furniture.chess
{
    class ChessHorse : ChessFigure
    {
        private Vector2[] possiblePositionsTemplate;
        public ChessHorse(int x, int y, bool white, ChessGame chessgame) : base(x, y, white, chessgame)
        {

        }

        public ChessHorse()
        {
        }


        public override void LoadContent(ContentManager cm)
        {
            if (whitecolor) source = new Microsoft.Xna.Framework.Rectangle(0 * 16, 16 * 16, 16, 16);
            else source = new Microsoft.Xna.Framework.Rectangle(0 * 16, 17 * 16, 16, 16);
            possiblePositionsTemplate = new Vector2[8] { new Vector2(-2, -1), new Vector2(-1, -2), new Vector2(1, -2), new Vector2(2, -1), new Vector2(2, 1), new Vector2(1, 2), new Vector2(-1, 2), new Vector2(-2, 1) };
            possiblePositions = new Vector2[0] { };

            base.LoadContent(cm);
        }

        
        public override void UpdatePossiblePositions()
        {
            List<Vector2> posPos = new List<Vector2>();
            foreach(var temp in possiblePositionsTemplate)
            {
                if (chessgame.positionFreeOffset(this, (int)temp.X, (int)temp.Y) || chessgame.positionEnemyOffset(this, (int)temp.X, (int)temp.Y)) posPos.Add(temp);
            }
            possiblePositions = posPos.ToArray();
        }
    }
}
