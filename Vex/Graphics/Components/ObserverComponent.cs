using Vex.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Vex.Types;

namespace Vex.Graphics
{
    /// <summary>
    /// Most basic observer component
    /// </summary>
    public abstract class ObserverComponent : Component
    {
        /// <summary>
        /// The primal observer
        /// </summary>
        public static ObserverComponent PrimalObserver { get; protected set; }

        public ObserverComponent()
        {
            m_NearPlane = 0.001f;
            m_FarPlane = 1000.0f;
            m_AspectRatio = 1.0f;
            m_ClearColor = OpenTK.Mathematics.Color4.CornflowerBlue;
            m_RenderPasses = new List<RenderPass>();
            m_Framebuffer2DResources = new List<Framebuffer2D>();
        }
       

        /// <summary>
        /// Returns the list of framebuffer2D resources
        /// </summary>
        public List<Framebuffer2D> Framebuffer2DResources
        {
            get
            {
                return new List<Framebuffer2D>(m_Framebuffer2DResources);
            }
        }

        /// <summary>
        /// Returns renderpasses this observer has
        /// </summary>
        public List<RenderPass> RenderPasses
        {
            get
            {
                return new List<RenderPass>(m_RenderPasses);
            }
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

        /*
         * Returns whether this observer is the primal observer
         */
        public bool IsPrimal
        {
            get
            {
                return this == PrimalObserver;
            }
        }

        /*
        * Sets this observer as the primal observer of the world
        */
        public void SetAsPrimal()
        {
            PrimalObserver = this;
        }

        public void CreateFramebuffer2DResource(int width,int height,TextureFormat format,TextureInternalFormat internalFormat)
        {
            m_Framebuffer2DResources.Add(new Framebuffer2D(width, height, format, internalFormat));
        }
        public void CreateRenderPass(string passName)
        {
            RenderPass pass = new RenderPass();
            pass.PassName = passName;
            m_RenderPasses.Add(pass);
        }
        internal sealed override void OnAttachInternal()
        {
            /*
             * Call base component attach
             */
            base.OnAttachInternal();

            /*
             * Try register to an world graphics view (if any?)
             */
            OwnerEntity.World.GetView<WorldGraphicsView>()?.RegisterObserver(this);

            /*
             * Primat observer setup
             */
            if (PrimalObserver == null)
            {
                PrimalObserver = this;
            }
                
        }

        internal sealed override void OnDetachInternal()
        {
            /*
             * Call base component detach
             */
            base.OnDetachInternal();

            /*
             * Try remove from the owner world(if any?)
             */
            OwnerEntity.World.GetView<WorldGraphicsView>()?.RemoveObserver(this);

            /*
             * Primal observer setup
             */
            if(PrimalObserver == this)
            {
                Console.WriteLine("Primal observer removed " + OwnerEntity.Name);
                PrimalObserver = null;
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
        public abstract Matrix4 GetProjectionMatrix();

        private List<Framebuffer2D> m_Framebuffer2DResources;
        private List<RenderPass> m_RenderPasses;
        private Framebuffer m_Framebuffer;
        private Color4 m_ClearColor;
        [ExposeThis]
        private float m_NearPlane;
        [ExposeThis]
        private float m_FarPlane;
        [ExposeThis]
        private float m_AspectRatio;

    }
}
