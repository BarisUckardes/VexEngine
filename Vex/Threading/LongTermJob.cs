using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vex.Threading
{
    /// <summary>
    /// Base class for long term jobs
    /// </summary>
    public abstract class LongTermJob
    {
        public LongTermJob(object parameters,int stackSize = 2)
        {
            /*
             * Create mutex
             */
            m_Mutex = new Mutex();

            /*
             * Set default finish state
             */
            m_Finished = false;

            /*
             * Set parameters
             */
            m_Parameters = parameters;

            /*
             * Create thread
             */
            m_Thread = new Thread(new ParameterizedThreadStart(DoJob), stackSize);
        }

        /// <summary>
        /// Returns if this job is finished or not
        /// </summary>
        public bool IsFinished
        {
            get
            {
                /*
                 * Lock mutex
                 */
                m_Mutex.WaitOne();

                /*
                 * Get state
                 */
                bool state = m_Finished;

                /*
                 * Release mutex
                 */
                m_Mutex.ReleaseMutex();

                return state;
            }
        }

        /// <summary>
        /// Sets new on finished delegate
        /// </summary>
        /// <param name="targetDelegate"></param>
        public void SetOnFinishedDelegate(OnJobFinishedDelegate targetDelegate)
        {
            m_JobFinishedDelegate += targetDelegate;
        }
           
        /// <summary>
        /// Executes new job
        /// </summary>
        public void ExecuteJob()
        {
            /*
             * Start thread
             */
            m_Thread.Start(m_Parameters);
        }

        /// <summary>
        /// Execute job with new parameters
        /// </summary>
        /// <param name="parameters"></param>
        public void ExecuteJob(object parameters)
        {
            /*
             * Start new thread
             */
            m_Thread.Start(parameters);

            /*
             * Set new parameters
             */
            m_Parameters = parameters;
        }

        /// <summary>
        /// Internal do job
        /// </summary>
        /// <param name="parameters"></param>
        private void DoJob(object parameters)
        {
            /*
             * Invoke core job
             */
            DoJobCore(parameters);

            /*
             * Set finished
             */
            m_Mutex.WaitOne();
            m_Finished = true;
            m_Mutex.ReleaseMutex();

            /*
             * Invoke delegate event
             */
            m_JobFinishedDelegate?.Invoke();
        }

        /// <summary>
        /// Waits for this thread to finish
        /// </summary>
        public void WaitForFinish()
        {
            /*
             * Wait for execution to finish
             */
            m_Thread.Join();
        }
        /// <summary>
        /// User do job implementation
        /// </summary>
        /// <param name="parameters"></param>
        protected abstract void DoJobCore(object parameters);

        private Thread m_Thread;
        private Mutex m_Mutex;
        private OnJobFinishedDelegate m_JobFinishedDelegate;
        private object m_Parameters;
        private bool m_Finished;
    }
}
