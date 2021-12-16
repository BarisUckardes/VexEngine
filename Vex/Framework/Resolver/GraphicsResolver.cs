using Vex.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    /// <summary>
    /// The graphics object register delegate
    /// </summary>
    /// <param name="component"></param>
    public delegate void OnGraphicsComponentRegister(Component component);

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
        /// Returns the graphics object register informations of this resovler
        /// </summary>
        /// <returns></returns>
        public abstract List<GraphicsObjectRegisterInfo> GetGraphicsComponentRegisterInformations();


        public Type TargetViewType
        {
            get
            {
                return typeof(WorldGraphicsView);
            }
        }
    }
}
