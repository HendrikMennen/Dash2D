using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using TestGame.src.animations;
using TestGame.src.entities.particles;
using TestGame.src.graphics;
using TestGame.src.netcode;
using TestGame.src.tools;

namespace TestGame.src.entities
{
    [DataContract]
    public abstract class Mob : Entity
    {
        
        protected int animationSpeed = 60/5;
        
        protected bool walking = false;
        
        private int animCounter;
               
        protected int rows;
        
        protected int columns;

        [ContentSerializerIgnore]
        public int direction = 1;

        public AnimatedMobSprite animatedSprite;

        public string name = "Nameless";
        //CHAT BUBBLE
        
        private Texture2D chatBubble;
        
        protected SpriteFont font, namefont;
        public string text { get; set; }
        
        private int textlength,textheight;
        
        public int chatBubbleDuration = 0;

        public float speed = 1f;
        private int maxhealth = 100;
        public int MaxHealth
        {
            get
            {
                return maxhealth;
            }
            set
            {
                if (value > 0)
                {
                    maxhealth = value;
                    if (health < maxhealth)
                    {
                        health = maxhealth;
                    }
                }
            }
        }
        private int health = 100;
        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                if (value >= 0)
                {
                    if (value > maxhealth) health = maxhealth;
                    else health = value;
                }
                if (value <= 0)
                {
                    level.remEntity(this);
                }
            }
        }

        public Mob(Vector2 Position, int MapID) : base(Position, MapID)
        {

        }

        public Mob(EntityPacket ep) : base(ep)
        {

        }

        public Mob() : base()
        {

        }

        public override void ApplyPacket(EntityPacket ep)
        {
            if (ep.Speed.Available) speed = ep.Speed.Value;
            if (ep.Name.Available) name = ep.Name.Value;
            //Debug.WriteLine(name);
            if (ep.direction.Available) direction = ep.direction.Value;
            base.ApplyPacket(ep);
        }

        public override void LoadContent(ContentManager cm)
        {
            animatedSprite = new AnimatedMobSprite(sprite, rows, columns);
            width = animatedSprite.getWidth();
            height = animatedSprite.getHeight();

            chatBubble = cm.Load<Texture2D>("textures/ui/chat/chatBubble");          
            font = cm.Load<SpriteFont>("fonts/SegoeUIBlack");
            namefont = cm.Load<SpriteFont>("fonts/SegoeUIBlack");

            Position = new Microsoft.Xna.Framework.Vector2(x, y);
        }

        public void say(string text)
        {
            this.text = text;
            chatBubbleDuration = 3 * 60; //3 Sekunden
            textlength = (int)font.MeasureString(text).X;
            textheight = (int)font.MeasureString(text).Y;
        }

        protected void Moveto()
        {
            Vector2 movev;
            if (MoveTo2 != Vector2.Zero && MoveTo != Position)
            {
                movev = MoveTo - MoveTo2;   //DIRECTION ONLY
            }
            else
            {
                movev = Vector2.Zero;
            }
            float xx = 0, yy = 0;
            if (movev.X != 0) xx = movev.X / (Math.Abs(movev.X));
            if (movev.Y != 0) yy = movev.Y / (Math.Abs(movev.Y));
            if (Position.X < MoveTo.X + 1) xx += 0.2f;
            if (Position.X > MoveTo.X - 1) xx -= 0.2f;
            if (Position.Y < MoveTo.Y + 1) yy += 0.2f;
            if (Position.Y > MoveTo.Y - 1) yy -= 0.2f;
            if (this is Player && (Math.Abs(Position.Y-MoveTo.Y) > 2 || Math.Abs(Position.X - MoveTo.X) > 2))
            {
                //Position = MoveTo;
                //return;
            }           
            movev = new Vector2(xx, yy);
            move(movev * (speed));
            Debug.WriteLine(movev.X + " " + movev.Y);
        }
        public void move(Vector2 offset)
        {
            if(collision(0, (int)Math.Floor(offset.Y)))
            {
                offset.Y = 0;
            }
            if(collision((int)Math.Floor(offset.X), 0))
            {
                offset.X = 0;
            }

            if (offset.X == 0f && offset.Y == 0f)
            {
                animatedSprite.setColumn(0);
                walking = false;
                animatedSprite.setDirection(direction);
                return;
            }
            else
            {
                Position += offset;
                if (level.mapID == mapid) updateAnimation();
            }

            if (offset.Y > 0) direction = 3;
            if (offset.Y < 0) direction = 1;
            if (offset.X > 0) direction = 4;
            if (offset.X < 0) direction = 2;           
        }
        public override void Draw(SpriteBatch sb)
        {
            Draw(sb, Color.White); 
        }
        public override void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(sprite, new Rectangle(x, y, width, height), animatedSprite.getSourceRect(), color, 0f, new Vector2(), animatedSprite.flip, 0f);
            base.Draw(sb, color);
        }

        public void renderChat(SpriteBatch sb)
        {
            Vector2 pos = input.getScreenPos(new Vector2(x+30, y));
            int xa = (int)pos.X;
            int ya = (int)pos.Y;
            //if (Game1.online) sb.DrawString(namefont, name, new Vector2(xa - namefont.MeasureString(name).X/2 , ya), Color.Black); //TODO
            if (chatBubbleDuration > 0)
            {            
                int xPos = xa;
                int yPos = ya;

                if (Game1.online) yPos -= (int)(namefont.MeasureString(name).Y);

                sb.Draw(chatBubble, new Rectangle(xPos, yPos, 15, chatBubble.Height),new Rectangle(0,0,15,chatBubble.Height), Color.White);
                for(int i = 0; font.MeasureString(text).X > (chatBubble.Width-20)*i; i++)
                {
                    sb.Draw(chatBubble, new Rectangle(xPos+(chatBubble.Width-20)*i+15, yPos, chatBubble.Width-15, chatBubble.Height),new Rectangle(15,0,chatBubble.Width-15,chatBubble.Height), Color.White);
                }                               
                sb.DrawString(font, text, new Vector2(xPos + 15, yPos + (int)(0.5)),Color.Black);
            }
            else
            {
                text = "";
            }
        }
    
        public void updateAnimation()
        {
           
            animatedSprite.setDirection(direction);
           
                animCounter++;
                if (animCounter >= animationSpeed / speed)
                {                
                //if (animatedSprite.column == 0 || animatedSprite.column == 2) Global.sound.Play(stepstone, 0.1f);
 
                animatedSprite.nextFrame();
                    animCounter = 0;
                }                    
            }

        public bool collision(int xa, int ya) //TODO ENTITY COLLISION
        {
            xa += (x);
            ya += (y);
            if(Collision(new Vector2(xa, ya), level))
            {
                return true;
            }

            foreach(var coll in level.CurrentMap.teleporters)
            {
                
                if (CollisionDetector.Collison(new Rectangle(xa + hitbox.X, ya + hitbox.Y, hitbox.Width, hitbox.Height), coll.position))
                {
                    if(this == level.getClientPlayer())
                        if (level.ChangeMap(coll.mapID))
                        {
                            mapid = coll.mapID;
                            x = (int)coll.tpCoordinates.X;
                            y = (int)coll.tpCoordinates.Y;
                            Position = new Vector2(x, y);
                        }                 
                }
                    
            }

            return false;                
        }
    }
}
