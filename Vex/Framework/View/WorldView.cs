using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    /// <summary>
    /// Represents a single view of a world
    /// </summary>
    public abstract class WorldView : VexObject
    {
        /// <summary>
        /// Returns the world of this view
        /// </summary>
        public World World
        {
            get
            {
                return m_World;
            }
            internal set
            {
                m_World = value;
            }
        }

        /// <summary>
        /// Returns the resolvers this world view has
        /// </summary>
        public abstract List<IWorldResolver> Resolvers { get; }

        /// <summary>
        /// Retunrs the expected base component type
        /// </summary>
        public abstract Type ExpectedBaseComponentType { get; }

        /// <summary>
        /// Register new resolver with type
        /// </summary>
        /// <param name="resolverType"></param>
        public abstract void RegisterResolver(Type resolverType);

        /// <summary>
        /// Remove new resolver with type
        /// </summary>
        /// <param name="resolverType"></param>
        public abstract void RemoveResolver(Type resolverType);

        /// <summary>
        /// Initializes world view internally
        /// </summary>
        /// <param name="components"></param>
        internal abstract void Initialize(List<Component> components);
        
        private World m_World;
    }
}
