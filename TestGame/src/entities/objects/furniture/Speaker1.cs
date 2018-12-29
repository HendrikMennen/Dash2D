using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using TestGame.src.graphics.ui;
using TestGame.src.items;
using System.Runtime.Serialization;

namespace TestGame.src.entities.objects.furniture
{
    [DataContract]
    class Speaker1 : Furniture
    {
        private Song music;
        public Speaker1(int x, int y)
        {
            this.x = x;
            this.y = y;
              
        }

        public Speaker1()
        {
        }

        public override void LoadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>("textures/objects/furniture");

            source = new Rectangle(2 * 16, 2 * 16, 16, 32);

            width = source.Width;
            height = source.Height;

            hitbox = new Rectangle(0, 27, 16, 5);
            clickbox = new Rectangle(0, 0, 16, 32);
            base.LoadContent(cm);
        }        

        public override void OnClick(Item sender)
        {
            base.OnClick(sender);
            Vector2 mapPos = input.GetMapPos(input.MousePos);
           
                if (sender is DiskItem<Song>)
                {
                    if (MediaPlayer.State == MediaState.Playing) MediaPlayer.Stop();
                    music = (Song)((DiskItem<Song>)sender).disk;
                }
                if (MediaPlayer.State == MediaState.Playing) MediaPlayer.Pause();
                else if (music != null)
                {
                    if (MediaPlayer.State == MediaState.Paused) MediaPlayer.Resume();
                    else MediaPlayer.Play(music);
                }
            
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
