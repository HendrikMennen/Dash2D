using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame
{
    class AnimatedTile : Tile
        
    {
        private AnimatedSprite animatedSprite;
        private int column, row, animationSpeed, animCounter;
        public AnimatedTile(string path, bool solid, int column, int row, int animationSpeed) : base(path, solid)
        {
            this.column = column;
            this.row = row;
            this.animationSpeed = 60/animationSpeed;
        }

        public override void loadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>(path);
            animatedSprite = new AnimatedSprite(sprite, column, row);
    }

        public override void render(int x, int y, Screen screen)
        {
            int width = animatedSprite.getWidth();
            int height = animatedSprite.getHeight();
            screen.renderSprite(x * width, y * height,width,height,animatedSprite.getSourceRect(), sprite);
        }
        public override void update()
        {
            animCounter++;
            if (animCounter == animationSpeed)
            {
                animCounter = 0;
               // animatedSprite.nextFrame();
            }
            
        }
    }
}
