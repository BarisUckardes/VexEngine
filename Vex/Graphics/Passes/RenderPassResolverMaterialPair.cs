using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    public class RenderPassResolverMaterialPair
    {
        /// <summary>
        /// The material
        /// </summary>
        public Material TargetMaterial
        {
            get
            {
                return m_TargetMaterial;
            }
            set
            {
                m_TargetMaterial = value;
            }
        }

        /// <summary>
        /// The resolver type
        /// </summary>
        public Type TargetResolver
        {
            get
            {
                return m_TargetResolver;
            }
            set
            {
                m_TargetResolver = value;
            }
        }

        private Material m_TargetMaterial;
        private Type m_TargetResolver;
    }
}
