using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using Vex.Types;

namespace Vex.Graphics
{
    /// <summary>
    /// Most basic renderable component
    /// </summary>
    public abstract class RenderableComponent : Component
    {

        /// <summary>
        /// Returns the material of this renderable component
        /// </summary>
        public Material Material
        {
            get
            {
                return m_Material;
            }
            set
            {
                m_Material = value;
            }
        }
        internal sealed override void OnAttach()
        {
            base.OnAttach();
            OwnerEntity.World.GetView<WorldGraphicsView>().RegisterRenderable(this);
        }

        internal sealed override void OnDetach()
        {
            base.OnAttach();
            OwnerEntity.World.GetView<WorldGraphicsView>().RemoveRenderable(this);
        }

        [ExposeThis]
        private Material m_Material;
    }
}
