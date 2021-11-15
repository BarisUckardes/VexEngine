using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Application
{
    public enum ApplicationExitType
    {
        Undefined = 0,
        Force = 1,
        SessionRequest = 2,
        WindowClose = 3
    }
}
