using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class MainMenuItemAttribute : Attribute
    {
        public MainMenuItemAttribute(string category)
        {
            Category = category;
        }
        public string Category { get; set; }
    }
}
