using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.src.graphics
{
    public class Camera
    {
        private readonly Viewport viewport;
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Zoom { get; set; }

        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }

        public Camera(Viewport viewport)
        {
            this.viewport = viewport;

            Rotation = 0;
            Zoom = 1;
            Position = Vector2.Zero;
        }

        public Vector2 ViewportCenter //Top Left Corner
        {
            get
            {
                return new Vector2(ViewportWidth * 0.5f, ViewportHeight * 0.5f);
            }
        }

        public Matrix TranslationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(-(int)Position.X,
                   -(int)Position.Y, 0) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                   Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));
            }
        }

        public void AdjustZoom(float amount)
        {
            Zoom += amount;
            if (Zoom < 0.2f)
            {
                Zoom = 0.2f;
            }
        }

        public void MoveCamera(Vector2 cameraMovement, bool clampToMap = false)
        {
            Vector2 newPosition = Position + cameraMovement;

            if (clampToMap) Position = MapClampedPosition(newPosition);
            else Position = newPosition;          
        }

        
        public void CenterOn(Vector2 position, bool clampToMap = false)
        {
            Vector2 newPosition = position;

            if (clampToMap) Position = MapClampedPosition(newPosition);
            else Position = newPosition;
        }


        private Vector2 MapClampedPosition(Vector2 position)
        {
            var cameraMax = new Vector2(Game1.MapWidth * Game1.SpriteWidth -
                (ViewportWidth / Zoom / 2),
                Game1.MapHeight * Game1.SpriteHeight -
                (ViewportHeight / Zoom / 2));

            return Vector2.Clamp(position,
               new Vector2(ViewportWidth / Zoom / 2, ViewportHeight / Zoom / 2),
               cameraMax);
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, TranslationMatrix);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition,
                Matrix.Invert(TranslationMatrix));
        }

    }
}

