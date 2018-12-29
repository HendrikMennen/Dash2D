using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.src.netcode
{
    public class PacketPropertyType : PacketProperty
    {
        public string typeString;
        public readonly string Shorttype = "TestGame.src.entities";
        public string SerializeValue;

        public Type Value
        {
            get
            { 
                return Type.GetType((Shorttype + typeString));
            }
            set
            {
                Available = true;
                typeString = value.ToString();
                string[] param = typeString.Split('.');

                typeString = "";
                for (int i = 3; i < param.Length; i++)
                {
                    typeString += "." + param[i];
                }

                SerializeValue = (string)typeString;
            }
        }
    }
}
