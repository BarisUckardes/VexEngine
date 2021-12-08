using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Vex.Extensions;
using Vex.Types;

namespace Vex.Graphics
{
    /// <summary>
    /// Custom sprite observer component
    /// </summary>
    public sealed class ForwardMeshObserver : ObserverComponent
    {
        public ForwardMeshObserver()
        {
            AspectRatio = 720.0f / 1280.0f;
            m_FieldOfView = 60;
            Name = "Forward Mesh Observer";
        }

        /// <summary>
        ///Get&Set ortho size of this sprite observer
        /// </summary>
        [FloatSlider(20,179)]
        public float FieldOfView
        {
            get
            {
                return m_FieldOfView;
            }
            set
            {
                m_FieldOfView = value;
            }
        }

       
        public override Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView((float)((Math.PI / 180) * m_FieldOfView), AspectRatio, NearPlane, FarPlane);
        }

        public override Matrix4 GetViewMatrix()
        {
            Vector3 forwardAbsolute = Spatial.Forward.GetAsOpenTK();
            return Matrix4.LookAt(new System.Numerics.Vector3(-Spatial.Position.X, Spatial.Position.Y, Spatial.Position.Z).GetAsOpenTK(), new System.Numerics.Vector3(-Spatial.Position.X, Spatial.Position.Y, Spatial.Position.Z).GetAsOpenTK() + new Vector3(-forwardAbsolute.X, forwardAbsolute.Y, forwardAbsolute.Z),new Vector3(0,1,0));
        }

        public override bool ShouldTick
        {
            get
            {
                return false;
            }
        }

        private float m_FieldOfView;
    }
}
