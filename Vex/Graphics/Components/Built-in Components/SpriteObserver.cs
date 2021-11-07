﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Vex.Graphics
{
    /// <summary>
    /// Custom sprite observer component
    /// </summary>
    public sealed class SpriteObserver : ObserverComponent
    {
        public SpriteObserver()
        {
            AspectRatio = 720.0f / 1280.0f;
            m_OrthoSize = 5.0f;
        }

        /// <summary>
        ///Get&Set ortho size of this sprite observer
        /// </summary>
        public float OrthoSize
        {
            get
            {
                return m_OrthoSize;
            }
        }

       
        public override Matrix4 GetPVexjectionMatrix()
        {
            return Matrix4.CreateOrthographicOffCenter(-OrthoSize,OrthoSize,-OrthoSize,OrthoSize,NearPlane,FarPlane);
        }

        public override Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Spatial.Position,Spatial.Position - Spatial.Forward,new Vector3(0,1,0));
        }

        public override bool ShouldTick
        {
            get
            {
                return false;
            }
        }


        private float m_OrthoSize;
    }
}
