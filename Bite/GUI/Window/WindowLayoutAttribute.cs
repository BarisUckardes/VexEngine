using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{

    /// <summary>
    /// An attribute for specifying the custom window name
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class WindowLayoutAttribute : Attribute
    {
        public WindowLayoutAttribute(string name)
        {
            WindowName = name;
        }

        /// <summary>
        /// The custom window name
        /// </summary>
        public string WindowName { get; set; }
    }
}
