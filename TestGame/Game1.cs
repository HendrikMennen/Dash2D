using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TestGame.src.graphics.effects;
using TestGame.src.graphics;
using TestGame.src.entities;
using TestGame.src.level;
using TestGame.src.input;
using TestGame.src.graphics.ui;
using TestGame.src.netcode;
using Lidgren.Network;
using TestGame.src.graphics.ui.menu;
using TestGame.src.tools;
using TestGame.src.effects;
using TestGame.src.sound;
using TestGame.src.modules;
using System.Diagnostics;
using TestGame.src.entities.objects.furniture;

namespace TestGame
{
    public class Game1 : Game
    {
        #region Variables

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Level level;
        Inventory inv;
        Healthbar hb;
        Input input;
        Chat chat;
        Time time;
        Interface menu;
        SpriteFont font;
        Commands cmd;
        MapEditor editor;
        EffectManager effectmanager;
        Camera camera;
        Sound sound;
        FrameCounter frameCounter;

        int noFullscreenResolutionW, noFullscreenResolutionH;
        int mouseX = 0, mouseY = 0;

        RenderTarget2D lightsTarget, mainTarget;
        Texture2D pointer;
        Effect basiclight, basicshadow;

        public static bool playerlight = true;
        public static int MapWidth, MapHeight;
        public static int SpriteHeight, SpriteWidth;
        public static Rectangle UI_Margin = new Rectangle();
        public static bool online = false;
        public static int screenWidth = 900, screenHeight = 500;
        public static float SOUNDEFFECTVOLUME = 0.1f;


        public enum GameState
        {
            Loading,
            Running,
            Pause,
            MainMenu,
            Editor,
        }
        public static GameState currentGamestate = GameState.Running;

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            inv = new Inventory();
            hb = new Healthbar();
            input = new Input();
            level = new Level();
            chat = new Chat();
            menu = new Interface();
            cmd = new Commands();
            time = new Time();
            sound = new Sound();
            editor = new MapEditor();
            effectmanager = new EffectManager();
            frameCounter = new FrameCounter();

            //optional modules
            Global.mapModules.Add(new ChessGame(64*4-32,64*4));

            time.CurrentTime = 0.05f;
        }

