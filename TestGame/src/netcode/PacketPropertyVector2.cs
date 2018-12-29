using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.src.netcode
{
    public class PacketPropertyVector2 : PacketProperty
    {
        public int[] SerializeValue = new int[2];
        public Vector2 Value
        {
            get
            {
                return new Vector2(SerializeValue[0], SerializeValue[1]);              
            }
            set
            {
                SerializeValue[0] = (int)value.X;
                SerializeValue[1] = (int)value.Y;
                Available = true;
            }
        }
        
    }
}
