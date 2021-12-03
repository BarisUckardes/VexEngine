using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Application
{
    /// <summary>
    /// Enums for application exit
    /// </summary>
    public enum ApplicationExitType
    {
        /// <summary>
        /// Source of exit not defined
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Force exit
        /// </summary>
        Force = 1,

        /// <summary>
        /// Request came from the session
        /// </summary>
        SessionRequest = 2,

        /// <summary>
        /// Application window closed
        /// </summary>
        WindowClose = 3
    }
}
