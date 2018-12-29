using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TestGame.src.animations
{
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public Rectangle Source { get; set; }
        public int Rows { get; set; }
        public int Columns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
                column = 0;
            }
        }
        private int columns;
           
        public int currentFrame;
        protected int totalFrames;
        protected int width;
        protected int height;
        public int row;
        public int column;

        public AnimatedSprite(Texture2D texture, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;

            width = Texture.Width / Columns;
            height = Texture.Height / Rows;
            row = (int)((float)currentFrame / (float)Columns);
            column = currentFrame % Columns;
        }
        public AnimatedSprite(Rectangle source, int rows, int columns)
        {
            Source = source;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;

            width = source.Width / Columns;
            height = source.Height / Rows;
            row = (int)((float)currentFrame / (float)Columns);
            column = currentFrame % Columns;
        }

        public virtual void nextFrame()
        {
            currentFrame++;
            if (currentFrame == totalFrames)
                currentFrame = 0;           
            row = (int)((float)currentFrame / (float)Columns);
            column = currentFrame % Columns;
        }

        public Rectangle getSourceRect()
        {
            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            return sourceRectangle;
        }
        public int getWidth()
        {
            return width;
        }
        public int getHeight()
        {
            return height;
        }
    }
}