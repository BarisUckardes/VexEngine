using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using Vex.Platform;

namespace Vex.Engine
{
    /// <summary>
    /// Engine graphics module
    /// </summary>
    public sealed class GraphicsModule : EngineModule
    {
        public override void OnAttach()
        {

        }

        public override void OnDetach()
        {

        }

        public override void OnUpdate(bool active)
        {
            /*
             * Get all the session worlds
             */
            World currentWorld = Session.CurrentWorld;

            /*
             * Validate world
             */
            if (currentWorld == null)
                return;

            /*
            * Get world graphics view
            */
            WorldGraphicsView graphicsView = currentWorld.GetView<WorldGraphicsView>();

            /*
             * Validate graphics view
             */
            if (graphicsView == null)
                return;

            /*
             * Get resolvers
             */
            List<IWorldResolver> graphicsResolvers = graphicsView.Resolvers;

            /*
             * Iterate each resolver
             */
            for (int resolverIndex = 0; resolverIndex < graphicsResolvers.Count; resolverIndex++)
            {
                graphicsResolvers[resolverIndex].Resolve();
            }
        }

        public override void OnEvent(PlatformEvent eventData)
        {

        }
    }
}
