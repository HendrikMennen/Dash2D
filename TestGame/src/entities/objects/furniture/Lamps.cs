using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using TestGame.src.entities.objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TestGame.src.input;
using System.Diagnostics;
using TestGame.src.animations;
using Microsoft.Xna.Framework.Audio;
using TestGame.src.items;
using TestGame.src.tools;
using System.Runtime.Serialization;
using TestGame.src.netcode;

namespace TestGame.src.entities.objects.furniture
{
    [DataContract]
    public abstract class Lamp : AnimatedFurniture
    {
        
        private SoundEffect switchsound;

        [ContentSerializerIgnore]
        public override bool On
        {
            get
            {
                return on;
            }
            set
            {
                if (!switchable) return;
                on = value;
                if (switchsound != null) Global.sound.Play(switchsound);
                if (value && Animation != null) Animation.column = 1;
                else if(Animation != null) Animation.column = 0;
            }
        }
        [ContentSerializer]
        [DataMember]
        protected bool on = false;      
        public Color lightColor = Color.White;
        [ContentSerializerIgnore]
        public float scale { get; protected set; } = 1f;
        [ContentSerializerIgnore]
        [DataMember]     
        public bool switchable = true;

        public Lamp(Vector2 Position, int MapID) : base(Position, MapID)
        {

        }

        public Lamp(EntityPacket ep) : base(ep)
        {

        }

        public Lamp() : base()
        {

        }

        public override void Update()
        {
            
        }

        public override void onClick(Item sender)
        {          
            if (On)
            {
                On = false;
            }
            else
            {
                On = true;
            }       
        }

        public override void LoadContent(ContentManager cm)
        {           
            base.LoadContent(cm);
            switchsound = cm.Load<SoundEffect>("sound/effects/stereo/oldlamp_switch");

            if (On && Animation != null) Animation.column = 1;
            else if (Animation != null) Animation.column = 0;
        }
    }

    [DataContract]
    class Torch : Lamp
    {
        int animationCounter = 0;
        bool glow = false;       

        public Torch(Vector2 Position, int MapID) : base(Position, MapID)
        {

        }

        public Torch(EntityPacket ep) : base(ep)
        {

        }

        public Torch() : base()
        {

        }

        public override void Update()
        {
            if (glow) scale += 0.002f;
            else scale -= 0.002f;
            if (animationCounter > 12)
            {
                if (glow) glow = false;
                else glow = true;
                animationCounter = Global.rdm.Next(3);
                Animation.nextFrame();
                if (!glow) scale = 1;
            }
            animationCounter++;
        }

        public override void LoadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>("textures/objects/furniture");

            source = new Rectangle(0, 9 * 16, 3 * 16, 16);
            Position = new Microsoft.Xna.Framework.Vector2(x, y);

            on = true;
            width = 16;
            height = 16;

            Animation = new AnimatedSprite(new Rectangle(0, 0, 64, 16), 1, 4);

            solid = false;
            lightColor = new Color(255,255,200,255);

