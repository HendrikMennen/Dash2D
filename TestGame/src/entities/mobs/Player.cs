

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using TestGame.src.entities.projectiles;
using TestGame.src.graphics;
using TestGame.src.items;
using TestGame.src.netcode;
using TestGame.src.tools;

namespace TestGame.src.entities
{
    public class Player : Mob
    {
        
        public Item currentItem;
        
        private Texture2D sprite_walk;
        private Texture2D sprite_run;

        private Texture2D sprite_walk_hold;
        private Texture2D sprite_face;
        private Texture2D sprite_jacket;

        [DataMember]
        public ClothItem jacket { get; set; }

        public Entity selectedEntity { get; set; }
        
        public Player(Vector2 position, int MapID) : base(position, MapID)
        {

        }

        public Player(EntityPacket ep) : base(ep)
        {

        }

        public Player()
        {

        }

        public override void LoadContent(ContentManager cm)
        {
            animationSpeed = 12;
            rows = 3;
            columns = 4;

            hitbox = new Rectangle(14, 30, 12, 9);

            sprite_walk = cm.Load<Texture2D>("textures/models/player/player2_walk");
            sprite_run = cm.Load<Texture2D>("textures/models/player/player2_run");
            sprite_walk_hold = cm.Load<Texture2D>("textures/models/player/player_walk_hold");
            sprite_face = cm.Load<Texture2D>("textures/models/player/face/face1");

            sprite = sprite_walk;
            base.LoadContent(cm);    
        }

        public override void Update()
        {
            if(speed >= 2)
            {
                sprite = sprite_run;
                if (jacket != null) sprite_jacket = jacket.sprite_run;
                columns = 6;               
            }
            else
            {
                sprite = sprite_walk;
                if (jacket != null) sprite_jacket = jacket.sprite_walk;
                columns = 4;
            }
            if(columns != animatedSprite.Columns)animatedSprite.Columns = columns;
            if (currentItem is GunItem<Projectile>)
            {
                //sprite = sprite_walk_hold;
            }
            else
            {
                //sprite = sprite_walk;
            }
            
            chatBubbleDuration--;
            if (this != level.getClientPlayer()) Moveto();
            base.Update();
        }

        public override void FixedUpdate()
        {
          
            base.FixedUpdate();
        }

        public override void ApplyPacket(EntityPacket ep)
        {
            if (ep.Player_Jacket.Available) jacket = (ClothItem)StaticItems.Items[ep.Player_Jacket.Value];
            base.ApplyPacket(ep);
        }

        public override void Draw(SpriteBatch sb)
        {
            //DRAW PLAYER
            base.Draw(sb); 

            //DRAW JACKET
            if (jacket != null && sprite_jacket != null) sb.Draw(sprite_jacket, new Rectangle(x, y, width, height), animatedSprite.getSourceRect(), jacket.clothcolor, 0f, new Vector2(), animatedSprite.flip, 0f);
            //DRAW FACE
            Rectangle source = new Rectangle(animatedSprite.row * 16, 0, 16, 16); 
            Rectangle destination = new Rectangle(x+12, y+3, 16, 16); 
            if (sprite == sprite_walk && (animatedSprite.column == 1 || animatedSprite.column == 3)) destination.Y += 1;
            if (sprite == sprite_run && (animatedSprite.column == 1 || animatedSprite.column == 4)) destination.Y -= 1;
            sb.Draw(sprite_face, destination, source, Color.White, 0f, new Vector2(), animatedSprite.flip, 0f);
        }

    }
}
