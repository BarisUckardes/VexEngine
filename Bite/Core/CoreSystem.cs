using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{
    public abstract class CoreSystem
    {
        public abstract void OnAttach();
        public abstract void OnDetach();
        public abstract void OnUpdate();
    }
}
