using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    [TargetView(typeof(WorldLogicView))]
    public sealed class DefaultLogicResolver : LogicResolver
    {
        public DefaultLogicResolver()
        {
            m_Components = new List<Component>();
        }
        public override Type ExpectedComponentType
        {
            get
            {
                return typeof(Component);
            }
        }
        public override void OnRegisterComponent(Component component)
        {
            m_Components.Add(component);
        }

        public override void OnRemoveComponent(Component component)
        {
            m_Components.Remove(component);
        }

        public override void ResolveComponentLogic()
        {
            for(int i=0;i<m_Components.Count;i++)
            {
                m_Components[i].OnLogicUpdate();
            }
        }

        private List<Component> m_Components;
    }
}
