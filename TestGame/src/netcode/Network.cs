///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  For a more detailed comments found in the server program (ServerAplication), so if you have not done it, check out.  //
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using Lidgren.Network;
using TestGame.src.level;
using TestGame.src.entities;
using TestGame.src.entities.objects.furniture;
using System.Diagnostics;
using TestGame.src.tools;
using TestGame.src.graphics.ui.menu.components;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TestGame.src.items;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using TestGame.src.graphics.ui;

namespace TestGame.src.netcode
{
    public class NetCode 
    {
        public enum NetworkAction : byte { Connect,ConnectResponse, Disconnect, Deny, Move, Chat, AddEntity, RefreshEntity, RefreshEntities, OnClick };

        public static int id = 0;
        public static NetClient Client;
        public static NetPeerConfiguration Config;
        /*public*/ static NetIncomingMessage incmsg;
        public static NetOutgoingMessage outmsg;
        public static Level level;
        public static Commands cmd;
        public static List<ListMenuElement> servers = new List<ListMenuElement>();
        public static List<ListMenuElement> localServers = new List<ListMenuElement>();
        private static int recbytespersecond = 0;
        public static int RecievingBytesPerSecond = 0;
        private static int semdcbytespersecond = 0;
        public static int SendingBytesPerSecond = 0;
        static TimeSpan start = DateTime.Now.TimeOfDay;
        static TimeSpan end = start + TimeSpan.FromSeconds(1);
        
        
        public static void init(Level _level, Commands _cmd)
        {
            level = _level;
            cmd = _cmd;
        }

        public static void Connect(string[] param)
        {           
            string name = level.getClientPlayer().name;
            string ip = "localhost";
            int port = 25232;
            string[] paramip = new string[1];
            if (param.Length > 1)
            {
                paramip = param[1].Split(':');
                ip = paramip[0];
                if (paramip.Length > 1) port = Int32.Parse(paramip[1]);
            }
            if (param.Length > 2) port = Int32.Parse(param[2]);

            Debug.WriteLine(ip + " " + port);

            Client.Connect(ip, port);

            System.Threading.Thread.Sleep(300);

            outmsg = Client.CreateMessage();
            outmsg.Write((byte)NetworkAction.Connect);
            EntityPacket packet = new EntityPacket(level.getClientPlayer());
            byte[] objectArray = packet.Serialize(true); //Convert Object to bytearray

            outmsg.Write(objectArray.Length); //SEND LENGTH OF ARRAY
            outmsg.Write(objectArray); //SEND ARRAY ITSELF
            semdcbytespersecond += outmsg.LengthBytes;
            Client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);       
        }

        public static void Disconnect(string[] param)
        {          
            outmsg = Client.CreateMessage();
            outmsg.Write((byte)NetworkAction.Disconnect);
            outmsg.Write(level.getClientPlayer().ID);
            semdcbytespersecond += outmsg.LengthBytes;
            Client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
            Game1.online = false;
            level.clearEntities();
        }

        public static void Chat(string text)
        {
            outmsg = Client.CreateMessage();
            outmsg.Write((byte)NetworkAction.Chat);
            outmsg.Write(level.getClientPlayer().ID);
            outmsg.Write(text);
            semdcbytespersecond += outmsg.LengthBytes;
            Client.SendMessage(outmsg, NetDeliveryMethod.Unreliable);
        }

        public static void OnClick(Entity e, Item i)
        {
            outmsg = Client.CreateMessage();
            outmsg.Write((byte)NetworkAction.OnClick);
            outmsg.Write(e.ID);
            outmsg.Write(i.ID);
            semdcbytespersecond += outmsg.LengthBytes;
            Client.SendMessage(outmsg, NetDeliveryMethod.Unreliable);
              
        }

