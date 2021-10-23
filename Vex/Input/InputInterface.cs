using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Input
{
    /// <summary>
    /// Input interface for accessing in game
    /// </summary>
    public static class InputInterface
    {

        /// <summary>
        /// Returns whether specified key is down or not
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsKeyDown(Keys key)
        {
            return s_Frame.PressedKeys.Contains(key);
        }

        /// <summary>
        /// Retunrs the mouse position in pixels
        /// </summary>
        public static Vector2 MousePosition
        {
            get
            {
                return s_Frame.MousePosition;
            }
        }

        /// <summary>
        /// Internal setter for input frame
        /// </summary>
        /// <param name="frame"></param>
        internal static void SetInputFrame(in InputFrame frame)
        {
            s_Frame = frame;
        }

        private static InputFrame s_Frame;
    }
}
