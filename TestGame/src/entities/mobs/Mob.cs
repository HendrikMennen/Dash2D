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
using TestGame.src.level;
using TestGame.src.netcode;
using TestGame.src.tools;

namespace TestGame.src.entities
{
    public abstract class Mob : Entity
    {
        
        protected int animationSpeed = 60/5;     
        protected bool walking = false;       
        private int animCounter;            
        protected int rows;      
        protected int columns;
        public int direction = 1;
        public AnimatedMobSprite animatedSprite;
        private int particles = 2;
        private int particlecounter = 10;

        //CHAT
        private Texture2D bubble;
        private int bubbleduration;
        private string message;
        public string name = "Manfreed";
        private SpriteFont font;
        
        
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
            bubble = cm.Load<Texture2D>("textures/ui/chat/chatBubble");
            font = cm.Load<SpriteFont>("fonts/PixelFont");
            animatedSprite = new AnimatedMobSprite(sprite, rows, columns);
            width = animatedSprite.getWidth();
            height = animatedSprite.getHeight();           
            Position = new Vector2(x, y);
        }

        public void Say(string message)
        {
            this.message = message;
            bubbleduration = 60 * 5; //5 Sekunden
        }

        public void DrawChatBubble(SpriteBatch sb)
        {
            if (bubbleduration <= 0) return;
            string text = message;                    
            int segmentSize = (int)(15 * Global.UIScale);
            int chatbubbleHeight = (int)(bubble.Height * Global.UIScale);
            int chatbubbleWidth = (int)(font.MeasureString(text).X * Global.UIScale)-8;
            Vector2 pos = input.GetScreenPos(new Vector2(CenterPosition.X + 7, CenterPosition.Y - 5));
            
            int posX = (int)pos.X;
            int posY = (int)pos.Y - chatbubbleHeight;
            int textposY = (int)(posY+2);
            if (Global.UIScale == 2) textposY += 2;

            sb.Draw(bubble, new Rectangle(posX, posY, segmentSize, chatbubbleHeight), new Rectangle(0, 0, 15, bubble.Height), Color.White);
            for (int i = 0; chatbubbleWidth > (bubble.Width - 20) * i; i++)
            {
                sb.Draw(bubble, new Rectangle(posX + (bubble.Width - 20) * i + segmentSize, posY, (int)((bubble.Width - 15) * Global.UIScale), chatbubbleHeight), new Rectangle(15, 0, bubble.Width - 15, bubble.Height), Color.White);
            }
            sb.DrawString(font, text, new Vector2(posX + segmentSize-5, textposY), Color.Black, 0f, Vector2.Zero, Global.UIScale, SpriteEffects.None, 0f);
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

            float antilagX = 0, antilagY = 0;
            if (Position.X < MoveTo.X + 1) antilagX += 0.2f;
            if (Position.X > MoveTo.X - 1) antilagX -= 0.2f;
            if (Position.Y < MoveTo.Y + 1) antilagY += 0.2f;
            if (Position.Y > MoveTo.Y - 1) antilagY -= 0.2f;
            Position += new Vector2(antilagX, antilagY);

            if (this is Player && (Math.Abs(Position.Y-MoveTo.Y) > 2 || Math.Abs(Position.X - MoveTo.X) > 2))
            {
                //Position = MoveTo;
                //return;
            }           
            movev = new Vector2(xx, yy);
            Move(movev * (speed));
            //Debug.WriteLine(movev.X + " " + movev.Y);
        }
        public void Move(Vector2 offset)
        {
            if(Collision(0, (int)Math.Floor(offset.Y)))
            {
                offset.Y = 0;
            }
            if(Collision((int)Math.Floor(offset.X), 0))
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
                if(speed >= 2)
                {
                    particlecounter++;
                    if(particlecounter >= particles)
                    {
                        int id = level.CurrentMap.getTile(0, CenterPosition + new Vector2(0, height / 2 - 5));
                        Color[] cc = level.CurrentMap.getTileTex(id);
                        Color color = new Color(cc[0].R / 2, cc[0].G / 2, cc[0].B / 2);
                        particlecounter = 0;
                        RunParticle p = new RunParticle((int)CenterPosition.X, y + height-5, color)
                        {
                            mapid = mapid
                        };
                        level.AddEntity(p);                       
                    }
                    
                }
                if (level.mapID == mapid) UpdateAnimation();
            }

            if (offset.Y > 0) direction = 3;
            if (offset.Y < 0) direction = 1;
            if (offset.X > 0) direction = 4;
            if (offset.X < 0) direction = 2;           
        }

        public override void Update()
        {
            if(bubbleduration > 0) bubbleduration--;           
            base.Update();
        }
        public override void Draw(SpriteBatch sb)
        {
            Draw(sb, Color.White); 
        }
        public override void Draw(SpriteBatch sb, Color color)
        {
            //if (bubbleduration > 0) DrawChatBubble(sb, CenterPosition);
            sb.Draw(sprite, new Rectangle(x, y, width, height), animatedSprite.getSourceRect(), color, 0f, new Vector2(), animatedSprite.flip, 0f);           
            base.Draw(sb, color);
        }       
    
        public void UpdateAnimation()
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

        public bool Collision(int xa, int ya) //TODO ENTITY COLLISION
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
