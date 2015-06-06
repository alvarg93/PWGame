using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game4
{
    public class InputManager
    {
        KeyboardState prevKeyState, keyState;

        public KeyboardState KeyState
        {
            get { return keyState; }
            set { keyState = value; }
        }

        public KeyboardState PrevKeyState
        {
            get { return prevKeyState; }
            set { prevKeyState = value; }
        }

        public void Update()
        {
            prevKeyState = keyState;
            keyState = Keyboard.GetState();
        }

        public bool KeyPressed(Keys key)
        {
            if (keyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
                return true;
            return false;
        }
        public bool KeyPressed(params Keys[] keys)
        {
            bool result = true;
            foreach (Keys key in keys)
            {
                if (!(keyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key)))
                    return false;
            }
            return result;
        }
        public bool KeyReleased(Keys key)
        {
            if (!(keyState.IsKeyUp(key) && prevKeyState.IsKeyDown(key)))
                return true;
            return false;
        }
        public bool KeyReleased(params Keys[] keys)
        {
            bool result = true;
            foreach (Keys key in keys)
            {
                if (!(keyState.IsKeyUp(key) && prevKeyState.IsKeyDown(key)))
                    return false;
            }
            return result;
        }

        public bool KeyDown(Keys key)
        {
            return keyState.IsKeyDown(key);
        }

        public bool KeyDown(params Keys[] keys)
        {
            bool result = true;
            foreach (Keys key in keys)
                if (!keyState.IsKeyDown(key))
                    return false;
            return result;
        }
        public bool KeyUp(Keys key)
        {
            return keyState.IsKeyUp(key);
        }

        public bool KeyUp(params Keys[] keys)
        {
            bool result = true;
            foreach (Keys key in keys)
                if (!keyState.IsKeyUp(key))
                    return false;
            return result;
        }

    }
}
