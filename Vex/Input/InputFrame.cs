using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Vex.Input
{
    /// <summary>
    /// Struct for containing input data for single frame
    /// </summary>
    public readonly struct InputFrame
    {
        public InputFrame(in Keys[] pressedKeys,in Vector2 mousePosition)
        {
            PressedKeys = pressedKeys;
            MousePosition = mousePosition;
        }

        /// <summary>
        /// The keys are pressed in this frame
        /// </summary>
        public readonly Keys[] PressedKeys;

        /// <summary>
        /// Current mouse position in this frame
        /// </summary>
        public readonly Vector2 MousePosition;
    }
}
