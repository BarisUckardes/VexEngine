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

        /// <summary>
        /// Returns whether the job is finished
        /// </summary>
        public bool IsJobFinished
        {
            get
            {
                /*
                 * Lock mutex
                 */
                m_Mutex.WaitOne();

                /*
                 * Collect state
                 */
                bool state = m_IsFinished;

                /*
                 * Release mutex
                 */
                m_Mutex.ReleaseMutex();

                return state;
            }
        }

        /// <summary>
        /// Starts the execution of the job
        /// </summary>
        public void ExecuteJob()
        {
            /*
            * Create new job object
            */
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoJob), m_Parameters);
        }

        /// <summary>
        /// Waits until the job finished(Freezes the thread which this function is called)
        /// </summary>
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

        /// <summary>
        /// Sets on finsihed delegate
        /// </summary>
        /// <param name="targetDelegate"></param>
        public void SetOnFinishDelegate(OnJobFinishedDelegate targetDelegate)
        {
            m_OnFinishEvent += targetDelegate;
        }

        /// <summary>
        /// Main do job method
        /// </summary>
        /// <param name="targetObject"></param>
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
