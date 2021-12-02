using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{

    /// <summary>
    /// Attribute for marking a static method which registers as main menu item
    /// </summary>
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
