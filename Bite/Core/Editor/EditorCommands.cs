using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{
    /// <summary>
    /// Set of editor specific commands
    /// </summary>
    public static class EditorCommands
    {
        /// <summary>
        /// Request a editor application close
        /// </summary>
        public static void SendEditorShutdownRequest()
        {
            s_Session.SendApplicationShutdownRequest("User closed manually");
        }

        /// <summary>
        /// Shutdows the singleton
        /// </summary>
        internal static void Shutdown()
        {
            s_Session = null;
        }

        /// <summary>
        /// An internal setter for session accessor
        /// </summary>
        /// <param name="session"></param>
        internal static void SetSession(EditorSession session)
        {
            s_Session = session;
        }
        private static EditorSession s_Session;
    }
}
