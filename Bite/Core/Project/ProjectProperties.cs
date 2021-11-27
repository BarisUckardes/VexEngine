using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{
    public static class ProjectProperties
    {
        public static string ProjectName
        {
            get
            {
                return s_ProjectName;
            }
            internal set
            {
                s_ProjectName = value;
            }

        }
        private static string s_ProjectName;
    }
}
