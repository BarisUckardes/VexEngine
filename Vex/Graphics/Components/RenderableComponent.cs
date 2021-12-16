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
        internal sealed override void OnAttachInternal()
        {
            base.OnAttachInternal();
            OwnerEntity.World.GetView<WorldGraphicsView>()?.RegisterGraphicsObject(this);

        }

        internal sealed override void OnDetachInternal()
        {
            base.OnDetachInternal();
            OwnerEntity.World.GetView<WorldGraphicsView>()?.RemoveGraphicsObject(this);
        }

        [ExposeThis]
        private Material m_Material;
    }
}
