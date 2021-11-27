using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{
    public static class EditorCommands
    {
        public static void SendEditorShutdownRequest()
        {
            s_Session.SendApplicationShutdownRequest("User closed manually");
        }

        internal static void Shutdown()
        {
            s_Session = null;
        }
        internal static void SetSession(EditorSession session)
        {
            s_Session = session;
        }
        private static EditorSession s_Session;
    }
}