            hitbox = new Rectangle(6, 4, 4, 8);
            clickbox = new Rectangle(6, 4, 4, 8);
            switchable = false;
            scale = 1f;
            base.LoadContent(cm);
        }
    }

    [DataContract]
    class CristmasStar : Lamp
    {
        public CristmasStar(Vector2 Position, int MapID) : base(Position, MapID)
        {

        }

        public CristmasStar(EntityPacket ep) : base(ep)
        {

        }

        public CristmasStar() : base()
        {

        }

        public override void LoadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>("textures/objects/furniture");

            source = new Microsoft.Xna.Framework.Rectangle(0, 13 * 16, 16, 16);

            width = source.Width;
            height = source.Height;

            solid = false;
            hitbox = new Rectangle(6, 27, 5, 5);
            lightColor = Color.Red;
            clickbox = new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16);
            base.LoadContent(cm);
        }
    }

    [DataContract]
    class DiscoLight : Lamp
    {
        int animationCounter = 0;
        
        private Color startColor = Color.Blue;
        
        private Color endColor = Color.Red;
        
        private List<Color> gradientColors;       

        public override void Update()
        {
            
            if(gradientColors == null || animationCounter > Global.rdm.Next(39,39))
            {
                startColor = lightColor;
                animationCounter = 0;
                Animation.column = 0; //TODO REMOVE LATER
                int randomcolor = Global.rdm.Next(5);
                if (randomcolor == 0) endColor = Color.Blue;
                else if (randomcolor == 1) endColor = Color.Yellow;
                else if (randomcolor == 2) endColor = Color.Green;
                else if (randomcolor == 3) endColor = Color.DarkViolet;
                else if (randomcolor == 4) endColor = Color.Red;

                gradientColors = Global.GetGradients(startColor, endColor, 40).ToList();
            }

            lightColor = gradientColors[animationCounter];
            animationCounter++;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb, lightColor);
        }

        public override void LoadContent(ContentManager cm)
        {
            solid = false;
            sprite = cm.Load<Texture2D>("textures/objects/furniture");
            on = true;
            scale = 0.5f;

            source = new Rectangle(0, 10 * 16, 5*16, 16);

            Animation = new AnimatedSprite(new Rectangle(0, 0, 16*5, 16), 1, 5);

            width = 16;
            height = 16;

            hitbox = new Rectangle(0, 0, 16, 16);
            clickbox = new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16);
            switchable = false;
            Flat = true;
            base.LoadContent(cm);
        }
    }

    [DataContract]
    class GrandmasLamp : Furniture
    {
        public GrandmasLamp(int x, int y)
        {
            this.x = x;
            this.y = y;
            Position = new Microsoft.Xna.Framework.Vector2(x, y);
        }
        public GrandmasLamp()
        {
        }

        public override void LoadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>("textures/objects/furniture");

            source = new Microsoft.Xna.Framework.Rectangle(16, 2 * 16, 16, 32);

            width = source.Width;
            height = source.Height;
           
            hitbox = new Rectangle(6, 27, 5, 5);
            clickbox = new Microsoft.Xna.Framework.Rectangle(0,0, 16, 32);
            base.LoadContent(cm);          
        }
    }

    [DataContract]
    class Latern : Lamp
    {
        public Latern(int x, int y)
        {
            this.x = x;
            this.y = y;
            Position = new Microsoft.Xna.Framework.Vector2(x, y);
        }

        public Latern()
        {
        }
     
        public override void LoadContent(ContentManager cm)
        {
            sprite = cm.Load<Texture2D>("textures/objects/furniture");

            source = new Rectangle(0, 2 * 16, 16, 32);

            Animation = new AnimatedSprite(new Rectangle(0, 0, 32, 32), 1, 2);

            width = 16;
            height = 32;
            scale = 1f;

            hitbox = new Rectangle(6, 27, 5, 5);
            lightColor = new Color(255, 255, 200);
            clickbox = new Microsoft.Xna.Framework.Rectangle(0,0, 16, 32);
            switchable = true;
            Position = new Vector2(x, y);
            base.LoadContent(cm);           
        }    
    }

    [DataContract]
    class MagicGlass : Lamp
    {
        public MagicGlass(int x, int y)
        {
             this.x = x;
             this.y = y;
             Position = new Microsoft.Xna.Framework.Vector2(x, y);
        }
      
        public MagicGlass()
        {
        }

    public override void LoadContent(ContentManager cm)
    {
            sprite = cm.Load<Texture2D>("textures/objects/furniture");

            source = new Rectangle(0, 7 * 16, 16, 32);

        Animation = new AnimatedSprite(new Rectangle(0, 0, 32, 32), 1, 2);
    
        width = 16;
        height = 32;
        scale = 1f;

        lightColor = new Color(87f,0f, 127f);
        hitbox = new Rectangle(6, 27, 5, 5);
        switchable = true;
        clickbox = new Rectangle(0, 0, 16, 32);
        Position = new Vector2(x, y);
        base.LoadContent(cm);
    }
}    
}
