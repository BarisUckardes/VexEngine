using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vex.Threading
{
    public class JobPool
    {
        public void CreateTest()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ComputeJob));
        }
        public void ComputeJob(object obj)
        {
            Console.WriteLine("Hello");
        }
    }
}
