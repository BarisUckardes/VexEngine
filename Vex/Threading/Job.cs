using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vex.Threading
{
    /// <summary>
    /// Delegate for job finish
    /// </summary>
    public delegate void OnJobFinishedDelegate();

    /// <summary>
    /// Base class for all jobs to implement
    /// </summary>
    public abstract class Job
    {
        protected Job(object parameters)
        {
            /*
             * Set finished to false as DEFAULT
             */
            m_IsFinished = false;

            /*
             * Create new mutex object
             */
            m_Mutex = new Mutex();

            /*
             * Set parameters
             */
            m_Parameters = parameters;
        }
        public void SetOnFinishDelegate(OnJobFinishedDelegate targetDelegate)
        {
            m_OnFinishEvent += targetDelegate;
        }
        public void ExecuteJob()
        {


            /*
            * Create new job object
            */
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoJob), m_Parameters);
        }
        public void WaitForFinish()
        {
            /*
             * Wait until the job finishes
             */
            while (true)
            {
                /*
                 * Wait for lock
                 */
                m_Mutex.WaitOne();

                /*
                 * Validate finish
                 */
                if (m_IsFinished)
                    break;

                /*
                 * Release lock
                 */
                m_Mutex.ReleaseMutex();
            }

            /*
             * Release lock
             */
            m_Mutex.ReleaseMutex();
        }

        private void DoJob(object targetObject)
        {
            /*
             * Do job on thread side
             */
            DoJobCore(targetObject);

            /*
             * Lock mutex on thread side
             */
            m_Mutex.WaitOne();

            /*
             * Set this job finished
             */
            m_IsFinished = true;

            /*
             * Release lock
             */
            m_Mutex.ReleaseMutex();

            /*
             * Invoke event
             */
            m_OnFinishEvent?.Invoke();

        }

        /// <summary>
        /// Job function implementation
        /// </summary>
        /// <param name="targetObject"></param>
        protected abstract void DoJobCore(object targetObject);

        private event OnJobFinishedDelegate m_OnFinishEvent;
        private object m_Parameters;
        private bool m_IsFinished;
        private Mutex m_Mutex;
    }
}
