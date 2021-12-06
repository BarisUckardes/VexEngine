using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    /// <summary>
    /// Base class for all custom logic resolver
    /// </summary>
    public abstract class LogicResolver : IWorldResolver
    {

        public void Resolve()
        {
            ResolveComponentLogic();
        }


        /// <summary>
        /// Expected component type of this resolver
        /// </summary>
        public abstract Type ExpectedComponentType { get; }

        public Type TargetViewType
        {
            get
            {
                return typeof(WorldLogicView);
            }
        }

        /// <summary>
        /// Called when registering a component
        /// </summary>
        /// <param name="component"></param>
        public abstract void OnRegisterComponent(Component component);

        /// <summary>
        /// Called when removing a component
        /// </summary>
        /// <param name="component"></param>
        public abstract void OnRemoveComponent(Component component);

        /// <summary>
        /// Called when resolving all the component logics
        /// </summary>
        public abstract void ResolveComponentLogic();

    }
}
