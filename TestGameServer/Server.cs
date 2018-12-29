using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestGame.src.entities;
using TestGame.src.items;
using TestGame.src.level;
using TestGame.src.tools;
using System.Diagnostics;
using TestGame.src.entities.objects.furniture;
using TestGame.src.netcode;

namespace TestGame
{
    class Server
    {
        private static ServerLevel serverLevel;
        static void Main(string[] args)
        {
            serverLevel = new ServerLevel();

            Network.Config = new NetPeerConfiguration("TestGame")
            {
                Port = 25232
            }; // The server and the client program must also use this name, so that can communicate with each other.
            Network.Server = new NetServer(Network.Config);
            Network.Config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            Network.Server.Start();

            Console.Title = "TestGame Server GAMMA";
            WriteLine("Server started on Port: " + Network.Config.Port, ConsoleColor.Blue);
            WriteLine("Waiting for connections...", ConsoleColor.Blue);
            serverLevel.LoadEntities();            

            //INIT
            Global.rdm = new Random();
            StaticItems s = new StaticItems();

            while (true)
            {
                TimerTick(); 
            }                 
        }     
        
        static void WriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void TimerTick()
        {
            serverLevel.UpdateFirst();
            Network.Update();
            serverLevel.Update();
            Thread.Sleep(16); //ca 60 tick           
        }

        class Network // A Basics Network class
        {
            private const int MAXCLIENTS = 20;
            public static NetServer Server; //the Server
            public static NetPeerConfiguration Config; //the Server config
                                                       /*public*/
            static NetIncomingMessage incmsg; //the incoming messages that server can read from clients
            public static NetOutgoingMessage outmsg; //the outgoing messages that clients can receive and read