        public static void addEntity(Entity e)
        {
            if (Game1.online)
            {
                outmsg = Client.CreateMessage();
                outmsg.Write((byte)NetworkAction.AddEntity);
                try
                {
                    EntityPacket packet = new EntityPacket(e);
                    byte[] objectArray = packet.Serialize(true); //Convert Object to bytearray
                    outmsg.Write(objectArray.Length); //SEND LENGTH OF ARRAY
                    outmsg.Write(objectArray); //SEND ARRAY ITSELF
                }

                catch(Exception b)
                {
                    Global.chat.WriteLine(new graphics.ui.ChatMessage("Network", Color.White, e + " Objekt kann noch nicht in bytes umgewandelt werden!", Color.Red));
                    Debug.WriteLine(b);
                    return;               
                }

                semdcbytespersecond += outmsg.LengthBytes;
                Client.SendMessage(outmsg, NetDeliveryMethod.Unreliable);

                return;
            }
        }
        public static void Update()
        {          
            while ((incmsg = Client.ReadMessage()) != null)
            {
                switch (incmsg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        {
                            byte headStringMessage = incmsg.ReadByte();

                            switch (headStringMessage)
                            {
                                case (byte) NetworkAction.Connect: //CONNECT
                                    {
                                        recbytespersecond += incmsg.LengthBytes;
                                        int arraylength = incmsg.ReadInt32();
                                        EntityPacket receiveE = EntityPacket.Deserialize(incmsg.ReadBytes(arraylength));
                                        Player e = new Player();
                                        if (level.getClientPlayer().ID == 0 || level.getClientPlayer().ID == receiveE.ID.Value) break;
                                        e.ApplyPacket(receiveE);                                       
                                                                                
                                        Debug.WriteLine(receiveE.Name.Value);
                                        level.AddEntity(e);
                                        Global.chat.WriteLine(new graphics.ui.ChatMessage("Network", Color.Blue, receiveE.Name.Value + " joined!", Color.Gray));
                                    }
                                    break;

                                case (byte)NetworkAction.ConnectResponse: //CONNECT
                                    {
                                        int id = incmsg.ReadInt32();
                                        level.getClientPlayer().ID = id;
                                        Game1.online = true;
                                        Global.chat.WriteLine(new graphics.ui.ChatMessage("Network", Color.Blue, "Connected to server with ID: " + id, Color.Gray));
                                    }
                                    break;

                                case (byte)NetworkAction.Disconnect: //Disconnect
                                    {
                                        int id = incmsg.ReadInt32();
                                        //TODO REMOVE ENTITY
                                        foreach (var e in level.mobs)
                                        {
                                            if(e != level.getClientPlayer() && e.ID == id)
                                            {
                                                Global.chat.WriteLine(new graphics.ui.ChatMessage("Network", Color.Blue, e.name + " left!", Color.Gray));
                                                level.remEntity(e);
                                                break;
                                            }
                                        }
                                       
                                    }
                                    break;

                                case (byte)NetworkAction.Deny: //CONNECTION DENY
                                    {
                                        //EMPTY
                                    }
                                    break;
                              
                                case (byte)NetworkAction.Chat: //CHAT
                                    {
                                        recbytespersecond += incmsg.LengthBytes;
                                        int id = incmsg.ReadInt32();
                                        string message = incmsg.ReadString();
                                        foreach (var e in level.mobs)
                                        {
                                            if(e.ID == id)
                                            {
                                                Global.chat.WriteLine(new ChatMessage(e.name, Color.Blue, message, Color.White));
                                                e.say(message);
                                                break;
                                            }
                                        }                                                                          
                                    }
                                    break;

                                case (byte)NetworkAction.AddEntity: //ADDENTITY
                                    {
                                        recbytespersecond += incmsg.LengthBytes;
                                        int arraylength = incmsg.ReadInt32();
                                        EntityPacket receiveE = EntityPacket.Deserialize(incmsg.ReadBytes(arraylength));
                                        Entity e = (Entity)Activator.CreateInstance(receiveE.EntityType.Value);
                                        e.ApplyPacket(receiveE);
                                        level.AddEntity(e);
                                    }
                                    break;

                                case (byte)NetworkAction.RefreshEntity: //REFRESHENTITY
                                    {
                                        int arraylength = incmsg.ReadInt32();
                                        EntityPacket receiveE = EntityPacket.Deserialize(incmsg.ReadBytes(arraylength));                                   

                                        if (receiveE.ID.Value == level.getClientPlayer().ID) //CLIENT PLAYER
                                        {
                                           break;
                                        }                                                                                
                                        foreach (var e in level.entities)
                                        {
                                            if (e.ID == receiveE.ID.Value) //FIND ENTITY
                                            {
                                                //receiveE.Position.Value = e.Position; //TEMPORARY SOLUTION TO NOT SYNC POSITION DIRECTLY
                                                e.ApplyPacket(receiveE);
                                                break;
                                            }
                                        }
                                    
                                    }
                                    break;

                                case (byte)NetworkAction.RefreshEntities: //ADDENTITY
                                    {
                                        recbytespersecond += incmsg.LengthBytes;
                                        level.clearEntities();
                                        int counter = incmsg.ReadInt32(); //ENTITIES COUNT
                                        for (int i = 0; i < counter; i++)
                                        {
                                            int length = incmsg.ReadInt32(); //ARRAYLENGTH
                                            byte[] packetbytes = incmsg.ReadBytes(length);
                                            EntityPacket entitypacket = EntityPacket.Deserialize(packetbytes);

                                            if (entitypacket.ID.Value == level.getClientPlayer().ID) continue;
                                            Entity ent = (Entity)Activator.CreateInstance(entitypacket.EntityType.Value);
                                            ent.ApplyPacket(entitypacket);
                                            level.AddEntity(ent);
                                        }                                                                            
                                    }
                                    break;                                                                                        
                            }
                        }
                        break;

                    case NetIncomingMessageType.DiscoveryResponse:
                        {
                            string ip = incmsg.SenderEndPoint.ToString();
                            string name = incmsg.ReadString();
                            string players = incmsg.ReadString();
                            Debug.WriteLine(("Found server at " + ip + " name: " + name));
                            foreach(var server in servers)
                            {
                                if (server.ip == ip) return;
                            }
                            servers.Add(new ListMenuElement(name, ip, players));
                            break;
                        }
                }
                recbytespersecond += incmsg.LengthBytes;
                Client.Recycle(incmsg);
            } //INCOMING MESSAGES

            if (!Game1.online) return;
            outmsg = Client.CreateMessage();
            outmsg.Write((byte)NetworkAction.RefreshEntity); //REFRESH CLIENT PLAYER
            EntityPacket packet = new EntityPacket(level.getClientPlayer(), level.getClientPlayer().EntityPacket);
            packet.MoveTo.Value = level.getClientPlayer().Position;
            byte[] objectArray = packet.Serialize(true); //Convert Object to bytearray

            outmsg.Write(objectArray.Length); //SEND LENGTH OF ARRAY
            outmsg.Write(objectArray); //SEND ARRAY ITSELF
            semdcbytespersecond += outmsg.LengthBytes;

            Client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);

            start = DateTime.Now.TimeOfDay;
            if (start >= end)
            {
                RecievingBytesPerSecond = recbytespersecond;
                SendingBytesPerSecond = semdcbytespersecond;
                semdcbytespersecond = 0;
                recbytespersecond = 0;
                end = start + TimeSpan.FromSeconds(1);
            }
            
           
        }
        
    }
}
