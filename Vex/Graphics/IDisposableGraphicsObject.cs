using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Interface for all the disposable graphics object
    /// </summary>
    public interface IDisposableGraphicsObject
    {
        /// <summary>
        /// Dispose graphics handles
        /// </summary>
        public void Dispose();

        /// <summary>
        /// Whether this graphics object disposed or not
        /// </summary>
        public bool IsDisposed { get; set; }
    }
}