            public static void Update()
            {
                while ((incmsg = Server.ReadMessage()) != null) //while the message is received, and is not equal to null...
                {
                    switch (incmsg.MessageType) //There are several types of messages (see the Lidgren Basics tutorial), but it is easier to just use it the most important thing the "Data".
                    {
                        case NetIncomingMessageType.Data:
                            {

                                byte headStringMessage = incmsg.ReadByte(); 

                                switch (headStringMessage)
                                {
                                    case (byte)NetCode.NetworkAction.Connect: //CONNECT
                                        {                                           
                                            int arraylength = incmsg.ReadInt32();
                                            EntityPacket receiveE = EntityPacket.Deserialize(incmsg.ReadBytes(arraylength));                                          
                                            Player p = new Player();
                                            p.ApplyPacket(receiveE);
                                            ServerPlayer sp = new ServerPlayer(p, incmsg.SenderConnection);
                                            serverLevel.Connect(sp);
                                            serverLevel.RefreshEntities(incmsg.SenderConnection, receiveE.MapID.Value);
                                        }
                                        break;

                                    case (byte)NetCode.NetworkAction.Chat: //CHAT
                                        {
                                            int id = incmsg.ReadInt32();
                                            string message = incmsg.ReadString();

                                            foreach(var e in serverLevel.mobs)
                                            {
                                                if(e.ID == id)
                                                {
                                                    WriteLine("[" + e.name + "]: " + message, ConsoleColor.Magenta);
                                                    break;
                                                }
                                            }    
                                            
                                            //DIRECTLY FORWARD?
                                            outmsg = Server.CreateMessage();
                                            outmsg.Write((byte)NetCode.NetworkAction.Chat);
                                            outmsg.Write(id);
                                            outmsg.Write(message);
                                            Server.SendMessage(outmsg, Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                                        }
                                        break;
                                    case (byte)NetCode.NetworkAction.RefreshEntity: //UPDATE ENTITY
                                        {
                                            int arraylength = incmsg.ReadInt32();
                                            EntityPacket receiveE = EntityPacket.Deserialize(incmsg.ReadBytes(arraylength));
                                            foreach (var p in serverLevel.players)
                                            {
                                                if(p.Player.ID == receiveE.ID.Value)
                                                {
                                                    p.timeOut = 0;
                                                    break;
                                                }
                                            }
                                            foreach (var e in serverLevel.entities)
                                            {
                                                if(e.ID == receiveE.ID.Value)
                                                {
                                                    e.ApplyPacket(receiveE);
                                                    break;
                                                }                                              
                                            }
                                        }
                                        break;

                                    case (byte)NetCode.NetworkAction.AddEntity: //ADDENTITY
                                        {
                                            try
                                            {
                                                int arraylength = incmsg.ReadInt32();
                                                WriteLine("Receiving " + arraylength + " Bytes...", ConsoleColor.DarkYellow);
                                                EntityPacket receiveE = EntityPacket.Deserialize(incmsg.ReadBytes(arraylength));
                                                Entity e = (Entity)Activator.CreateInstance(receiveE.EntityType.Value);
                                                e.ApplyPacket(receiveE);
                                                serverLevel.AddEntity(e);
                                               
                                                WriteLine(e.GetType() + " created at (" + receiveE.Position.Value.X + "|" + receiveE.Position.Value.Y + ") on Map: " + receiveE.MapID.Value, ConsoleColor.Blue);                                             
                                            }
                                            catch (Exception e)
                                            {
                                                WriteLine(e.ToString(), ConsoleColor.DarkRed);
                                                continue;
                                            }
                                        }
                                        break;

                                    case (byte)NetCode.NetworkAction.OnClick: //ONCLICK
                                        {
                                            try
                                            {
                                                int entityid = incmsg.ReadInt32();                                               
                                                int itemid = incmsg.ReadInt32();

                                                foreach (var e in serverLevel.entities)
                                                {
                                                    if (e.ID == entityid)
                                                    {                                                       
                                                        WriteLine("Entity ONCLICK at (" + e.Position.X + "|" + e.Position.Y + ") on Map: " + e.mapid, ConsoleColor.Blue);
                                                        e.OnClick(StaticItems.Items[itemid]);
                                                        break;
                                                    }
                                                }                                                                                             
                                            }
                                            catch (Exception e)
                                            {
                                                WriteLine(e.ToString(), ConsoleColor.DarkRed);
                                                continue;
                                            }
                                        }
                                        break;                                   

                                    case (byte)NetCode.NetworkAction.Disconnect: //DISCONNECT
                                        {
                                            try
                                            {
                                                int id = incmsg.ReadInt32();

                                                for (int i = 0; i < serverLevel.players.Count; i++)
                                                {
                                                    if (id == serverLevel.players[i].Player.ID) serverLevel.Disconnect(serverLevel.players[i]);
                                                    WriteLine(id + " Disconnected.", ConsoleColor.Red);
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                WriteLine(e.ToString(), ConsoleColor.DarkRed);
                                                continue;
                                            }

                                        }
                                        break;
                                }
                            }
                            break;
                        case NetIncomingMessageType.DiscoveryRequest:
                            {
                                outmsg = Server.CreateMessage();
                                outmsg.Write("TestGame Server");
                                outmsg.Write(Network.Server.ConnectionsCount + "/" + MAXCLIENTS);
                                Server.SendDiscoveryResponse(outmsg, incmsg.SenderEndPoint);
                                break;
                            }                                            
                    }
                    Server.Recycle(incmsg); //All messages processed at the end of the case, delete the contents.
                }
            }
        }

    
        class ServerLevel : Level
        {
            int idcounter = 1;
            public new List<ServerPlayer> players = new List<ServerPlayer>();
            public override void AddEntity(Entity e)
            {                
                e.ID = idcounter;
                idcounter++;
                base.AddEntity(e);
                e.EntityPacket = new EntityPacket(e);
                if (e is Player) return;

                if (Network.Server.ConnectionsCount == 0) return;
                Network.outmsg = Network.Server.CreateMessage();

                Network.outmsg.Write((byte)NetCode.NetworkAction.AddEntity);
                EntityPacket ep = new EntityPacket(e);
                byte[] objectArray = ep.Serialize(true); //Convert Object to bytearray
                Network.outmsg.Write(objectArray.Length); //SEND LENGTH OF ARRAY
                Network.outmsg.Write(objectArray); //SEND ARRAY ITSELF

                Network.Server.SendMessage(Network.outmsg, Network.Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
            }

            public void Connect(ServerPlayer e)
            {
                AddEntity(e.Player); //ADD ID TO PLAYER
                players.Add(e);

                //SEND ID TO PLAYER
                Network.outmsg = Network.Server.CreateMessage();
                Network.outmsg.Write((byte)NetCode.NetworkAction.ConnectResponse);
                Network.outmsg.Write(e.Player.ID);
                
                Network.Server.SendMessage(Network.outmsg, e.user, NetDeliveryMethod.Unreliable, 0);

                //SEND PLAYER TO OTHER CLIENTS
                Network.outmsg = Network.Server.CreateMessage();
                Network.outmsg.Write((byte)NetCode.NetworkAction.Connect);
                EntityPacket ep = new EntityPacket(e.Player);
                byte[] objectArray = ep.Serialize(true); //Convert Object to bytearray
                Network.outmsg.Write(objectArray.Length); //SEND LENGTH OF ARRAY
                Network.outmsg.Write(objectArray); //SEND ARRAY ITSELF
                Network.Server.SendMessage(Network.outmsg, Network.Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);

                WriteLine("Connect " + ep.Name.Value, ConsoleColor.Green);
            }

            public void Disconnect(ServerPlayer e)
            {
                           
                System.Threading.Thread.Sleep(100);
                if (Network.Server.ConnectionsCount != 0) //TELL OTHER CLIENTS
                {
                    Network.outmsg = Network.Server.CreateMessage();

                    Network.outmsg.Write((byte)NetCode.NetworkAction.Disconnect);
                    Network.outmsg.Write(e.Player.ID);

                    Network.Server.SendMessage(Network.outmsg, Network.Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                }

                e.user.Disconnect("bye"); //DISCONNECT PLAYER
                players.Remove(e);
                entities.Remove(e.Player);
            }
            public void RefreshEntities(List<NetConnection> user, int mapid) //ALL SERVER ENTITIES
            {
                int bytecounter = 0;
                Network.outmsg = Network.Server.CreateMessage();
                Network.outmsg.Write((byte)NetCode.NetworkAction.RefreshEntities);
                Network.outmsg.Write(entities.Count); //ENTITIES COUNT
                foreach (var e in entities)
                {
                    EntityPacket packet = new EntityPacket(e);
                    byte[] objectArray = packet.Serialize(true); //Convert Object to bytearray
                    bytecounter += objectArray.Length;
                    Network.outmsg.Write(objectArray.Length); //SEND LENGTH OF ARRAY
                    Network.outmsg.Write(objectArray); //SEND ARRAY ITSELF
                }
                WriteLine("Sending " + bytecounter + " Bytes...", ConsoleColor.DarkYellow);            
                Network.Server.SendMessage(Network.outmsg, user, NetDeliveryMethod.Unreliable, 0);
            }

            public void RefreshEntities(NetConnection user, int mapid)
            {
                List<NetConnection> users = new List<NetConnection>
                {
                    user
                };
                RefreshEntities(users, mapid);
            }

            public void RefreshEntity(List<NetConnection> user, EntityPacket packet)
            {
                Network.outmsg = Network.Server.CreateMessage();
                
                Network.outmsg.Write((byte)NetCode.NetworkAction.RefreshEntity);
                byte[] objectArray = packet.Serialize(false); //Convert Object to bytearray
                //WriteLine("Sending " + objectArray.Length + " Bytes...", ConsoleColor.DarkYellow);
                Network.outmsg.Write(objectArray.Length); //SEND LENGTH OF ARRAY
                Network.outmsg.Write(objectArray); //SEND ARRAY ITSELF

                Network.Server.SendMessage(Network.outmsg, user, NetDeliveryMethod.Unreliable, 0);
            }

            public void RefreshEntity(NetConnection user, EntityPacket packet)
            {
                List<NetConnection> users = new List<NetConnection>
                {
                    user
                };
                RefreshEntity(users, packet);
            }

            public void LoadEntities()
            {
                maps.Add(SaveGame.DeserializeMap<Map>("Disco"));
                maps.Add(SaveGame.DeserializeMap<Map>("christmas"));
                for(int i = 0; i < maps.Count; i++)
                {
                    foreach (var e in maps[i].entities)
                    {
                        e.mapid = i;
                        AddEntity(e);
                    }
                }                            
            }

            public void UpdateFirst()
            {
                foreach (var e in entities)
                {
                    e.EntityPacket = new EntityPacket(e); //WERTE SPEICHERN BEVOR SPIELER / ENTITIES SIE ÄNDERN
                }
            }

            public void Update()
            {
                foreach(var e in entities)
                {
                    if (e is Player) continue;
                    e.FixedUpdate();
                    e.EntityPacket = new EntityPacket(e, e.EntityPacket); //NUR VERÄNDERTE WERTE SPEICHERN
                    if(!e.EntityPacket.Empty())
                    if (Network.Server.ConnectionsCount > 0) RefreshEntity(Network.Server.Connections, e.EntityPacket);                      
                }               

                if (Network.Server.ConnectionsCount == players.Count) //If the number of the player object actually corresponds to the number of connected clients.
                {
                    for (int i = 0; i < players.Count; i++)
                    {
                        EntityPacket packet = new EntityPacket(players[i].Player, players[i].Player.EntityPacket);
                        if(players[i].Player.MoveTo2 == players[i].Player.MoveTo) packet.MoveTo.Value = players[i].Player.MoveTo;
                        RefreshEntity(Network.Server.Connections, packet);
                        players[i].timeOut++;
                        if (players[i].timeOut > 180) //TIMEOUT
                        {                                                  
                            WriteLine(players[i].Player.ID + " is timed out.", ConsoleColor.Red);                           
                            Disconnect(players[i]);
                        }
                    }
                }
            }
        }

        class ServerPlayer
        {
            public int timeOut = 0;
            public NetConnection user = null;
            public Player Player;

            public ServerPlayer(Player p, NetConnection user)
            {
                this.Player = p;
                this.user = user;
            }
        }   
    }
}
