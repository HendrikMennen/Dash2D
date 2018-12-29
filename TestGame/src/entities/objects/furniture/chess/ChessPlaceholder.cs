using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TestGame.src.tools;

namespace TestGame.src.entities.objects.furniture.chess
{
    class ChessPlaceholder : ChessFigure
    {
       
        public ChessPlaceholder()
        {
        }

        public override void LoadContent(ContentManager cm)
        {
            if (whitecolor) source = new Microsoft.Xna.Framework.Rectangle(0 * 16, 16 * 16, 16, 16);
            else source = new Microsoft.Xna.Framework.Rectangle(0 * 16, 17 * 16, 16, 16);
            possiblePositions = new Vector2[] { };

            base.LoadContent(cm);
        }

        public override void Draw(SpriteBatch sb)
        {
            //base.Draw(sb); Nicht zeichnen
        }
    }
}
