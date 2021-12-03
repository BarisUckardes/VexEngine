using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Threading
{
    /// <summary>
    /// Represents a batch of jobs
    /// </summary>
    public sealed class JobBatch
    {
        public JobBatch(List<Job> jobs)
        {
            /*
             * Get new list copy
             */
            m_Jobs = new List<Job>(jobs);
        }

        /// <summary>
        /// Stars the execution of all the jobs
        /// </summary>
        public void ExecuteAll()
        {
            /*
             * Iterate and execute all
             */
            for (int jobIndex = 0; jobIndex < m_Jobs.Count; jobIndex++)
                m_Jobs[jobIndex].ExecuteJob();
        }

        /// <summary>
        /// Waits for all the jobs
        /// </summary>
        public void WaitAllToFinish()
        {
            /*
             * Wait each one of them to finish
             */
            while(m_Jobs.Count !=0)
            {
                m_Jobs[0].WaitForFinish();
                m_Jobs.RemoveAt(0);
            }
        }

        private List<Job> m_Jobs;
    }
}
