using Vex.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using Vex.Profiling;

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
            Profiler.StartProfile();
            /*
             * Get all the session worlds
             */
            World currentWorld = Session.CurrentWorld;

            /*
             * Get world logic view
             */
            WorldLogicView logicView = currentWorld.GetView<WorldLogicView>();

            /*
             * Get logic resolvers
             */
            LogicResolver[] logicResolvers = logicView.Resolvers;

            /*
             * Iterate each resolver
             */
            for (int resolverIndex = 0; resolverIndex < logicResolvers.Length; resolverIndex++)
            {
                logicResolvers[resolverIndex].Resolve();
            }

            Profiler.EndProfile();
        }
    }
}
