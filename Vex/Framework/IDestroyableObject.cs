using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    /// <summary>
    /// Interface for all the disposable graphics object
    /// </summary>
    public interface IDestroyableObject
    {
        /// <summary>
        /// Dispose graphics handles
        /// </summary>
        public void Destroy();

        /// <summary>
        /// Whether this graphics object disposed or not
        /// </summary>
        public bool IsDestroyed { get; set; }
    }
}
