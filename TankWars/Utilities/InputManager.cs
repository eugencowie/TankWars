using Microsoft.Xna.Framework.Input;
using System;

namespace TankWars
{
    /// <summary>
    /// Defines the different mouse buttons available.
    /// </summary>
    enum MouseButtons { Left, Middle, Right, XButton1, XButton2 }
    
    /// <summary>
    /// Manages gamepad, keyboard and mouse input using a consistant interface.
    /// </summary>
    class InputManager
    {
        // Current input states.
        private GamePadState[] m_gamepads;
        private KeyboardState m_keyboard;
        private MouseState m_mouse;

        // Previous input states.
        private GamePadState[] m_prevGamepads;
        private KeyboardState m_prevKeyboard;
        private MouseState m_prevMouse;

        public InputManager()
        {
            m_gamepads = new GamePadState[4];
            m_prevGamepads = new GamePadState[4];
        }
        
        /// <summary>
        /// Updates the input manager, getting the current input device states.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < 4; i++)
            {
                m_prevGamepads[i] = m_gamepads[i];
                m_gamepads[i] = GamePad.GetState(i);
            }

            m_prevKeyboard = m_keyboard;
            m_keyboard = Keyboard.GetState();

            m_prevMouse = m_mouse;
            m_mouse = Mouse.GetState();
        }

        // Gamepad functions

        public bool IsDown(Buttons button, int i=0) { return m_gamepads[i].IsButtonDown(button); }
        public bool WasDown(Buttons button, int i=0) { return m_prevGamepads[i].IsButtonDown(button); }

        public bool IsUp(Buttons button, int i=0) { return !IsDown(button, i); }
        public bool WasUp(Buttons button, int i=0) { return !WasDown(button, i); }

        public bool IsJustPressed(Buttons button, int i=0) { return WasUp(button, i) && IsDown(button, i); }
        public bool IsJustReleased(Buttons button, int i=0) { return WasDown(button, i) && IsUp(button, i); }

        // Keyboard functions

        public bool IsDown(Keys key) { return m_keyboard.IsKeyDown(key); }
        public bool WasDown(Keys key) { return m_prevKeyboard.IsKeyDown(key); }

        public bool IsUp(Keys key) { return !IsDown(key); }
        public bool WasUp(Keys key) { return !WasDown(key); }

        public bool IsJustPressed(Keys key) { return WasUp(key) && IsDown(key); }
        public bool IsJustReleased(Keys key) { return WasDown(key) && IsUp(key); }

        // Mouse functions

        public bool IsDown(MouseButtons button) { return GetMouseButtonState(m_mouse, button) == ButtonState.Pressed; }
        public bool WasDown(MouseButtons button) { return GetMouseButtonState(m_prevMouse, button) == ButtonState.Pressed; }

        public bool IsUp(MouseButtons button) { return !IsDown(button); }
        public bool WasUp(MouseButtons button) { return !WasDown(button); }

        public bool IsJustPressed(MouseButtons button) { return WasUp(button) && IsDown(button); }
        public bool IsJustReleased(MouseButtons button) { return WasDown(button) && IsUp(button); }

        /// <summary>
        /// Takes in a MouseButton and returns the corresponding XNA ButtonState.
        /// </summary>
        private static ButtonState GetMouseButtonState(MouseState state, MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:     return state.LeftButton;
                case MouseButtons.Middle:   return state.MiddleButton;
                case MouseButtons.Right:    return state.RightButton;
                case MouseButtons.XButton1: return state.XButton1;
                case MouseButtons.XButton2: return state.XButton2;

                default:
                    throw new ArgumentOutOfRangeException("button");
            }
        }
    }
}
