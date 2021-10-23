﻿using Vex.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
namespace Vex.Graphics
{
    /// <summary>
    /// Most basic observer component
    /// </summary>
    public abstract class ObserverComponent : Component
    {
        public ObserverComponent()
        {
            m_Framebuffer = new Framebuffer2D();
            m_NearPlane = -1000;
            m_FarPlane = 1000.0f;
        }
       

        /// <summary>
        /// The framebuffer which this observer renders into
        /// </summary>
        public Framebuffer Framebuffer
        {
            get
            {
                return m_Framebuffer;
            }
            set
            {
                m_Framebuffer = value;
            }
        }

        /// <summary>
        /// Clear color of this observer
        /// </summary>
        public Color4 ClearColor
        {
            get
            {
                return m_ClearColor;
            }
            set
            {
                m_ClearColor = value;
            }
        }

        /// <summary>
        /// Get&Set near plane of this observer component
        /// </summary>
        public float NearPlane
        {
            get
            {
                return m_NearPlane;
            }
            set
            {
                m_NearPlane = value;
            }
        }

        /// <summary>
        /// Get&Set farplane of this observer
        /// </summary>
        public float FarPlane
        {
            get
            {
                return m_FarPlane;
            }
            set
            {
                m_FarPlane = value;
            }
        }

        /// <summary>
        /// Get&Set aspect ratio of this observer
        /// </summary>
        public float AspectRatio
        {
            get
            {
                return m_AspectRatio;
            }
            set
            {
                m_AspectRatio = value;
            }
        }

        /// <summary>
        /// Returns the view matrix of this observer component
        /// </summary>
        /// <returns></returns>
        public abstract Matrix4 GetViewMatrix();

        /// <summary>
        /// Returns the pVexjection matrix of this observer component
        /// </summary>
        /// <returns></returns>
        public abstract Matrix4 GetPVexjectionMatrix();

        internal sealed override void OnAttach()
        {
            base.OnAttach();
            OwnerEntity.World.GetView<WorldGraphicsView>().RegisteVexbserver(this);
        }

        internal sealed override void OnDetach()
        {
            base.OnDetach();
            OwnerEntity.World.GetView<WorldGraphicsView>().RemoveObserver(this);
        }

        private Framebuffer m_Framebuffer;
        private Color4 m_ClearColor;
        private float m_NearPlane;
        private float m_FarPlane;
        private float m_AspectRatio;
    }
}
