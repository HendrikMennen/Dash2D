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
        public MouseState CurrentMouseState { get; private set; }
        public MouseState PreviousMouseState { get; private set; }
        public KeyboardState CurrentKeyboardState { get; private set; }
        public KeyboardState PreviousKeyboardState { get; private set; }
        public GamePadState CurrentGamepadState { get; private set; }
        public GamePadState PreviousGamepadState { get; private set; }
        public Level level;
        public List<string> text = new List<string>();
        private bool active;
        public event EventHandler TextInput;
        public delegate void EventHandler(object b, TextInputEventArgs e);

        public void Update(bool active, MouseState mouseState, KeyboardState keyboardState, GamePadState gamepadState)
        {
            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = mouseState;
            CurrentKeyboardState = keyboardState;
            CurrentGamepadState = gamepadState;
            this.active = active;  
            
        }
                                
        public void UpdatePrev()
        {
            PreviousKeyboardState = CurrentKeyboardState;
            PreviousGamepadState = CurrentGamepadState;
            PreviousMouseState = CurrentMouseState;
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
                return new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
            }
        }

        public bool MouseLeftButtonPressed()
        {
            if (CurrentMouseState.LeftButton == ButtonState.Released && PreviousMouseState.LeftButton == ButtonState.Pressed)
            {
                if(active) return true;
            }
            return false;
        }

        public bool MouseRightButtonPressed()
        {
            if (CurrentMouseState.RightButton == ButtonState.Released && PreviousMouseState.RightButton == ButtonState.Pressed)
            {
                if(active) return true;
            }
            return false;
        }       

        public bool KeyDown(Keys key)
        {
            if (active) return CurrentKeyboardState.IsKeyDown(key);
            return false;
        }
        public bool KeyPressed(Keys key)
        {
            if (CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key))
            {
                if (active) return true;
            }
            return false;
        }

        public void Init(Level level)
        {
            this.level = level;
        }
        
        public Vector2 GetMapPos(Vector2 pos)
        {
            return Global.camera.ScreenToWorld(pos);
        }

        public Vector2 GetScreenPos(Vector2 pos)
        {
            return Global.camera.WorldToScreen(pos);
        }
        
        public Keys[] GetKeysPressed()
        {          
            var keys = CurrentKeyboardState.GetPressedKeys();
            var pkeys = PreviousKeyboardState.GetPressedKeys();
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

        public void Reset()
        {
            PreviousKeyboardState = CurrentKeyboardState;
        }

        public string GetInput()
        {        
            string input = "";
            Keys[] keyPressed = GetKeysPressed();
            if (active) foreach (var key in keyPressed)
            {
                string k = key.ToString();
                if (k.Length > 1)
                {
                    if (key == Keys.Space) input += " ";
                    if (key == Keys.D0) if (!CurrentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "0";
                    if (key == Keys.D1) if (!CurrentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "1";
                    if (key == Keys.D2) if (!CurrentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "2";
                    if (key == Keys.D3) if (!CurrentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "3";
                    if (key == Keys.D4) if (!CurrentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "4";
                    if (key == Keys.D5) if (!CurrentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "5";
                    if (key == Keys.D6) if (!CurrentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "6";
                    if (key == Keys.D7) if (!CurrentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "7";
                    if (key == Keys.D8) if (!CurrentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "8";
                    if (key == Keys.D9) if (!CurrentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "9";

                    if (key == Keys.OemPeriod) if (!CurrentKeyboardState.IsKeyDown(Keys.LeftShift)) input += ".";

                    if (key == Keys.D1) if (CurrentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "!";
                    if (key == Keys.OemBackslash) if (CurrentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "?";
                    if (key == Keys.D7) if (CurrentKeyboardState.IsKeyDown(Keys.LeftShift)) input += "/"; 
                }
                else
                {
                    if (!CurrentKeyboardState.IsKeyDown(Keys.LeftShift)) k = k.ToLower();
                    input += k;
                }
            }
            return input;
        }
    }
}
