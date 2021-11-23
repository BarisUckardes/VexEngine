using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    public class TestComponent : Component
    {
        public override bool ShouldTick => true;
        public override void OnLogicUpdate()
        {
            Spatial.Position += new System.Numerics.Vector3(0.001f, 0, 0);
        }
    }
}
