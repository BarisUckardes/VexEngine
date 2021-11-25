using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Graphics;

namespace Vex.Framework
{
    public class TestComponent : Component
    {
        public float MyFloat = 113;
        public Texture2D MyTexture = null;
        public ForwardMeshObserver MyObserver = null;
        public override bool ShouldTick => true;
        public override void OnLogicUpdate()
        {
            Spatial.Position += new System.Numerics.Vector3(0.001f, 0, 0);
        }
    }
}
