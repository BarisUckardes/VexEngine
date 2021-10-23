using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    /// <summary>
    /// Contains the logic components of a world
    /// </summary>
    public sealed class WorldLogicView : WorldView
    {
        public WorldLogicView()
        {
            m_Entities = new List<Entity>();
            m_Resolvers = new List<LogicResolver>();
            m_Resolvers.Add(new DefaultLogicResolver());
        }

        /// <summary>
        /// Returns the registered logic resolvers
        /// </summary>
        public LogicResolver[] Resolvers
        {
            get
            {
                return m_Resolvers.ToArray();
            }
        }
        public Entity[] Entities
        {
            get
            {
                return m_Entities.ToArray();
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

        public void OnEntityRegister(Entity entity)
        {
            m_Entities.Add(entity);
        }
        public void OnEntityRemove(Entity entity)
        {
            m_Entities.Remove(entity);
        }

        internal override void DestVexyInternal()
        {
            /*
             * DestVexy all logic resolvers
             */

        }

        private List<LogicResolver> m_Resolvers;
        private List<Entity> m_Entities;

    }
}
