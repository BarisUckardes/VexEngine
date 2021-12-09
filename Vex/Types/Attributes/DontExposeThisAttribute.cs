using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Types
{
    /// <summary>
    /// Prevents vex from seeing this property||field
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class DontExposeThis : Attribute
    {

    }
}
