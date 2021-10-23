using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    /// <summary>
    /// Single input state of an instant
    /// </summary>
    public class WindowInputState
    {
        public WindowInputState(KeyboardState nativeInputState)
        {
            m_NativeInputState = nativeInputState;
        }

        /// <summary>
        /// Returns the native input state
        /// </summary>
        public KeyboardState NativeInputState
        {
            get
            {
                return m_NativeInputState;
            }
        }

        /// <summary>
        /// Returns shether the key is currently pressed
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyDown(Vex.Input.Keys key)
        {
            return m_NativeInputState.IsKeyDown((Keys)key);
        }

        /// <summary>
        /// Returns whether the is pressed for the first time
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyPressed(Vex.Input.Keys key)
        {
            return m_NativeInputState.IsKeyPressed((Keys)key);
        }

        /// <summary>
        /// Returns whether the key is released
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyReleased(Vex.Input.Keys key)
        {
            return m_NativeInputState.IsKeyReleased((Keys)key);
        }

        private readonly KeyboardState m_NativeInputState;
    }
}
