using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{
    public abstract class CoreCommand
    {
        public abstract void OnAttach();
        public abstract void OnDetach();

        public EditorSession EditorSession
        {
            get
            {
                return m_Session;
            }
            internal set
            {
                m_Session = value;
            }
                
        }

      
        private EditorSession m_Session;
    }
}
