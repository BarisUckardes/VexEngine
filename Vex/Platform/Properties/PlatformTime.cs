using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    public static class PlatformTime
    {
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        private static extern void GetSystemTimePreciseAsFileTime(out long filetime);


        public static int GetCurrentMilliseconds()
        {
            long time = 0;
            GetSystemTimePreciseAsFileTime(out time);
            return DateTime.FromFileTimeUtc(time).Millisecond;
        }
    }
}
