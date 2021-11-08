using Vex.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
namespace Vex.Engine
{

    /// <summary>
    /// Engine logic module
    /// </summary>
    public sealed class LogicModule : EngineModule
    {
        public override void OnAttach()
        {

        }

        public override void OnDetach()
        {

        }

        public override void OnEvent(PlatformEvent eventData)
        {

        }

        public override void OnUpdate()
        {
            /*
             * Get all the session worlds
             */
            IReadOnlyCollection<World> worlds = Session.Worlds;

            for(int worldIndex = 0;worldIndex < worlds.Count;worldIndex++)
            {
                /*
                 * Get world logic view
                 */
                WorldLogicView logicView = worlds.ElementAt(worldIndex).GetView<WorldLogicView>();

                /*
                 * Get logic resolvers
                 */
                LogicResolver[] logicResolvers = logicView.Resolvers;

                /*
                 * Iterate each resolver
                 */
                for(int resolverIndex = 0;resolverIndex<logicResolvers.Length; resolverIndex++)
                {
                    logicResolvers[resolverIndex].Resolve();
                }
            }
        }
    }
}
