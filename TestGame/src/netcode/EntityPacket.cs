using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.entities;
using TestGame.src.entities.objects.furniture;

namespace TestGame.src.netcode
{
    public class EntityPacket
    {
        List<PacketProperty> PacketProperties = new List<PacketProperty>();

        //ENTITYS GENERAL
        public PacketPropertyVector2 Position= new PacketPropertyVector2();

        public PacketPropertyBase<byte> direction = new PacketPropertyBase<byte>();
        public PacketPropertyVector2 MoveTo = new PacketPropertyVector2();
        public PacketPropertyBase<short> ID = new PacketPropertyBase<short>();
        public PacketPropertyBase<byte> MapID = new PacketPropertyBase<byte>();
        
        public PacketPropertyType EntityType = new PacketPropertyType();
        //MOBS
        public PacketPropertyBase<float> Speed = new PacketPropertyBase<float>();
        public PacketPropertyBase<string> Name = new PacketPropertyBase<string>();
        //PLAYER ONLY
        public PacketPropertyBase<int> Player_Jacket = new PacketPropertyBase<int>();
        //FURNITURE ONLY
        public PacketPropertyBase<bool> On = new PacketPropertyBase<bool>();

        
        private EntityPacket()
        {
            PacketProperties.Add(ID);
            PacketProperties.Add(EntityType);
            PacketProperties.Add(Position);
            PacketProperties.Add(MoveTo);
            PacketProperties.Add(direction);
            PacketProperties.Add(MapID);
            PacketProperties.Add(Speed);         
            PacketProperties.Add(Player_Jacket);
            PacketProperties.Add(On);
            PacketProperties.Add(Name);
        }

        public bool Empty()
        {
            bool empty = true;
            foreach(var prop in PacketProperties)
            {
                if (prop != ID && prop.Available) empty = false;
            }
            return empty; 
        }

        public EntityPacket(Entity e, EntityPacket ep) : this() //DIFFERENCE
        {
            ID.Value = (short)e.ID; //SEND ID EVERYTIME
            if (ep.Position.Value != e.Position) Position.Value = e.Position;
            if (ep.MapID.Value != e.mapid) MapID.Value = (byte)e.mapid;
            
            if (ep.EntityType.Value != e.GetType()) EntityType.Value = e.GetType();
            if (ep.MoveTo.Value != e.MoveTo) MoveTo.Value = e.MoveTo;
            if (e is Player && ((Player)e).Jacket != null && ep.Player_Jacket.Value != ((Player)e).Jacket.ID) Player_Jacket.Value = ((Player)e).Jacket.ID;
            if (e is Mob && ep.Speed.Value != ((Mob)e).speed) Speed.Value = ((Mob)e).speed;
            if (e is Mob && ep.direction.Value != ((Mob)e).direction) direction.Value = (byte)((Mob)e).direction;
            if (e is Mob && !((Mob)e).name.Equals(ep.Name.Value)) Name.Value = ((Mob)e).name;
            if (e is Furniture && ep.On.Value != ((Furniture)e).On) On.Value = ((Furniture)e).On;
        }
      
        public EntityPacket(Entity e) : this(e, new EntityPacket())
        {

        }     

        public byte[] Serialize(bool full)
        {
            if (!full) Position.Available = false;
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    bool[] array = new bool[32];
                    for(int i = 0, c = 31; i < PacketProperties.Count || c == 0; i++ , c--) 
                    {
                        array[c] = PacketProperties[i].Available;
                    }
                    BitArray bit = new BitArray(array);

                    int b = ConvertToInt(bit); //BYTESTRING REVERSE
                    writer.Write(b);

                    foreach(var prop in PacketProperties)
                    {
                        if (!prop.Available) continue;
                        else if (prop is PacketPropertyBase<byte>) writer.Write(((PacketPropertyBase<byte>)prop).SerializeValue);
                        else if (prop is PacketPropertyBase<short>) writer.Write(((PacketPropertyBase<short>)prop).SerializeValue);
                        else if (prop is PacketPropertyBase<int>) writer.Write(((PacketPropertyBase<int>)prop).SerializeValue);
                        else if (prop is PacketPropertyBase<bool>) writer.Write(((PacketPropertyBase<bool>)prop).SerializeValue);
                        else if (prop is PacketPropertyBase<float>) writer.Write(((PacketPropertyBase<float>)prop).SerializeValue);
                        else if (prop is PacketPropertyBase<string>) writer.Write(((PacketPropertyBase<string>)prop).SerializeValue);
                        else if (prop is PacketPropertyVector2)
                        {
                            writer.Write(((PacketPropertyVector2)prop).SerializeValue[0]);
                            writer.Write(((PacketPropertyVector2)prop).SerializeValue[1]);
                        }
                        else if (prop is PacketPropertyType) writer.Write(((PacketPropertyType)prop).SerializeValue);
                        else throw new NotImplementedException("No serialisation method implemented! EntityPacket.Serialize");
                    }  
                                       
                }
                return m.ToArray();
            }
        }

        public static EntityPacket Deserialize(byte[] data)
        {
            EntityPacket result = new EntityPacket();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    int b = reader.ReadInt32();
                    string byteString = Convert.ToString(b, 2).PadLeft(32, '0');
                    for(int i = 0; i < result.PacketProperties.Count; i++)
                    {
                        if(byteString[i] == '1')
                        {
                            if (result.PacketProperties[i] is PacketPropertyBase<byte>)
                                ((PacketPropertyBase<byte>)result.PacketProperties[i]).Value = reader.ReadByte();
                            if (result.PacketProperties[i] is PacketPropertyBase<short>)
                                ((PacketPropertyBase<short>)result.PacketProperties[i]).Value = reader.ReadInt16();
                            if (result.PacketProperties[i] is PacketPropertyBase<int>)
                                ((PacketPropertyBase<int>)result.PacketProperties[i]).Value = reader.ReadInt32();
                            else if (result.PacketProperties[i] is PacketPropertyBase<bool>)
                                ((PacketPropertyBase<bool>)result.PacketProperties[i]).Value = reader.ReadBoolean();
                            else if (result.PacketProperties[i] is PacketPropertyBase<float>)
                                ((PacketPropertyBase<float>)result.PacketProperties[i]).Value = reader.ReadSingle();
                            else if (result.PacketProperties[i] is PacketPropertyBase<string>)
                                ((PacketPropertyBase<string>)result.PacketProperties[i]).Value = reader.ReadString();
                            else if (result.PacketProperties[i] is PacketPropertyVector2)
                                ((PacketPropertyVector2)result.PacketProperties[i]).Value = new Vector2(reader.ReadInt32(), reader.ReadInt32());
                            else if (result.PacketProperties[i] is PacketPropertyType)
                                ((PacketPropertyType)result.PacketProperties[i]).Value = Type.GetType((result.EntityType.Shorttype + reader.ReadString()));
                        }
                    }

                    //Debug.WriteLine(byteString);
                }
            }
            return result;
        }     

        byte ConvertToByte(BitArray bits)
        {
            if (bits.Count != 8)
            {
                throw new ArgumentException("bits");
            }
            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return bytes[0];
        }

        int ConvertToInt(BitArray bits)
        {
            if (bits.Count != 32)
            {
                throw new ArgumentException("bits");
            }
            int[] bytes = new int[1];
            bits.CopyTo(bytes, 0);
            return bytes[0];
        }
    }
}
