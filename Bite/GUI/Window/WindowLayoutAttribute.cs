using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class WindowLayoutAttribute : Attribute
    {
        public WindowLayoutAttribute(string name)
        {
            WindowName = name;
        }
        public string WindowName { get; set; }
    }
}
