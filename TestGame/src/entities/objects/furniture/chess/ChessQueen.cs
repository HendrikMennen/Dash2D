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
    class ChessQueen : ChessFigure
    {
        public ChessQueen(int x, int y, bool white, ChessGame chessgame) : base(x, y, white, chessgame)
        {

        }

        public ChessQueen()
        {
        }


        public override void LoadContent(ContentManager cm)
        {
            if (whitecolor) source = new Microsoft.Xna.Framework.Rectangle(5 * 16, 16 * 16, 16, 16);
            else source = new Microsoft.Xna.Framework.Rectangle(5 * 16, 17 * 16, 16, 16);

            possiblePositions = new Vector2[] { };

            base.LoadContent(cm);
        }

        public override void UpdatePossiblePositions()
        {
            List<Vector2> posPos = new List<Vector2>(); //Verkürzen
            //Vertical & Horizontal
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

            //Diagonal
            for (int no = 1, sw = -1; no < 10 && sw > -10; no++, sw--)
            {
                if (checkPos(posPos, no, no)) break;
            }
            for (int no = 1, sw = -1; no < 10 && sw > -10; no++, sw--)
            {
                if (checkPos(posPos, no, sw)) break;
            }
            for (int no = 1, sw = -1; no < 10 && sw > -10; no++, sw--)
            {
                if (checkPos(posPos, sw, no)) break;
            }
            for (int no = 1, sw = -1; no < 10 && sw > -10; no++, sw--)
            {
                if (checkPos(posPos, sw, sw)) break;
            }

            possiblePositions = posPos.ToArray();
        }
    }
}
