using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using TestGame.src.graphics;
using TestGame.src.level;
using TestGame.src.tools;

namespace TestGame.src.input
{
    public class Input
    {
        public MouseState currentMouseState { get; private set; }
        public MouseState previousMouseState { get; private set; }
        public KeyboardState currentKeyboardState { get; private set; }
        public KeyboardState previousKeyboardState { get; private set; }
        public GamePadState currentGamepadState { get; private set; }
        public GamePadState previousGamepadState { get; private set; }
        public Level level;
        public List<string> text = new List<string>();
        private bool active;
        public event EventHandler TextInput;
        public delegate void EventHandler(object b, TextInputEventArgs e);

        public void update(bool active, MouseState mouseState, KeyboardState keyboardState, GamePadState gamepadState)
        {
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = mouseState;
            currentKeyboardState = keyboardState;
            currentGamepadState = gamepadState;
            this.active = active;  
            
        }
                                
        public void updatePrev()
        {
            previousKeyboardState = currentKeyboardState;
            previousGamepadState = currentGamepadState;
            previousMouseState = currentMouseState;
        }

        public void WindowsTextInput(object sender, TextInputEventArgs args) //WINDOWS KEYS
        {
            TextInput(this, args);
        }

        public bool ButtonDown(ButtonState button)
        {
            if (button == ButtonState.Pressed) return true;
            return false;
        }

        public Vector2 MousePos
        {
            get
            {
                return new Vector2(currentMouseState.X, currentMouseState.Y);
            }
        }

        public bool MouseLeftButtonPressed()
        {
            if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                if(active) return true;
            }
            return false;
        }

        public bool MouseRightButtonPressed()
        {
            if (currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed)
            {
                if(active) return true;
            }
            return false;
        }       

        public bool KeyDown(Keys key)
        {
            if (active) return currentKeyboardState.IsKeyDown(key);
            return false;
        }
        public bool KeyPressed(Keys key)
        {
            if (currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key))
            {
                if (active) return true;
            }
            return false;
        }

        public void init(Level level)
        {
            this.level = level;
        }
        
        public Vector2 getMapPos(Vector2 pos)
        {
            return Global.camera.ScreenToWorld(pos);
        }

        public Vector2 getScreenPos(Vector2 pos)
        {
            return Global.camera.WorldToScreen(pos);
        }
        
        public Keys[] getKeysPressed()
        {          
            var keys = currentKeyboardState.GetPressedKeys();
            var pkeys = previousKeyboardState.GetPressedKeys();
            List<Keys> pressedKeys = new List<Keys>();
            if (active) foreach (var key in keys)
            {
                bool pressed = true;
                foreach(var pkey in pkeys)
                {
                    if (pkey == key) pressed = false;
                }
                if (pressed) pressedKeys.Add(key);
            }

            return pressedKeys.ToArray();
        }

        public void reset()
        {
            previousKeyboardState = currentKeyboardState;
        }

        public string getInput()
        {        
            string input = "";
            Keys[] keyPressed = getKeysPressed();
            if (active) foreach (var key in keyPressed)
            {
                string k = key.ToString();
                if (k.Length > 1)
                {
                    if (key == Keys.Space) input += " ";
                    if (key == Keys.D0) if (!currentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "0";
                    if (key == Keys.D1) if (!currentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "1";
                    if (key == Keys.D2) if (!currentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "2";
                    if (key == Keys.D3) if (!currentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "3";
                    if (key == Keys.D4) if (!currentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "4";
                    if (key == Keys.D5) if (!currentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "5";
                    if (key == Keys.D6) if (!currentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "6";
                    if (key == Keys.D7) if (!currentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "7";
                    if (key == Keys.D8) if (!currentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "8";
                    if (key == Keys.D9) if (!currentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "9";

                    if (key == Keys.OemPeriod) if (!currentKeyboardState.IsKeyDown(Keys.LeftShift)) input += ".";

                    if (key == Keys.D1) if (currentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "!";
                    if (key == Keys.OemBackslash) if (currentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "?";
                    if (key == Keys.D7) if (currentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "/"; 
                }
                else
                {
                    if (!currentKeyboardState.IsKeyDown(Keys.LeftShift)) k = k.ToLower();
                    input += k;
                }
            }
            return input;
        }
    }
}
