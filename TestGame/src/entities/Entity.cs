using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using TestGame.src.entities.objects.furniture;
using TestGame.src.graphics;
using TestGame.src.input;
using TestGame.src.items;
using TestGame.src.level;
using TestGame.src.netcode;
using TestGame.src.tools;

namespace TestGame.src.entities
{
    [DataContract]   
    [KnownType(typeof(Rectangle))]
    [KnownType(typeof(Vector2))]    
    [KnownType(typeof(Sofa))]
    [KnownType(typeof(Speaker1))]
    [KnownType(typeof(Particelspawner))]      
    [KnownType(typeof(TV))]
    [KnownType(typeof(Water))]     
    [KnownType(typeof(Beachchair))]
    [KnownType(typeof(Cabinet1))]
    [KnownType(typeof(Fence1))]
    [KnownType(typeof(Grass))]
    [KnownType(typeof(Plant1))]
    //LAMPS
    [KnownType(typeof(Torch))]
    [KnownType(typeof(CristmasStar))]
    [KnownType(typeof(DiscoLight))]
    [KnownType(typeof(GrandmasLamp))]
    [KnownType(typeof(Latern))]
    [KnownType(typeof(MagicGlass))]

    //TREES
    [KnownType(typeof(Palm))]
    [KnownType(typeof(Fir))]
    [KnownType(typeof(Tree1))]

    //MOBS
    [KnownType(typeof(Sheep))]

    public abstract class Entity
    {
        [ContentSerializerIgnore]
        public EntityPacket EntityPacket; //FOR MULTIPLAYER
        [DataMember, ContentSerializerIgnore]
        protected int x, y;
        [DataMember, ContentSerializerIgnore]
        public int ID = 0;       
        [DataMember, ContentSerializerIgnore]
        public bool solid = true;
        [DataMember]
        public int mapid = 0;
        [ContentSerializerIgnore]
        public int width=0, height=0;       
        [ContentSerializerIgnore]
        public Rectangle hitbox;
        [ContentSerializerIgnore]
        public Texture2D sprite;
        [ContentSerializerIgnore]
        protected Level level;
        [ContentSerializerIgnore]
        public Input input;
        [ContentSerializerIgnore]
        protected Entity LastCollision;
        [ContentSerializerIgnore]
        public bool Drag { get; set; } = false;
        [ContentSerializerIgnore]
        public bool Dragstart { get; set; } = false;
        [ContentSerializerIgnore]
        public bool Flat { get; protected set; } = false;

        [ContentSerializerIgnore]

        public Vector2 CenterPosition
        {
            get
            {
                return new Vector2(x+width/2, y+height/2);
            }
            private set
            {
                x = (int)value.X - width / 2;
                y = (int)value.Y - width / 2;
            }
        }
        public Vector2 Position {
            get
            {
                return position;
            }
            set
            {
                position = value;
                x = (int)value.X;
                y = (int)value.Y;
            }
        } 

        private Vector2 position = new Vector2();
    
        [ContentSerializerIgnore]
        public byte MoveToDirection;

        [ContentSerializerIgnore]
        public Vector2 MoveTo
        {
            get
            {
                return movetopos;
            }
            set
            {
                MoveTo2 = movetopos;
                movetopos = value;             
            }
        }
        private Vector2 movetopos;
        [ContentSerializerIgnore]
        public Vector2 MoveTo2 { get; set; }
        public Entity(Vector2 Position, int MapID)
        {
            this.Position = Position;
            this.mapid = MapID;
        }

        public Entity(EntityPacket ep)
        {
            ApplyPacket(ep);
        }

        public virtual void ApplyPacket(EntityPacket ep)
        {
            if (ep.Position.Available) Position = ep.Position.Value;
            if (ep.MapID.Available) mapid = ep.MapID.Value;
            if (ep.ID.Available) ID = ep.ID.Value;
            if (ep.MoveTo.Available) MoveTo = ep.MoveTo.Value;
            if (ep.direction.Available) MoveToDirection = ep.direction.Value;
        }

        public Entity()
        {

        }         

        public virtual void LoadContent(ContentManager cm)
        {          
        }
        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {

        }

        public virtual void Draw(SpriteBatch sb)
        {
            //sb.Draw(sprite, new Vector2(x,y));
        }

        public virtual void Draw(SpriteBatch sb, Color color)
        {
            if(Global.showHitboxes) sb.Draw(Global.buttontexture, new Rectangle(x + hitbox.X, y + hitbox.Y, hitbox.Width, hitbox.Height), Color.Red * 0.5f);
        }

        public virtual void Init(Level level, Input input)
        {
            this.level = level;
            this.input = input;
        }      

        public virtual void OnClick(Item sender)
        {

        }

        public virtual bool Collision(Vector2 position, Level level)
        {
            int xa = (int)position.X;
            int ya = (int)position.Y;
            if (xa + hitbox.Left < 0 || ya + hitbox.Top < 0 || xa + hitbox.Right >= level.mapWidth*level.CurrentMap.TileWidth || ya + hitbox.Bottom >= level.mapHeight*level.CurrentMap.TileHeight) return true;
            foreach (var coll in level.maps[mapid].collisions) //MAP Collisions
            {
                if (CollisionDetector.Collison(new Rectangle(xa + hitbox.X, ya + hitbox.Y, hitbox.Width, hitbox.Height), coll)) //Level Collisions
                {
                    LastCollision = null;
                    return true;
                }
            }

            foreach (var coll in level.entities) //Mobs Collisions
            {
                if (coll != this && coll.mapid == level.mapID && coll.solid)
                    if (xa + hitbox.Left < coll.x + coll.hitbox.Right && xa + hitbox.Right > coll.x + coll.hitbox.Left && ya + hitbox.Top < coll.y + coll.hitbox.Bottom && ya + hitbox.Bottom > coll.y + coll.hitbox.Top) //Level Collisions
                    {
                        if (coll is ChessFigure && this is ChessFigure) continue;
                        LastCollision = coll;
                        return true;
                    }
            }
            return false;
        }

        public bool Equals(Entity e)
        {
            if (ID != 0 && ID == e.ID) return true;
            //if (x == e.x && y == e.y && mapid == e.mapid && e.GetType() == this.GetType()) return true;
            return false;
        }
    }
}
