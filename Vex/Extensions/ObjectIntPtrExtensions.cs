using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Extensions
{
    public static class ObjectIntPtrExtensions
    {
        public static GCHandle GetNativePtr(this object target,out IntPtr ptr)
        {
            GCHandle handle = GCHandle.Alloc(target);
            ptr = (IntPtr)handle;
            return handle;
        }
        public static int SizeOf(this object target)
        {
            return Marshal.SizeOf(target);
        }
    }
}
