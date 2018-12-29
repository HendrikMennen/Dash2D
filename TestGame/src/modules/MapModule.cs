using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.level;

namespace TestGame.src.modules
{
    public abstract class MapModule
    {
        protected Level level;
        protected int mapID;
        public virtual void Init(Level level)
        {
            this.level = level;
        }

        public virtual void LoadContent(ContentManager cm)
        {

        }

        public virtual void Activate(bool activate, int mapID)
        {
            this.mapID = mapID;
        }
    }
}
