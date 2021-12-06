using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{

    /// <summary>
    /// Interface for all world resolvers should implement
    /// </summary>
    public interface IWorldResolver
    {
        /// <summary>
        /// Resolves the data this world resolver has
        /// </summary>
        public void Resolve();

        /// <summary>
        /// Returns the view which this resolver targets
        /// </summary>
        public Type TargetViewType { get; }
    }
}
