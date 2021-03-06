using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Platform;
namespace Vex.Input
{
    /// <summary>
    /// A class providing game input
    /// </summary>
    public static class GameInput
    {

        public static bool IsKeyPressed(Keys targetKey)
        {
            return s_KeyPressedEvents.Contains(targetKey);
        }
        public static bool IsKeyReleased(Keys targetKey)
        {
            return s_KeyReleasedEvents.Contains(targetKey);
        }
        public static bool IsKeyDown(Keys targetKey)
        {
            return s_DownKeyEvents.Contains(targetKey);
        }
        public static PlatformFileDropEvent GetFileDropEvent()
        {
            return s_FileDropEvent;
        }
        internal static void SetFileDropEvent(PlatformFileDropEvent eventData)
        {
            s_FileDropEvent = eventData;
        }
        internal static void SetEventBulk(List<Keys> pressedKeys,List<Keys> releasedKeys,List<Keys> downKeys)
        {
            s_KeyPressedEvents = pressedKeys;
            s_KeyReleasedEvents = releasedKeys;
            s_DownKeyEvents = downKeys;

        }

        private static List<Keys> s_KeyPressedEvents = new List<Keys>();
        private static List<Keys> s_KeyReleasedEvents = new List<Keys>();
        private static List<Keys> s_DownKeyEvents = new List<Keys>();
        private static PlatformFileDropEvent s_FileDropEvent;
    }
}
