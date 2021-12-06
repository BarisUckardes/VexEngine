using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;

namespace Vex.Framework
{
    /// <summary>
    /// Contains the logic components of a world
    /// </summary>
    public sealed class WorldLogicView : WorldView
    {
        public WorldLogicView()
        {
            m_Components = new List<Component>(1000);
            m_Resolvers = new List<LogicResolver>();
        }


        public override List<IWorldResolver> Resolvers
        {
            get
            {
                List<IWorldResolver> resolvers = new List<IWorldResolver>();
                foreach (LogicResolver resolver in m_Resolvers)
                    resolvers.Add(resolver);
                return resolvers;
            }
        }

        public override Type ExpectedBaseComponentType
        {
            get
            {
                return typeof(Component);
            }
        }

        /// <summary>
        /// Called when registering a new component
        /// </summary>
        /// <param name="component"></param>
        public void OnRegisterComponent(Component component)
        {
            for(int i=0;i<m_Resolvers.Count;i++)
            {
                m_Resolvers[i].OnRegisterComponent(component);
            }
        }

        /// <summary>
        /// Called when removing component component
        /// </summary>
        /// <param name="component"></param>
        public void OnRemoveComponent(Component component)
        {
            for(int i=0;i<m_Resolvers.Count;i++)
            {
                m_Resolvers[i].OnRemoveComponent(component);
            }
        }

        public override void RegisterResolver(Type resolverType)
        {
            /*
             * Create new logic resolver
             */
            LogicResolver resolver = Activator.CreateInstance(resolverType) as LogicResolver;

            /*
             * Broadcast components
             */
            for(int componentIndex = 0;componentIndex < m_Components.Count;componentIndex++)
            {
                /*
                 * Get component
                 */
                Component component = m_Components[componentIndex];


                /*
                 * Register
                 */
                resolver.OnRegisterComponent(component);
            }

            /*
             * Register resolver
             */
            m_Resolvers.Add(resolver);
        }

        public override void RemoveResolver(Type resolverType)
        {
            /*
             * Remove
             */
            for(int resolverIndex =0;resolverIndex < m_Resolvers.Count;resolverIndex++)
            {
                /*
                 * Get resolver
                 */
                LogicResolver resolver = m_Resolvers[resolverIndex];

                /*
                 * Validate and delete
                 */
                if(resolver.GetType() == resolverType)
                {
                    m_Resolvers.RemoveAt(resolverIndex);
                    return;
                }
            }
        }

        internal override void Initialize(List<Component> components)
        {
            /*
             * Iterate and validate components
             */
            foreach(Component component in components)
            {
                /*
                 * Validate tick
                 */
                if (!component.ShouldTick)
                    continue;

                /*
                 * Register
                 */
                m_Components.Add(component);
            }
        }

        private List<LogicResolver> m_Resolvers;
        private List<Component> m_Components;

    }
}
