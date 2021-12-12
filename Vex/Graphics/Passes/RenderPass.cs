using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    public sealed class RenderPass
    {
        public RenderPass()
        {
            m_ResolverMaterialPairs = new List<RenderPassResolverMaterialPair>();
            m_PassName = "Default pass name";
        }

        /// <summary>
        /// Returns the list of a resolver material pairs
        /// </summary>
        public List<RenderPassResolverMaterialPair> ResolverMaterialPairs
        {
            get
            {
                return new List<RenderPassResolverMaterialPair>(m_ResolverMaterialPairs);
            }
        }

        /// <summary>
        /// The target framebuffer which this pass will render into
        /// </summary>
        public Framebuffer TargetFramebuffer
        {
            get
            {
                return m_TargetFramebuffer;
            }
            set
            {
                m_TargetFramebuffer = value;
            }
        }

        /// <summary>
        /// The name of this pass
        /// </summary>
        public string PassName
        {
            get
            {
                return m_PassName;
            }
            set
            {
                m_PassName = value;
            }
        }

        public PolygonFillFace FillFace
        {
            get
            {
                return m_FillFace;
            }
            set
            {
                m_FillFace = value;
            }
        }

        public PolygonFillMethod FillMethod
        {
            get
            {
                return m_FillMethod;
            }
            set
            {
                m_FillMethod = value;
            }
        }
        public TriangleFrontFace FrontFace
        {
            get
            {
                return m_FrontFace;
            }
            set
            {
                m_FrontFace = value;
            }
        }
        public CullFace CullFace
        {
            get
            {
                return m_CullFace;
            }
            set
            {
                m_CullFace = value;
            }
        }
        public DepthFunction DepthFunction
        {
            get
            {
                return m_DepthFunction;
            }
            set
            {
                m_DepthFunction = value;
            }
        }
        public Vector4 ClearColor
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
        public float ClearDepthValue
        {
            get
            {
                return m_ClearDepthValue;
            }
            set
            {
                m_ClearDepthValue = value;
            }
        }
        public bool UseDepthTest
        {
            get
            {
                return m_DepthTest;
            }
            set
            {
                m_DepthTest = value;
            }
        }
        public bool UseClearColor
        {
            get
            {
                return m_UseClearColor;
            }
            set
            {
                m_UseClearColor = value;
            }
        }
        public bool UseClearDepth
        {
            get
            {
                return m_UseClearDepth;
            }
            set
            {
                m_UseClearDepth = value;
            }
        }



        /// <summary>
        /// Register new pass pair
        /// </summary>
        /// <param name="pair"></param>
        public void RegisterPair(in RenderPassResolverMaterialPair pair)
        {
            m_ResolverMaterialPairs.Add(pair);
        }

        private List<RenderPassResolverMaterialPair> m_ResolverMaterialPairs;
        private Framebuffer m_TargetFramebuffer;
        private PolygonFillFace m_FillFace;
        private PolygonFillMethod m_FillMethod;
        private TriangleFrontFace m_FrontFace;
        private CullFace m_CullFace;
        private DepthFunction m_DepthFunction;
        private Vector4 m_ClearColor;
        private float m_ClearDepthValue;
        private bool m_DepthTest;
        private bool m_UseClearColor;
        private bool m_UseClearDepth;
        private string m_PassName;
    }
}
