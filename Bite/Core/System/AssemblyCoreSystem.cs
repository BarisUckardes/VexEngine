using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{
    public sealed class AssemblyCoreSystem : CoreSystem
    {
        public override void OnAttach()
        {
            m_Registry = new AssemblyRegistry();
        }

        public override void OnDetach()
        {
            m_Registry = null;
        }

        public override void OnUpdate()
        {

        }

        private AssemblyRegistry m_Registry;
    }
}
