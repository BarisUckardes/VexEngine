using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slope.Editor
{
    internal abstract class GUILayout
    {
        public abstract void Initialize();
        public abstract void Finalize();
        public abstract void Render();
    }
}
