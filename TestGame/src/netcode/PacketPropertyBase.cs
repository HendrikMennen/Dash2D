using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.src.netcode
{
    public class PacketPropertyBase<T> : PacketProperty
    {    
        T propvalue;
        public virtual T Value
        {
            get
            {
                return propvalue;
            }
            set
            {
                propvalue = value;
                Available = true;
                SerializeValue = propvalue;
            }
        }

        public T SerializeValue;
    }
}