        public void ChangeResolution(int width, int height)
        {
            screenWidth = width;
            screenHeight = height;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            inv.updateResolution();
            menu.changeResolution();
            chat.changeResolution();
            editor.ChangeResolution();
            hb.updateResolution();
            graphics.ApplyChanges();

            double scale = width / 300;
            scale = Math.Round(scale);
            camera.Zoom = (float)scale;
            camera.ViewportWidth = GraphicsDevice.Viewport.Width;
            camera.ViewportHeight = GraphicsDevice.Viewport.Height;

            var pp = GraphicsDevice.PresentationParameters;
            lightsTarget = new RenderTarget2D(
            GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            mainTarget = new RenderTarget2D(
            GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);

            Debug.WriteLine("w" + height + " a" + graphics.PreferredBackBufferWidth);
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {

            inv.Init(level, cmd);
            input.init(level);
            chat.init(level, input, cmd);
            menu.init(input, cmd);
            cmd.init(this, level, chat, inv, menu, effectmanager, editor);
            level.init(input);
            editor.Init(level, input, GraphicsDevice.Viewport);
            sound.Init(level);
            hb.Init(level);
            foreach (var module in Global.mapModules)
            {
                module.Init(level);
            }

            NetCode.init(level, cmd);

            NetCode.Config = new NetPeerConfiguration("TestGame");
            NetCode.Client = new NetClient(NetCode.Config);
            NetCode.Client.Start();

            camera = new Camera(GraphicsDevice.Viewport);
            
            Global.Init(camera, time, sound, chat, cmd, new Random());

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Global.loadContent(Content);
            font = Content.Load<SpriteFont>("fonts/dashFont");
            pointer = Content.Load<Texture2D>("textures/ui/misc/pointer");
            basiclight = Content.Load<Effect>("effects/lighteffect/lighteffect");
            basicshadow = Content.Load<Effect>("effects/shadow/shadoweffect");

            level.loadContent(Content);
            inv.loadContent(Content);
            hb.loadContent(Content);
            chat.loadContent(Content);
            menu.loadContent(Content);
            editor.loadContent(Content);
            foreach (var module in Global.mapModules)
            {
                module.LoadContent(Content);
            }

            level.AddEntity(new Player(new Vector2(50,50), 0));           

            ChangeResolution(screenWidth, screenHeight);
            Window.TextInput += new EventHandler<TextInputEventArgs>(input.WindowsTextInput);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
#if true
            level.getClientPlayer().EntityPacket = new EntityPacket(level.getClientPlayer()); //DIFFERENCE DETECTION
            float x = 0, y = 0;
            input.update(this.IsActive, Mouse.GetState(), Keyboard.GetState(), GamePad.GetState(PlayerIndex.One));           
            chat.update();
            mouseX = (int)input.MousePos.X;
            mouseY = (int)input.MousePos.Y;

            if (!chat.Open && !menu.Open)
            {
                if (input.KeyDown(Keys.LeftShift)) level.getClientPlayer().speed = 2;
                else level.getClientPlayer().speed = 1;
                if (input.KeyDown(Keys.W)) y -= level.getClientPlayer().speed;
                if (input.KeyDown(Keys.A)) x -= level.getClientPlayer().speed;
                if (input.KeyDown(Keys.S)) y += level.getClientPlayer().speed;
                if (input.KeyDown(Keys.D)) x += level.getClientPlayer().speed;

                y += (int)Math.Round(input.currentGamepadState.ThumbSticks.Left.Y) * -1;
                x += (int)Math.Round(input.currentGamepadState.ThumbSticks.Left.X);

                if (input.KeyPressed(Keys.PageUp)) time.CurrentTime += 0.1f;
                if (input.KeyPressed(Keys.PageDown)) time.CurrentTime -= 0.1f;

                if (input.KeyPressed(Keys.T))
                {
                    chat.Open = true;
                }

                if (input.KeyPressed(Keys.L))
                {
                    if (playerlight) playerlight = false;
                    else playerlight = true;
                }
            }



            if (input.KeyPressed(Keys.F11))
            {

                if (graphics.IsFullScreen)
                {
                    graphics.IsFullScreen = false;
                    ChangeResolution(noFullscreenResolutionW, noFullscreenResolutionH);
                }
                else
                {
                    graphics.IsFullScreen = true;
                    noFullscreenResolutionW = screenWidth;
                    noFullscreenResolutionH = screenHeight;
                    ChangeResolution(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
                }
            }

            if (input.KeyPressed(Keys.Escape))
            {
                if (chat.Open) chat.Open = false;
                if (menu.Open) menu.Open = false;
                else menu.Open = true;
            }

            switch (currentGamestate)
            {

                case GameState.Running:

                    if (input.currentMouseState.ScrollWheelValue > input.previousMouseState.ScrollWheelValue && input.KeyDown(Keys.LeftControl)) camera.AdjustZoom(0.2f);
                    if (input.currentMouseState.ScrollWheelValue < input.previousMouseState.ScrollWheelValue && input.KeyDown(Keys.LeftControl)) camera.AdjustZoom(-0.2f);
                    level.getClientPlayer().move(new Vector2(x,y));
                    camera.CenterOn(new Vector2(level.getClientPlayer().Position.X + level.getClientPlayer().width/2, level.getClientPlayer().Position.Y + level.getClientPlayer().height / 2), true);
                    inv.Update(input);
                    break;

                case GameState.Editor:

                    editor.Update();
                    if (editor.tilesetScroll) editor.moveTileset(new Vector2(x, y));
                    else camera.MoveCamera(new Vector2(x, y));
                    if (editor.currentEditorMode == MapEditor.EditorMode.EntityAdd) inv.Update(input);
                    break;

                case GameState.Pause:

                    break;
            }

            level.update();

            effectmanager.Update(input);
            menu.update();
            hb.Update(gameTime);
            sound.update();
            NetCode.Update(); //After all movement !important
            input.updatePrev();
#endif
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            float width = (screenWidth / camera.Zoom);
            float height = (int)(screenHeight / camera.Zoom);
            var viewMatrix = camera.TranslationMatrix;
            float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds; //FPS
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            frameCounter.Update(deltaTime);
#if true

            //RENDER TARGET MAIN
            GraphicsDevice.SetRenderTarget(mainTarget);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, viewMatrix);
            level.Draw(spriteBatch);
            if (!menu.Open && !chat.Open) inv.drawPreview(spriteBatch);
            spriteBatch.End();
            //MAIN END

            //RENDER TARGET LIGHT
            GraphicsDevice.SetRenderTarget(lightsTarget);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, null, null, null, viewMatrix);
            level.DrawLight(spriteBatch);
            spriteBatch.End();
            //LIGHT END         

            //SPLICE RENDER TARGETS TOGETHER
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            basiclight.Parameters["lightMask"].SetValue(lightsTarget);
            basiclight.Parameters["AmbientIntensity"].SetValue(time.CurrentTime);
            basiclight.CurrentTechnique.Passes[0].Apply();
          
            spriteBatch.Draw(mainTarget, Vector2.Zero, Color.White);
            //spriteBatch.Draw(shadowTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
            //

            //MAPEDITOR & INTERFACE
            int xStart = 10; //UI START X
            switch (currentGamestate)
            {
                case GameState.Running:
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp); //UI
                    inv.Draw(spriteBatch);
                    hb.Draw(spriteBatch);                    
                    spriteBatch.End();
                    if (Global.showHitboxes)
                    {
                        spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.TranslationMatrix);
                        editor.DrawPreview(spriteBatch);
                        spriteBatch.End();
                    }
                    break;

                case GameState.Editor:
                    xStart += editor.TileSelectorBackground.Width;
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.TranslationMatrix);
                    editor.DrawPreview(spriteBatch);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
                    editor.Draw(spriteBatch);
                    if (editor.currentEditorMode == MapEditor.EditorMode.EntityAdd) inv.Draw(spriteBatch);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, editor.TilesetView.TranslationMatrix);
                    editor.DrawTileset(spriteBatch);
                    spriteBatch.End();
                    break;

                case GameState.Pause:
                    break;
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp); //UI

            foreach (var mob in (level.mobs))
            {
                if (mob.mapid == level.getClientPlayer().mapid) mob.renderChat(spriteBatch); //Chatbubble + Name
            }

            spriteBatch.DrawString(font, "X: " + level.getClientPlayer().Position.X.ToString(), new Vector2(xStart, 10), Color.Black);
            spriteBatch.DrawString(font, "Y: " + level.getClientPlayer().Position.Y.ToString(), new Vector2(xStart + 120, 10), Color.Black);
            spriteBatch.DrawString(font, "Map: " + level.currentMapName, new Vector2(xStart, 40), Color.Black);
            spriteBatch.DrawString(font, "FPS: " + Math.Round(frameCounter.CurrentFramesPerSecond), new Vector2(xStart, 70), Color.Black);
            spriteBatch.DrawString(font, "RenderedTiles: " + Level.RenderedTiles, new Vector2(xStart, 100), Color.Black);
            if (Game1.online)
            {
                spriteBatch.DrawString(font, "Receiving: " + Math.Round((float)NetCode.RecievingBytesPerSecond/100f)/10 + "kb/s", new Vector2(xStart, 130), Color.Black);
                spriteBatch.DrawString(font, "Sending: " + Math.Round((float)NetCode.SendingBytesPerSecond/100f)/10 + "kb/s", new Vector2(xStart, 160), Color.Black);
            }

            chat.render(spriteBatch);
            menu.render(spriteBatch);

            spriteBatch.Draw(pointer, new Rectangle(mouseX, mouseY, pointer.Width, pointer.Height), Color.MonoGameOrange);
            spriteBatch.End();
            //
#endif

            base.Draw(gameTime);
        }

               
    }
}
