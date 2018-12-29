using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TestGame.src.modules;
using TestGame.src.tools;

namespace TestGame.src.entities.objects.furniture.chess
{
    class ChessTower : ChessFigure
    {
        public ChessTower(int x, int y, bool white, ChessGame chessgame) : base(x, y, white, chessgame)
        {

        }

        public ChessTower()
        {
        }


        public override void LoadContent(ContentManager cm)
        {
            if (whitecolor) source = new Microsoft.Xna.Framework.Rectangle(3 * 16, 16 * 16, 16, 16);
            else source = new Microsoft.Xna.Framework.Rectangle(3 * 16, 17 * 16, 16, 16);

            possiblePositions = new Vector2[] { };

            base.LoadContent(cm);
        }
      
        public override void UpdatePossiblePositions()
        {
            List<Vector2> posPos = new List<Vector2>();
            for (int no = 1, sw = -1; no < 10 && sw > -10; no++, sw--)
            {
                if (checkPos(posPos, no, 0)) break;
            }
            for (int no = 1, sw = -1; no < 10 && sw > -10; no++, sw--)
            {
                if (checkPos(posPos, 0, no)) break;
            }
            for (int no = 1, sw = -1; no < 10 && sw > -10; no++, sw--)
            {
                if (checkPos(posPos, sw, 0)) break;
            }
            for (int no = 1, sw = -1; no < 10 && sw > -10; no++, sw--)
            {
                if (checkPos(posPos, 0, sw)) break;
            }

            possiblePositions = posPos.ToArray();
        }
    }
}
