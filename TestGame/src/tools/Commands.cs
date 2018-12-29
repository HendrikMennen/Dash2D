using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.effects;
using TestGame.src.entities;
using TestGame.src.graphics.ui;
using TestGame.src.graphics.ui.menu;
using TestGame.src.level;
using static TestGame.Game1;

namespace TestGame.src.tools
{
    public class Commands
    {
        private Game1 game;
        private Level level;
        private Chat chat;
        private Inventory inv;
        private Interface menu;
        private EffectManager effectmanager;
        private MapEditor editor;

        public void init(Game1 game, Level level, Chat chat, Inventory inv, Interface menu, EffectManager effectmanager, MapEditor editor)
        {
            this.game = game;
            this.level = level;
            this.chat = chat;
            this.inv = inv;
            this.menu = menu;
            this.effectmanager = effectmanager;
            this.editor = editor;
        }

        public void command(string[] text)
        {
            foreach (var t in text) command(t);
        }

        public void command(string text)
        {           
            Debug.WriteLine(text);
            string[] param;
            try
            {
                param = text.Split(' ');
                param[0] = param[0].ToLower();
                if (param[0] == "!edit") {
                    if (Game1.currentGamestate != GameState.Editor)
                    {
                        Game1.currentGamestate = GameState.Editor;
                        Game1.UI_Margin = new Rectangle(editor.TileSelectorBackground.Width, 0, 0, 0);
                    }
                    else
                    {
                        Game1.currentGamestate = GameState.Running;
                        Game1.UI_Margin = new Rectangle(0, 0, 0, 0);
                    }
                        chat.changeResolution();
                        inv.updateResolution();
                }
                


                if (param[0] == "!map")  level.ChangeMap(Int32.Parse(param[1]));
                if (param[0] == "!give") inv.addItem(Int32.Parse(param[1]), Int32.Parse(param[2]));
                if (param[0] == "!sethp") level.getClientPlayer().Health = Int32.Parse(param[1]);
                if (param[0] == "!setname") level.getClientPlayer().name = param[1];
                if (param[0] == "!changemenu") menu.changeMenu(Int32.Parse(param[1]));
                if (param[0] == "!drunk") command("!addeffect 0 1000");
                if (param[0] == "!portal") command("!addeffect 1 100");
                if (param[0] == "!settime") Global.time.CurrentTime = float.Parse(param[1]);
                if (param[0] == "!setvolume") Global.sound.MasterVolume = float.Parse(param[1]);
                if (param[0] == "!hb") Global.showHitboxes = bool.Parse(param[1]);
                if (param[0] == "!traffic") Global.chat.WriteLine(new ChatMessage("Network", " " + netcode.NetCode.RecievingBytesPerSecond + " bytes per Second"));
                if (param[0] == "!addeffect")
                {
                    effectmanager.addEffect(Int32.Parse(param[1]), Int32.Parse(param[2]));
                }
                if (param[0] == "!chat")
                {
                    string say = "";
                    for(int i = 2; i < param.Length; i++)
                    {
                        say += param[i] + " ";
                    }
                    if (param[2].Length > 0) chat.WriteLine(new ChatMessage(param[1], say));

                }
                if (param[0] == "!close")
                {
                    chat.Open = false;
                    menu.Open = false;
                }
                if (param[0] == "!exit")
                {
                    command("!disconnect");
                    game.Exit();
                }
                if (param[0] == "!tp")
                {
                    level.getClientPlayer().Position = new Vector2(Int32.Parse(param[1]), Int32.Parse(param[2]));
                }
                if (param[0] == "!searchlocalservers")
                {
                    for(int i = 0; i < netcode.NetCode.localServers.Count; i++) 
                    netcode.NetCode.Config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
                    netcode.NetCode.Client.DiscoverLocalPeers(25232);
                }
                if (param[0] == "!addserver")
                {
                    netcode.NetCode.servers.Add(new graphics.ui.menu.components.ListMenuElement("Unknown", param[1], "0/0"));
                }
                if (param[0] == "!angle") Global.camera.Rotation = float.Parse(param[1]);
                if (param[0] == "!connect")
                {
                    command("!close");
                    netcode.NetCode.Connect(param);
                }
                if (param[0] == "!disconnect")
                {
                    if (Game1.online)
                    {
                        netcode.NetCode.Disconnect(param);                       
                    }
                }
                if (param[0] == "!changeresolution")
                {
                    game.ChangeResolution(Int32.Parse(param[1]), Int32.Parse(param[2]));
                }
              
                //ADDITIONAL DEV COMMANDS
            }
            catch (Exception e)
            {
                Global.chat.WriteLine(new ChatMessage("ERROR",Color.DarkRed, "Wrong Input Syntax!",Color.White));
                Debug.WriteLine(e);
            }
            if (Game1.currentGamestate == GameState.Editor)
            {
                param = text.Split(' ');
                //param[0] = param[0].ToLower();
                try
                {
                    editor.cmd(param);
                }catch(Exception b)
                {
                    Debug.Write(b.ToString());
                }                                                                                          
            }
        }
    }
}
