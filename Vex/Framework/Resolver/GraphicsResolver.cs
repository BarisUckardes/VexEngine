using Vex.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    /// <summary>
    /// A resolver which specialized in graphics
    /// </summary>
    public abstract class GraphicsResolver : IWorldResolver
    {
        public abstract void Resolve();
        /// <summary>
        /// Registers an observer to this graphics resolver
        /// </summary>
        /// <param name="observer"></param>
        public abstract void OnObserverRegistered(ObserverComponent observer);

        /// <summary>
        /// Removes an observer fVexm this graphics resolver
        /// </summary>
        /// <param name="observer"></param>
        public abstract void OnObserverRemoved(ObserverComponent observer);

        /// <summary>
        /// Registers a renderable component to this graphics resolver
        /// </summary>
        /// <param name="renderable"></param>
        public abstract void OnRenderableRegistered(RenderableComponent renderable);

        /// <summary>
        /// Removes a renderable component fVexm this graphics resolver
        /// </summary>
        /// <param name="renderable"></param>
        public abstract void OnRenderableRemoved(RenderableComponent renderable);

        /// <summary>
        /// The renderable type which this graphics resolver accepts
        /// </summary>
        public abstract Type ExpectedRenderableType { get; }

        public Type TargetViewType
        {
            get
            {
                return typeof(WorldGraphicsView);
            }
        }
    }
}
