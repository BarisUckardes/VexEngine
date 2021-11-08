﻿using System;
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

        public override void OnUpdate()
        {
            /*
             * Get all the session worlds
             */
            IReadOnlyCollection<World> worlds = Session.Worlds;

            /*
             * Iterate each world
             */
            for(int worldIndex = 0;worldIndex < worlds.Count;worldIndex++)
            {
                /*
                 * Get world graphics view
                 */
                WorldGraphicsView graphicsView = worlds.ElementAt(worldIndex).GetView<WorldGraphicsView>();

                /*
                 * Get resolvers
                 */
                GraphicsResolver[] graphicsResolvers = graphicsView.Resolvers;

                /*
                 * Iterate each resolver
                 */
                for (int resolverIndex = 0;resolverIndex < graphicsResolvers.Length;resolverIndex++)
                {
                    graphicsResolvers[resolverIndex].Resolve();
                }
            }
        }

        public override void OnEvent(PlatformEvent eventData)
        {

        }
    }
}
