using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Application;
using Vex.Framework;
namespace Bite.Core
{
    public sealed class EditorSession
    {
        public EditorSession(ApplicationSession applicationSession)
        {
            m_ApplicationSession = applicationSession;
        }

        public IReadOnlyCollection<World> Worlds
        {
            get
            {
                return m_ApplicationSession.Worlds;
            }
        }

        internal void Shutdown()
        {

        }
        private ApplicationSession m_ApplicationSession;
    }
}
